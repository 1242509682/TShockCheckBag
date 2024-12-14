using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using static TShockAPI.GetDataHandlers;

namespace CheckBag
{
    [ApiVersion(2, 1)]
    public class CheckBag : TerrariaPlugin
    {
        #region 插件信息
        public override string Name => "检查背包(超进度物品检测)";
        public override string Author => "hufang360 羽学";
        public override string Description => "定时检查玩家背包，删除违禁物品，满足次数封禁对应玩家。";
        public override Version Version => new Version(2, 7, 1);
        #endregion

        #region 注册与销毁
        public CheckBag(Main game) : base(game) { Config = new Configuration(); }
        public override void Initialize()
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            LoadConfig();
            GeneralHooks.ReloadEvent += ReloadConfig;
            GetDataHandlers.ItemDrop.Register(OnItemDrop);
            GetDataHandlers.PlayerSlot.Register(OnPlayerSlot);
            GetDataHandlers.ChestItemChange.Register(OnChestItemChange);
            TShockAPI.Commands.ChatCommands.Add(new Command("cbag", Commands.CBCommand, "cbag", "检查背包")
            { HelpText = "检查背包" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GeneralHooks.ReloadEvent -= ReloadConfig;
                GetDataHandlers.ItemDrop.UnRegister(this.OnItemDrop);
                GetDataHandlers.PlayerSlot.UnRegister(OnPlayerSlot);
                GetDataHandlers.ChestItemChange.UnRegister(OnChestItemChange);
                TShockAPI.Commands.ChatCommands.Remove(new Command("cbag", Commands.CBCommand, "cbag", "检查背包")
                { HelpText = "检查背包" });
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 配置文件创建与重载
        public static Configuration Config;
        public string FilePath = Path.Combine(TShock.SavePath, "检查背包");
        private static void ReloadConfig(ReloadEventArgs args)
        {
            LoadConfig();
            args.Player.SendInfoMessage("[检查背包(超进度物品检测)]重新加载配置完毕。");
        }

        private static void LoadConfig()
        {
            Config = Configuration.Read();
            Config.Write();
        }
        #endregion

        #region 选中超进度物品的清理方法
        private void OnPlayerSlot(object sender, PlayerSlotEventArgs e)
        {
            var plr = e.Player;
            if (!plr.IsLoggedIn || plr.HasPermission("免检背包") ||
                !Config.Enabled || !Config.ClearPlayerSlot) return;

            var ClearItem = Config.GetClearItemIds();
            if (ClearItem.Contains(e.Type))
            {
                e.Stack = 0;
                plr.SelectedItem.TurnToAir();
                TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", plr.Index, e.Slot);
                Pun(e, plr);
            }
        }
        #endregion

        #region 清理扔出物品方法
        private void OnItemDrop(object sender, ItemDropEventArgs e)
        {
            var plr = e.Player;
            if (e == null || !Config.Enabled || !plr.IsLoggedIn || !plr.Active ||
                plr.HasPermission("免检背包") || !Config.ClearItemDrop ||
                Config.ExemptItems.Contains(e.Type))
                return;

            var ClearItem = Config.GetClearItemIds();
            if (ClearItem.Contains(e.Type) || Config.ClearTable.Contains(e.Type))
            {
                e.Handled = true;
                return;
            }

        }
        #endregion

        #region 清理放入箱子内物品方法
        private static void OnChestItemChange(object sender, ChestItemEventArgs e)
        {
            var plr = e.Player;
            if (e == null || !Config.Enabled || !plr.IsLoggedIn || !plr.Active ||
                Config.ExemptItems.Contains(e.Type) || plr.HasPermission("免检背包"))
                return;

            var ClearItem = Config.GetClearItemIds();
            if (Config.ClearChestItem)
            {
                if (ClearItem.Contains(e.Type) || Config.ClearTable.Contains(e.Type))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        #endregion

        #region 惩罚与播报方法
        private static void Pun(PlayerSlotEventArgs e, TSPlayer plr)
        {
            var desc = $"拥有超进度物品 [i/s{1}:{e.Type}]{Lang.GetItemNameValue(e.Type)} ";
            if (Ban.Trigger(plr.Name) < Config.CheckCount)
            {
                if (Config.Message)
                {
                    TShock.Utils.Broadcast($"玩家[c/FFCCFF:{plr.Name}]被检测到{desc}", 240, 255, 150); // 发送广播消息
                }

                if (Config.Logs)
                {
                    string logPath = Path.Combine(TShock.SavePath, "检查背包", "检查日志"); //写入日志的路径
                    Directory.CreateDirectory(logPath); // 创建日志文件夹
                    string logName = $"log {DateTime.Now.ToString("yyyy-MM-dd")}.txt"; //给日志名字加上日期
                    File.AppendAllLines(Path.Combine(logPath, logName), new string[] { DateTime.Now.ToString("u") +
                                $"玩家【{plr.Name}】被检测到{desc}，疑似作弊请注意！" }); //写入日志log
                }
            }
            else if (Config.Ban)
            {
                Ban.Remove(plr.Name);
                TSPlayer.All.SendInfoMessage($"{plr.Name}已被封禁！原因：{desc}。");
                plr.Disconnect($"你已被封禁！原因：拥有超进度物品 {Lang.GetItemNameValue(e.Type)} ");
                foreach (var ban in Config.Banlist)
                {
                    Ban.AddBan(plr, $"{desc}", ban);
                }
            }
        }
        #endregion

    }
}