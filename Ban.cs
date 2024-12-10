using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using TShockAPI.DB;

namespace CheckBag
{
    public class Ban
    {
        static string BanningUser = "检查背包";
        static Dictionary<string, int> bans = new();


        /// <summary>
        /// 触发规则
        /// </summary>
        public static int Trigger(string name)
        {
            if (bans.ContainsKey(name))
            {
                bans[name]++;
            }
            else
            {
                bans.Add(name, 1);
            }
            return bans[name];
        }


        /// <summary>
        /// 移除记录
        /// </summary>
        public static void Remove(string name)
        {
            if (bans.ContainsKey(name))
            {
                bans.Remove(name);
            }
        }


        /// <summary>
        /// 添加封禁
        /// </summary>
        public static AddBanResult AddBan(TSPlayer plr, string reason, Configuration.Bandata ban)
        {

            DateTime stop = DateTime.UtcNow.AddSeconds(ban.BanTime);
            var mess = new StringBuilder();

            AddBanResult acc = null!, uuid = null!, ip = null!;
            bool flag = true;

            // 根据配置决定是否执行封禁操作
            if (ban.BanAccount)
            {
                // 插入账号封禁
                acc = TShock.Bans.InsertBan("acc:" + plr.Name, reason, BanningUser, DateTime.UtcNow, stop);
                if (acc.Ban == null)
                {
                    flag = false;
                    mess.Append($"账号封禁失败: {acc.Message}; ");
                }
            }

            if (ban.BanIP)
            {
                // 插入IP封禁
                ip = TShock.Bans.InsertBan("ip:" + plr.IP, reason, BanningUser, DateTime.UtcNow, stop);
                if (ip.Ban == null)
                {
                    flag = false;
                    mess.Append($"IP封禁失败: {ip.Message}; ");
                }
            }

            if (ban.BanUUID)
            {
                // 插入UUID封禁
                uuid = TShock.Bans.InsertBan("uuid:" + plr.UUID, reason, BanningUser, DateTime.UtcNow, stop);
                if (uuid.Ban == null)
                {
                    flag = false;
                    mess.Append($"UUID封禁失败: {uuid.Message}; ");
                }
            }

            if (flag)
            {
                // 获取封禁ID
                string accBanId = ban.BanAccount ? acc.Ban.TicketNumber.ToString() : "";
                string ipBanId = ban.BanIP ? ip.Ban.TicketNumber.ToString() : "";
                string uuidBanId = ban.BanUUID ? uuid.Ban.TicketNumber.ToString() : "";

                // 记录封禁ID
                mess.Clear();
                mess.Append(BanningUser + " 已封禁 ").Append(plr.Name).Append(" ");
                if (ban.BanAccount) mess.Append($"acc:{accBanId}\n");
                if (ban.BanUUID) mess.Append($"UUID:{uuidBanId}\n");
                if (ban.BanIP) mess.Append($"IP:{ipBanId}\n");
                mess.Append($"依次输入以上ID进行解封：/ban del {accBanId} {uuidBanId} {ipBanId}\n" +
                    $"或通知该玩家 等待{ban.BanTime}秒 自动解封");

                TShock.Log.ConsoleInfo(mess.ToString());
                return acc ?? uuid ?? ip; // 返回第一个成功的结果
            }
            else
            {
                // 记录失败信息
                mess.Insert(0, "封禁" + plr.Name + "失败！原因: ");
                TShock.Log.ConsoleInfo(mess.ToString());
                return new AddBanResult { Message = mess.ToString() };
            }

        }


        /// <summary>
        /// 列出封禁
        /// </summary>
        public static void ListBans(CommandArgs args)
        {
            var bans = (
                from ban in TShock.Bans.Bans
                where ban.Value.BanningUser == BanningUser
                orderby ban.Value.ExpirationDateTime descending
                select ban
                ).ToList();
            var lines = new List<string>();
            var flag = false;
            foreach (var ban in bans)
            {
                if (!flag && (ban.Value.ExpirationDateTime <= DateTime.UtcNow))
                {
                    lines.Add("----下面的记录都已失效----");
                    flag = true;
                }
                lines.Add($"{ban.Value.Identifier.Substring(4)}, 截止：{ban.Value.ExpirationDateTime.ToLocalTime():yyyy-dd-HH hh:mm:ss}, 原因：{ban.Value.Reason}, 解封：/ban del {ban.Key}");
            }

            if (!lines.Any())
            {
                args.Player.SendInfoMessage("没有记录！看来没人作弊(*^▽^*)");
                return;
            }

            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int pageNumber))
            {
                return;
            }
            PaginationTools.SendPage(args.Player, pageNumber, lines, new PaginationTools.Settings
            {
                MaxLinesPerPage = 15, // 每页显示15行
                HeaderFormat = "封禁记录 ({0}/{1})：",
                FooterFormat = "输入/cbag ban {{0}}查看更多".SFormat(TShockAPI.Commands.Specifier),
            }) ;
        }

    }
}
