using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using static CheckBag.Tool;
using static TShockAPI.GetDataHandlers;
using static TerrariaApi.Server.ServerApi;

namespace CheckBag
{
    [ApiVersion(2, 1)]
    public class CheckBag : TerrariaPlugin
    {
        #region 插件信息
        public override string Name => "检查背包(超进度物品检测)";
        public override string Author => "hufang360 羽学";
        public override string Description => "定时检查玩家背包，删除违禁物品，满足次数封禁对应玩家。";
        public override Version Version => new Version(2, 5, 0, 0); 
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
            PlayerUpdate += OnPlayerUpdate;
            GeneralHooks.ReloadEvent += LoadConfig;
            Hooks.GameUpdate.Register(this, OnGameUpdate);
            TShockAPI.Commands.ChatCommands.Add(new Command("cbag", Commands.CBCommand, "cbag", "检查背包")
            { HelpText = "检查背包" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PlayerUpdate -= OnPlayerUpdate;
                GeneralHooks.ReloadEvent -= LoadConfig;
                Hooks.GameUpdate.Deregister(this, OnGameUpdate);
                TShockAPI.Commands.ChatCommands.Remove(new Command("cbag", Commands.CBCommand, "cbag", "检查背包")
                { HelpText = "检查背包" });
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 配置文件创建与重载
        public static Configuration Config;
        string FilePath = Path.Combine(TShock.SavePath, "检查背包");
        private static void LoadConfig(ReloadEventArgs args = null)
        {
            Config = Configuration.Read();
            Config.Write();
            if (args != null && args.Player != null)
            {
                args.Player.SendSuccessMessage("[检查背包]重新加载配置完毕。");
            }
        }
        #endregion

        #region 游戏刷新 5秒遍历一次所有在线玩家所有储物空间
        int Count = -1;
        private void OnGameUpdate(EventArgs args)
        {
            if (Count != -1 && Count < Config.Interval * Config.UpdateRate)
            {
                Count++;
                return;
            }
            Count = 0;

            TShock.Players.Where(plr => plr != null && plr.Active).ToList().ForEach(plr =>
            {
                if (!plr.IsLoggedIn || plr.HasPermission("免检背包") || !Config.Enable) return;
                ClearPlayersSlot(plr);
                SetItemStack(plr);
            });
        }
        #endregion

        #region 检查超进度并清理方法
        public static void ClearPlayersSlot(TSPlayer args)
        {
            if (!args.IsLoggedIn || args.HasPermission("免检背包") || !Config.Enable)
            {
                return;
            }

            Player plr = args.TPlayer;
            Dictionary<int, int> dict = new();
            List<Item> list = new();
            TotalAllItems(plr, list); //统计收集到的所有物品
            list.RemoveAll(i => i.IsAir); //移除所有空物品
            list.Where(item => item != null && item.active).ToList().ForEach(item =>
            {
                if (dict.ContainsKey(item.netID))
                {
                    dict[item.netID] += item.stack;
                }
                else
                {
                    dict.Add(item.netID, item.stack);
                }
            });

            bool Check(Dictionary<int, int> List, bool isCurrent)
            {
                KeyValuePair<int, int>? data = null;
                foreach (var Limit in List)
                {
                    int id = Limit.Key;
                    int stack = Limit.Value;

                    if (dict.ContainsKey(id) && dict[id] >= stack)
                    {
                        data = Limit;
                        break;
                    }
                }

                if (data != null)
                {
                    var name = args.Name;
                    var id = data.Value.Key;
                    var stack = data.Value.Value;
                    var itemName = Lang.GetItemNameValue(id);
                    var itemDesc = stack > 1 ? $"{itemName}x{stack}" : itemName;
                    var opDesc = isCurrent ? "拥有超进度物品" : "拥有";
                    var desc = $"{opDesc}[i/s{stack}:{id}]{itemDesc}";

                    if (Ban.Trigger(name) <= Config.CheckCount)
                    {
                        var trash = plr.trashItem;
                        if (!trash.IsAir && trash.type == id && trash.stack >= stack)
                        {
                            trash.TurnToAir();
                            args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.TrashItem);
                        }

                        RemoveItem(plr.inventory, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Inventory0 + i), id, stack, plr);
                        RemoveItem(plr.bank.item, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Bank1_0 + i), id, stack, plr);
                        RemoveItem(plr.bank2.item, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Bank2_0 + i), id, stack, plr);
                        RemoveItem(plr.bank3.item, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Bank3_0 + i), id, stack, plr);
                        RemoveItem(plr.bank4.item, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Bank4_0 + i), id, stack, plr);
                        RemoveItem(plr.armor, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Armor0 + i), id, stack, plr);
                        RemoveItem(plr.Loadouts[0].Armor, i => args.SendData(PacketTypes.PlayerSlot,"",
                            args.Index, PlayerItemSlotID.Loadout1_Armor_0 + i), id, stack, plr);
                        RemoveItem(plr.Loadouts[1].Armor, i => args.SendData(PacketTypes.PlayerSlot,"",
                            args.Index, PlayerItemSlotID.Loadout2_Armor_0 + i), id, stack, plr);
                        RemoveItem(plr.Loadouts[2].Armor, i => args.SendData(PacketTypes.PlayerSlot,"",
                            args.Index, PlayerItemSlotID.Loadout3_Armor_0 + i), id, stack, plr);
                        RemoveItem(plr.miscEquips, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Misc0 + i), id, stack, plr);
                        RemoveItem(plr.miscDyes, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.MiscDye0 + i), id, stack, plr);
                        RemoveItem(plr.dye, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Dye0 + i), id, stack, plr);
                        RemoveItem(plr.Loadouts[0].Dye, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Loadout1_Dye_0 + i), id, stack, plr);
                        RemoveItem(plr.Loadouts[1].Dye, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Loadout2_Dye_0 + i), id, stack, plr);
                        RemoveItem(plr.Loadouts[2].Dye, i => args.SendData(PacketTypes.PlayerSlot, "",
                            args.Index, PlayerItemSlotID.Loadout3_Dye_0 + i), id, stack, plr);

                        if (Config.Message)
                        {
                            string tips = stack > 1 ? "请减少数量" : "请销毁";
                            TSPlayer.All.SendSuccessMessage($"玩家[c/FFCCFF:{name}]被检测到{desc}，疑似作弊请注意！"); // 发送广播消息
                            Console.WriteLine($"玩家[{name}]被检测到{desc}，疑似作弊请注意！"); // 控制台输出
                        }

                        if (Config.Logs)
                        {
                            string logFolderPath = Path.Combine(TShock.SavePath, "检查背包", "检查日志"); //写入日志的路径
                            Directory.CreateDirectory(logFolderPath); // 创建日志文件夹
                            string logFileName = $"log {DateTime.Now.ToString("yyyy-MM-dd")}.txt"; //给日志名字加上日期
                            File.AppendAllLines(Path.Combine(logFolderPath, logFileName), new string[] { DateTime.Now.ToString("u") + $"玩家【{name}】被检测到{desc}，疑似作弊请注意！" }); //写入日志log
                        }
                    }
                    else
                    {
                        if (Config.Ban)
                        {
                            args.SetBuff(156, 180, true);
                            Ban.Remove(name);
                            TSPlayer.All.SendInfoMessage($"{name}已被封禁！原因：{desc}。");
                            args.Disconnect($"你已被封禁！原因：{opDesc}{itemDesc}。");
                            foreach (var ban in Config.Banlist)
                            {
                                Ban.AddBan(args, $"{opDesc}{itemDesc}", ban);
                            }
                            return false;
                        }
                    }

                }
                return true;
            }
            if (!Check(Config.Anytime, false)) return;
            Check(Config.Current(), true);
        }
        #endregion

        #region 清理玩家身边掉落物方法
        public static TSPlayer RealPlayer { get; set; }
        private static void OnPlayerUpdate(object sender, PlayerUpdateEventArgs args)
        {
            if (args == null ||
                !Config.Enable ||
                !args.Player.IsLoggedIn ||
                args.Player.HasPermission("免检背包")) return;

            if (args.Player.Active && Config.ClearItemDrop)
            {
                var ItemList = Config.GetClearItemIds();
                RealPlayer = args.Player;
                ClearItemsDown(Config.ClearRange, ItemList, Config.ExemptItems);
            }
        }
        #endregion

        #region 清理掉落物方法
        public static void ClearItemsDown(int radius, List<int> ItemList, HashSet<int> exempt)
        {
            for (int i = 0; i < Terraria.Main.maxItems; i++)
            {
                var item = Terraria.Main.item[i];
                float dx = item.position.X - RealPlayer.X;
                float dy = item.position.Y - RealPlayer.Y;
                float Distance = dx * dx + dy * dy;

                if (exempt.Contains(item.netID))
                {
                    continue;
                }

                if (item.active && Distance <= radius * radius * 256f)
                {
                    if (ItemList.Contains(item.netID) || Config.ClearTable.ContainsKey(item.netID) || 
                        (Config.MaxStackToAir && item.stack >= Config.MaxStackLimit))
                    {
                        Terraria.Main.item[i].active = false;
                        Terraria.NetMessage.TrySendData((int)PacketTypes.PlayerUpdate, -1, -1, null, i);
                    }

                    else if (Config.MaxStackToAir && item.stack >= Config.MaxStackLimit)
                    {
                        Terraria.Main.item[i].active = false;
                        Terraria.NetMessage.TrySendData((int)PacketTypes.PlayerUpdate, -1, -1, null, i);
                    }
                }
            }
        }
        #endregion

        #region 设置或清理所有超数量的物品
        public static void SetItemStack(TSPlayer args)
        {
            if (!args.IsLoggedIn || args.HasPermission("免检背包") || !Config.Enable)
            {
                return;
            }

            Player plr = args.TPlayer;

            Action<Item[], int> PROC = (items, slotBase) =>
            {
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (!item.IsAir && item.stack >= Config.MaxStackLimit)
                    {
                        if (Config.MaxStackToAir)
                        {
                            item.TurnToAir();
                            args.SendData(PacketTypes.PlayerSlot, "", args.Index, slotBase + i);
                        }
                        else if (Config.SetMaxStack)
                        {
                            item.stack = Config.MaxStackLimit;
                            args.SendData(PacketTypes.PlayerSlot, "", args.Index, slotBase + i);
                        }
                    }
                }
            };

            // 处理玩家物品栏 存钱罐 保险箱 虚空袋 护卫熔炉
            PROC(plr.inventory, PlayerItemSlotID.Inventory0);
            PROC(plr.bank.item, PlayerItemSlotID.Bank1_0);
            PROC(plr.bank2.item, PlayerItemSlotID.Bank2_0);
            PROC(plr.bank3.item, PlayerItemSlotID.Bank3_0);
            PROC(plr.bank4.item, PlayerItemSlotID.Bank4_0);
        }
        #endregion

    }
}