using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;


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
                    FindDuplicates(args);
                    break;
            }
        }
        #endregion

        #region ID查重
        public static void FindDuplicates(CommandArgs args)
        {
            List<int> duplicates = Tool.FindDuplicateConfigItemIds();
            if (duplicates.Count > 0)
            {
                TShock.Utils.Broadcast($"[检查背包] 存在以下重复的配置项ID:", 240, 255, 150);
                foreach (int duplicate in duplicates)
                {
                    var lang = Lang.GetItemNameValue(duplicate);
                    TShock.Utils.Broadcast($"| {duplicate} |{lang} 《tableName》", 240, 255, 150);
                }
            }
            else { TShock.Utils.Broadcast($"[检查背包] 没有重复配置项ID:", 240, 255, 150); }
        }
        #endregion

        #region 列出违规物品
        public static void ListItems(CommandArgs args)
        {
            static string FormatData(KeyValuePair<int, int> data)
            {
                var id = data.Key;
                var stack = data.Value;
                var itemName = Lang.GetItemNameValue(id);
                var itemDesc = stack > 1 ? $"{itemName}x{stack}" : itemName;
                return $"[i/s{stack}:{id}]{itemDesc}";
            }

            var lines = new List<string>();

            var datas = CheckBag.Config.Current();
            var lines2 = datas.Select(d => FormatData(d)).ToList();
            lines.AddRange(WarpLines(lines2));
            if (datas.Count > 0)
            {
                lines[0] = "[c/FFCCFF:当前进度：]" + lines[0];
            }

            if (!lines.Any())
            {
                if (CheckBag.Config.IsEmpty())
                {
                    args.Player.SendInfoMessage("你未配置任何违规物品数据！");
                }
                else
                {
                    args.Player.SendInfoMessage("没有符合当前进度的物品！");
                }
                return;
            }

            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int pageNumber))
            {
                return;
            }
            PaginationTools.SendPage(args.Player, pageNumber, lines, new PaginationTools.Settings
            {
                HeaderFormat = "违规物品 ({0}/{1})：",
                FooterFormat = "输入/cbag i {{0}}查看更多".SFormat(TShockAPI.Commands.Specifier)
            });
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
