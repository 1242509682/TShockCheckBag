using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using static CheckBag.CheckBag;

namespace CheckBag
{
    internal class Tool
    {
        #region 收集所有物品方法
        internal static void TotalAllItems(Player plr, List<Item> list)
        {
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
        }
        #endregion

        #region 移除违规物品方法
        public static void RemoveItem<T>(T[] items, Action<int> SendData, int id, int stack, Player plr) where T : Item
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (!item.IsAir && item.type == id && item.stack >= stack)
                {
                    item.TurnToAir();
                    SendData(i);
                }
            }
        }
        #endregion

        #region 查重
        internal static IEnumerable<int> ALL()
        {
            return new[]
            {
                Config.ClearTable,
                Config.Anytime,
                Config.Goblins, 
                Config.SlimeKing,
                Config.EyeofCthulhu,
                Config.Deerclops,
                Config.EaterofWorlds,
                Config.Boss2,
                Config.QueenBee,
                Config.SkeletronHead, 
                Config.WallofFlesh,
                Config.QueenSlime, 
                Config.TheDestroyer,
                Config.SkeletronPrime,
                Config.TheTwins,
                Config.MechBossAny, 
                Config.MechBoss,
                Config.Fishron,
                Config.PlantBoss,
                Config.Pumpking,
                Config.MourningWood,
                Config.IceQueen,
                Config.SantaNK1,
                Config.Everscream,
                Config.EmpressOfLight, 
                Config.GolemBoss,
                Config.Betsy,
                Config.MartianSaucer,
                Config.Cultist, 
                Config.Moonlord
            }.SelectMany(config => config.Keys);
        }

        internal static List<int> FindDups()
        {
            IEnumerable<int> allIds = ALL();
            var idCounts = allIds.GroupBy(id => id).ToDictionary(group => group.Key, group => group.Count());
            List<int> dups = idCounts.Where(pair => pair.Value > 1).Select(pair => pair.Key).ToList();
            return dups;
        }
        #endregion
    }
}
