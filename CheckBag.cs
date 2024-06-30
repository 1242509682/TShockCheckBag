using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public override string Name => "检查背包(超进度物品检测)";
        public override string Author => "hufang360 羽学";
        public override string Description => "定时检查玩家背包，删除违禁物品，满足次数封禁对应玩家。";
        public override Version Version => new Version(2, 2, 0, 0);

        string FilePath = Path.Combine(TShock.SavePath, "检查背包");
        internal static Configuration Config;
        int Count = -1;

        #region 注册与销毁
        public CheckBag(Main game) : base(game) { Config = new Configuration(); }
        public override void Initialize()
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            LoadConfig();
            PlayerUpdate += OnPlayerUP;
            GeneralHooks.ReloadEvent += LoadConfig;
            On.Terraria.MessageBuffer.GetData += Tool.MMHook_PatchVersion_GetData;
            TShockAPI.Commands.ChatCommands.Add(new Command("cbag", Commands.CBCommand, "cbag", "检查背包")
            { HelpText = "检查背包" });
            ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PlayerUpdate -= OnPlayerUP;
                GeneralHooks.ReloadEvent -= LoadConfig;
                On.Terraria.MessageBuffer.GetData -= Tool.MMHook_PatchVersion_GetData;
                TShockAPI.Commands.ChatCommands.Remove(new Command("cbag", Commands.CBCommand, "cbag", "检查背包")
                { HelpText = "检查背包" });
                ServerApi.Hooks.GameUpdate.Deregister(this, OnGameUpdate);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 配置文件创建与重载
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

        #region 清理玩家身边掉落物方法
        private static void OnPlayerUP(object sender, PlayerUpdateEventArgs args)
        {
            if (args == null ||
                !Config.Enable ||
                !Config.PlayerUp ||
                !args.Player.IsLoggedIn ||
                args.Player.HasPermission("免检背包")) return;

            if (args.Player.Active)
            {
                var ItemLists = Tool.GetAllConfigItemIds().ToArray();
                Tool.RealPlayer = args.Player;
                Tool.ClearItems(Config.ClearrRange, ItemLists,Config.ExemptItems);
            }
        }
        #endregion

        #region 游戏刷新
        private void OnGameUpdate(EventArgs args)
        {
            if (Count != -1 && Count < Config.DetectionInterval * Config.UpdateRate)
            {
                Count++;
                return;
            }
            Count = 0;

            TShock.Players.Where(plr => plr != null && plr.Active).ToList().ForEach(plr =>
            {
                if (!plr.IsLoggedIn || plr.HasPermission("免检背包") || !Config.Enable) return;
                ClearPlayersItem(plr);
            });
        }
        #endregion

        #region 检查玩家背包
        public static void ClearPlayersItem(TSPlayer args)
        {
            if (!args.IsLoggedIn || args.HasPermission("免检背包") || !Config.Enable)
            {
                return;
            }

            Player plr = args.TPlayer;
            Dictionary<int, int> dict = new();
            List<Item> list = new();
            list.AddRange(plr.inventory); // 背包,钱币/弹药,手持
            list.Add(plr.trashItem); // 垃圾桶
            list.AddRange(plr.dye); // 染料
            list.AddRange(plr.armor); // 装备,时装
            list.AddRange(plr.miscEquips); // 工具栏
            list.AddRange(plr.miscDyes); // 工具栏染料
            list.AddRange(plr.bank.item); // 储蓄罐
            list.AddRange(plr.bank2.item); // 保险箱
            list.AddRange(plr.bank3.item); // 护卫熔炉
            list.AddRange(plr.bank4.item); // 虚空保险箱
            for (int i = 0; i < plr.Loadouts.Length; i++)
            {
                // 装备1,装备2,装备3
                list.AddRange(plr.Loadouts[i].Armor); // 装备,时装
                list.AddRange(plr.Loadouts[i].Dye); // 染料
            }
            list.RemoveAll(i => i.IsAir); //移除所有的空白物品
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
                    string itemName = Lang.GetItemNameValue(id);
                    string itemDesc = stack > 1 ? $"{itemName}x{stack}" : itemName;
                    string opDesc = isCurrent ? "拥有超进度物品" : "拥有";
                    var desc = $"{opDesc}[i/s{stack}:{id}]{itemDesc}";

                    if (Ban.Trigger(name) <= Config.WarningCount)
                    {
                        var trash = plr.trashItem;
                        if (!trash.IsAir && trash.type == id && trash.stack >= stack)
                        {
                            trash.TurnToAir();
                            args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.TrashItem);
                        }

                        for (int i = 0; i < plr.inventory.Length; i++)
                        {
                            var invItem = plr.inventory[i];
                            if (!invItem.IsAir && invItem.type == id && invItem.stack >= stack)
                            {
                                invItem.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Inventory0 + i);
                            }
                        }

                        for (int i = 0; i < plr.bank.item.Length; i++)
                        {
                            var bank = plr.bank.item[i];
                            if (!bank.IsAir && bank.type == id && bank.stack >= stack)
                            {
                                bank.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Bank1_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.bank2.item.Length; i++)
                        {
                            var bank2 = plr.bank2.item[i];
                            if (!bank2.IsAir && bank2.type == id && bank2.stack >= stack)
                            {
                                bank2.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Bank2_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.bank3.item.Length; i++)
                        {
                            var bank3 = plr.bank3.item[i];
                            if (!bank3.IsAir && bank3.type == id && bank3.stack >= stack)
                            {
                                bank3.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Bank3_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.bank4.item.Length; i++)
                        {
                            var bank4 = plr.bank4.item[i];
                            if (!bank4.IsAir && bank4.type == id && bank4.stack >= stack)
                            {
                                bank4.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Bank4_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.armor.Length; i++)
                        {
                            var armor = plr.armor[i];
                            if (!armor.IsAir && armor.type == id && armor.stack >= stack)
                            {
                                armor.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Armor0 + i);
                            }
                        }

                        for (int i = 0; i < plr.Loadouts[0].Armor.Length; i++)
                        {
                            var armorL1 = plr.Loadouts[0].Armor[i];
                            if (!armorL1.IsAir && armorL1.type == id && armorL1.stack >= stack)
                            {
                                armorL1.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Loadout1_Armor_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.Loadouts[1].Armor.Length; i++)
                        {
                            var armorL2 = plr.Loadouts[1].Armor[i];
                            if (!armorL2.IsAir && armorL2.type == id && armorL2.stack >= stack)
                            {
                                armorL2.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Loadout2_Armor_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.Loadouts[2].Armor.Length; i++)
                        {
                            var armorL3 = plr.Loadouts[2].Armor[i];
                            if (!armorL3.IsAir && armorL3.type == id && armorL3.stack >= stack)
                            {
                                armorL3.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Loadout3_Armor_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.miscEquips.Length; i++)
                        {
                            var misc = plr.miscEquips[i];
                            if (!misc.IsAir && misc.type == id && misc.stack >= stack)
                            {
                                misc.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Misc0 + i);
                            }
                        }

                        for (int i = 0; i < plr.miscDyes.Length; i++)
                        {
                            var miscDyes = plr.miscDyes[i];
                            if (!miscDyes.IsAir && miscDyes.type == id && miscDyes.stack >= stack)
                            {
                                miscDyes.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.MiscDye0 + i);
                            }
                        }

                        for (int i = 0; i < plr.dye.Length; i++)
                        {
                            var Dye = plr.dye[i];
                            if (!Dye.IsAir && Dye.type == id && Dye.stack >= stack)
                            {
                                Dye.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Dye0 + i);
                            }
                        }

                        for (int i = 0; i < plr.Loadouts[0].Dye.Length; i++)
                        {
                            var DyeL1 = plr.Loadouts[0].Dye[i];
                            if (!DyeL1.IsAir && DyeL1.type == id && DyeL1.stack >= stack)
                            {
                                DyeL1.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Loadout1_Dye_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.Loadouts[1].Dye.Length; i++)
                        {
                            var DyeL2 = plr.Loadouts[1].Dye[i];
                            if (!DyeL2.IsAir && DyeL2.type == id && DyeL2.stack >= stack)
                            {
                                DyeL2.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Loadout2_Dye_0 + i);
                            }
                        }

                        for (int i = 0; i < plr.Loadouts[2].Dye.Length; i++)
                        {
                            var DyeL3 = plr.Loadouts[2].Dye[i];
                            if (!DyeL3.IsAir && DyeL3.type == id && DyeL3.stack >= stack)
                            {
                                DyeL3.TurnToAir();
                                args.SendData(PacketTypes.PlayerSlot, "", args.Index, PlayerItemSlotID.Loadout3_Dye_0 + i);
                            }
                        }

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
                            Ban.AddBan(name, $"{opDesc}{itemDesc}", Config.BanTime * 60);
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


    }
}