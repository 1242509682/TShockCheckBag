using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;
using static CheckBag.CheckBag;


namespace CheckBag
{
    internal class Commands
    {
        #region 指令
        internal static void CBCommand(CommandArgs args)
        {
            TSPlayer op = args.Player;
            void Help()
            {
                List<string> lines = new()
                {
                    "/cbag ban， 列出封禁记录",
                    "/cbag item，列出违规物品",
                    "/cbag dup， ID查重",
                };
                op.SendInfoMessage(string.Join("\n", lines));
            }

            if (args.Parameters.Count == 0)
            {
                op.SendErrorMessage("语法错误，输入 /cbag help 查询用法");
                return;
            }

            switch (args.Parameters[0].ToLowerInvariant())
            {
                // 帮助
                case "help":
                case "h":
                case "帮助":
                    Help();
                    break;

                // 查看封禁
                case "ban":
                case "b":
                    Ban.ListBans(args);
                    break;

                // 物品
                case "item":
                case "i":
                    ListItems(args);
                    break;

                // 查重
                case "dup":
                case "d":
                    FindDup(args);
                    break;
            }
        }
        #endregion

        #region 列出违规物品
        public static void ListItems(CommandArgs args)
        {
            static string FormatData(int id)
            {
                var itemName = Lang.GetItemNameValue(id);
                return $"[i/s1:{id}]{itemName}";
            }

            var lines = new List<string>();
            var itemIds = CheckBag.Config.GetClearItemIds();
            if (itemIds != null && itemIds.Any())
            {
                var formattedItems = itemIds.Select(id => FormatData(id)).ToList();
                lines.AddRange(WarpLines(formattedItems));
                lines[0] = "[c/FFCCFF:当前超进度物品为]：" + lines[0];
            }

            if (!lines.Any())
            {
                return;
            }

            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int page))
            {
                return;
            }

            PaginationTools.SendPage(args.Player, page, lines, new PaginationTools.Settings
            {
                HeaderFormat = "违规物品 ({0}/{1})：",
                FooterFormat = "输入/cbag i {{0}}查看更多".SFormat(TShockAPI.Commands.Specifier)
            });
        }
        #endregion

        #region ID查重
        public static void FindDup(CommandArgs args)
        {
            List<int> dups = FindDups();
            if (dups.Count > 0)
            {
                TShock.Utils.Broadcast($"重复的物品ID:", 240, 255, 150);
                foreach (int id in dups)
                {
                    var lang = Lang.GetItemNameValue(id);
                    TShock.Utils.Broadcast($"| {id} | {lang}", 240, 255, 150);
                }
            }
            else { TShock.Utils.Broadcast($"没有重复物品ID:", 240, 255, 150); }
        }
        #endregion

        #region 合并哈希表
        internal static IEnumerable<int> GetAllIds()
        {
            return new[]
            {
                Config.ClearTable,
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
            }.SelectMany(set => set);
        }

        internal static List<int> FindDups()
        {
            var allIds = GetAllIds().ToList();
            var duplicates = allIds.GroupBy(id => id)
                                   .Where(group => group.Count() > 1)
                                   .Select(group => group.Key)
                                   .ToList();
            return duplicates;
        }
        #endregion

        #region 字符串换行
        /// <param name="lines"></param>
        /// <param name="column">列数，1行显示多个</param>
        /// <returns></returns>
        private static List<string> WarpLines(List<string> lines, int column = 15)
        {
            List<string> li1 = new();
            List<string> li2 = new();
            foreach (var line in lines)
            {
                if (li2.Count % column == 0)
                {
                    if (li2.Count > 0)
                    {
                        li1.Add(string.Join("\n", li2));
                        li2.Clear();
                    }
                }
                li2.Add(line);
            }
            if (li2.Any())
            {
                li1.Add(string.Join("\n", li2));
            }
            return li1;
        }
        #endregion
    }
}
