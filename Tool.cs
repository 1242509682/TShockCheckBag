using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using TShockAPI;

namespace CheckBag
{
    internal class Tool
    {

        #region 获取所有Config里的物品ID表（除了全时段表）
        internal static IEnumerable<int> GetAllConfigItemIds()
        {
            return new[]
            {
                CheckBag.Config.ClearTable, CheckBag.Config.Goblins, CheckBag.Config.SlimeKing,
                CheckBag.Config.Boss1, CheckBag.Config.Boss2, CheckBag.Config.Boss3,
                CheckBag.Config.QueenBee, CheckBag.Config.Deerclops, CheckBag.Config.hardMode,
                CheckBag.Config.QueenSlime, CheckBag.Config.MechBossAny, CheckBag.Config.MechBoss,
                CheckBag.Config.Fishron, CheckBag.Config.EmpressOfLight, CheckBag.Config.PlantBoss,
                CheckBag.Config.GolemBoss, CheckBag.Config.AncientCultist, CheckBag.Config.Moonlord
            }.SelectMany(config => config.Keys);
        }
        #endregion

        #region 清理物品方法2
        public static TSPlayer RealPlayer { get; set; }
        public static void ClearItems(int radius, int[] ItemIDs, HashSet<int> exemptItems)
        {
            for (int i = 0; i < Terraria.Main.maxItems; i++)
            {
                var item = Terraria.Main.item[i];
                float dx = item.position.X - RealPlayer.X;
                float dy = item.position.Y - RealPlayer.Y;
                float Distance = dx * dx + dy * dy;

                if (exemptItems.Contains(item.netID)) continue;

                if (item.active && Distance <= radius * radius * 256f)
                {
                    if (ItemIDs.Contains(item.netID))
                    {
                        Terraria.Main.item[i].active = false;
                        Terraria.NetMessage.TrySendData((int)PacketTypes.PlayerUpdate, -1, -1, null, i);
                    }
                    else if (!ItemIDs.Contains(item.netID) && item.stack >= CheckBag.Config.ItemCount)
                    {
                        Terraria.Main.item[i].active = false;
                        Terraria.NetMessage.TrySendData((int)PacketTypes.PlayerUpdate, -1, -1, null, i);
                    }
                }
            }
        }
        #endregion

        #region 恋恋修复五号包方法-辅助加速清理物品
        public static void MMHook_PatchVersion_GetData(On.Terraria.MessageBuffer.orig_GetData args, MessageBuffer self, int start, int length, out int messageType)
        {
            try
            {
                if (self.readBuffer[start] == 5)
                {
                    using BinaryReader data = new(new MemoryStream(self.readBuffer, start + 1, length - 1));
                    var playerID = data.ReadByte();
                    if (self.whoAmI != playerID)
                    {
                        self.readBuffer[start] = byte.MaxValue;
                        args(self, start, length, out messageType);
                        return;
                    }

                    var slot = data.ReadInt16();
                    var stack = data.ReadInt16();
                    var prefix = data.ReadByte();
                    var type = data.ReadInt16();
                    var inv = Terraria.Main.player[playerID].inventory[slot];



                    if (inv == null)
                    {
                        args(self, start, length, out messageType);
                        return;
                    }

                    if (!inv.IsAir || (type != 0 && stack != 0))
                    {
                        if (inv.netID != type || inv.stack != stack || inv.prefix != prefix)
                        {
                            args(self, start, length, out messageType);
                            return;
                        }
                    }

                    self.readBuffer[start] = byte.MaxValue;
                    args(self, start, length, out messageType);
                    return;
                }
            }
            catch
            {
                var plr = TShock.Players[self.whoAmI];
                CheckBag.ClearPlayersItem(plr);
                CheckBag.SetItemStack(plr);
                //Terraria.NetMessage.TrySendData((int)PacketTypes.PlayerUpdate, -1, -1, null, self.whoAmI);
            }
            args(self, start, length, out messageType);
        }
        #endregion

        #region 查重
        internal static IEnumerable<int> ALL()
        {
            return new[]
            {
                CheckBag.Config.ClearTable,CheckBag.Config.Anytime, CheckBag.Config.Goblins, CheckBag.Config.SlimeKing,
                CheckBag.Config.Boss1, CheckBag.Config.Boss2, CheckBag.Config.Boss3,
                CheckBag.Config.QueenBee, CheckBag.Config.Deerclops, CheckBag.Config.hardMode,
                CheckBag.Config.QueenSlime, CheckBag.Config.MechBossAny, CheckBag.Config.MechBoss,
                CheckBag.Config.Fishron, CheckBag.Config.EmpressOfLight, CheckBag.Config.PlantBoss,
                CheckBag.Config.GolemBoss, CheckBag.Config.AncientCultist, CheckBag.Config.Moonlord
            }.SelectMany(config => config.Keys);
        }

        internal static List<int> FindDuplicateConfigItemIds()
        {
            IEnumerable<int> allIds = ALL();
            var idCounts = allIds.GroupBy(id => id).ToDictionary(group => group.Key, group => group.Count());
            List<int> duplicates = idCounts.Where(pair => pair.Value > 1).Select(pair => pair.Key).ToList();
            return duplicates;
        }
        #endregion
    }
}
