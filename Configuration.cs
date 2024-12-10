using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Terraria;
using TShockAPI;

namespace CheckBag
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Configuration
    {
        #region 使用说明
        [JsonProperty("物品查询", Order = -40)]
        public string Wiki_ID = "https://terraria.wiki.gg/zh/wiki/Item_IDs";
        [JsonProperty("配置说明", Order = -39)]
        public string README = "指令/cbag 指令使用权限:cbag 免疫检查权限:免检背包";
        #endregion

        #region 主体变量
        [JsonProperty("插件开关", Order = -30)]
        public bool Enabled = true;
        [JsonProperty("清理背包", Order = -29)]
        public bool ClearPlayerSlot = true;
        [JsonProperty("检查次数", Order = -28)]
        public int CheckCount = 10;
        [JsonProperty("清理扔出", Order = -27)]
        public bool ClearItemDrop = true;
        [JsonProperty("清理箱子", Order = -26)]
        public bool ClearChestItem = true;
        [JsonProperty("强制清理", Order = -25)]
        public HashSet<int> ClearTable { get; set; } = new HashSet<int>();
        [JsonProperty("排除清理", Order = -24)]
        public HashSet<int> ExemptItems { get; set; } = new HashSet<int>();
        [JsonProperty("是否广播", Order = -23)]
        public bool Message = true;
        [JsonProperty("记录日志", Order = -22)]
        public bool Logs = true;
        [JsonProperty("是否封禁", Order = -21)]
        public bool Ban = false;
        [JsonProperty("封禁选项", Order = -20)]
        public List<Bandata> Banlist { get; set; } = new List<Bandata>();
        #endregion

        #region 超进度物品表
        [JsonProperty("哥布林入侵前", Order = 31)]
        public HashSet<int> Goblins { get; set; } = new HashSet<int>();
        [JsonProperty("史莱姆王前", Order = 32)]
        public HashSet<int> SlimeKing { get; set; } = new HashSet<int>();
        [JsonProperty("克苏鲁之眼前", Order = 33)]
        public HashSet<int> EyeofCthulhu { get; set; } = new HashSet<int>();
        [JsonProperty("鹿角怪前", Order = 34)]
        public HashSet<int> Deerclops { get; set; } = new HashSet<int>();
        [JsonProperty("世界吞噬怪前", Order = 35)]
        public HashSet<int> EaterofWorlds { get; set; } = new HashSet<int>();
        [JsonProperty("克苏鲁之脑前", Order = 35)]
        public HashSet<int> BrainofCthulhu { get; set; } = new HashSet<int>();
        [JsonProperty("世吞克脑前", Order = 35)]
        public HashSet<int> Boss2 { get; set; } = new HashSet<int>();
        [JsonProperty("蜂王前", Order = 36)]
        public HashSet<int> QueenBee { get; set; } = new HashSet<int>();
        [JsonProperty("骷髅王前", Order = 37)]
        public HashSet<int> SkeletronHead { get; set; } = new HashSet<int>();
        [JsonProperty("困难模式前", Order = 38)]
        public HashSet<int> WallofFlesh { get; set; } = new HashSet<int>();
        [JsonProperty("史莱姆皇后前", Order = 39)]
        public HashSet<int> QueenSlime { get; set; } = new HashSet<int>();
        [JsonProperty("毁灭者前", Order = 40)]
        public HashSet<int> TheDestroyer { get; set; } = new HashSet<int>();
        [JsonProperty("机械骷髅王前", Order = 41)]
        public HashSet<int> SkeletronPrime { get; set; } = new HashSet<int>();
        [JsonProperty("双子魔眼前", Order = 42)]
        public HashSet<int> TheTwins { get; set; } = new HashSet<int>();
        [JsonProperty("新一王前", Order = 43)]
        public HashSet<int> MechBossAny { get; set; } = new HashSet<int>();
        [JsonProperty("新三王前", Order = 44)]
        public HashSet<int> MechBoss { get; set; } = new HashSet<int>();
        [JsonProperty("猪龙鱼公爵前", Order = 45)]
        public HashSet<int> Fishron { get; set; } = new HashSet<int>();
        [JsonProperty("世纪之花前", Order = 46)]
        public HashSet<int> PlantBoss { get; set; } = new HashSet<int>();
        [JsonProperty("南瓜王前", Order = 47)]
        public HashSet<int> Pumpking { get; set; } = new HashSet<int>();
        [JsonProperty("哀木前", Order = 48)]
        public HashSet<int> MourningWood { get; set; } = new HashSet<int>();
        [JsonProperty("冰雪女王前", Order = 49)]
        public HashSet<int> IceQueen { get; set; } = new HashSet<int>();
        [JsonProperty("圣诞坦克前", Order = 50)]
        public HashSet<int> SantaNK1 { get; set; } = new HashSet<int>();
        [JsonProperty("常绿尖叫怪前", Order = 51)]
        public HashSet<int> Everscream { get; set; } = new HashSet<int>();
        [JsonProperty("光之女皇前", Order = 52)]
        public HashSet<int> EmpressOfLight { get; set; } = new HashSet<int>();
        [JsonProperty("石巨人前", Order = 53)]
        public HashSet<int> GolemBoss { get; set; } = new HashSet<int>();
        [JsonProperty("双足翼龙前", Order = 54)]
        public HashSet<int> Betsy { get; set; } = new HashSet<int>();
        [JsonProperty("火星飞碟前", Order = 55)]
        public HashSet<int> MartianSaucer { get; set; } = new HashSet<int>();
        [JsonProperty("拜月邪教徒前", Order = 56)]
        public HashSet<int> Cultist { get; set; } = new HashSet<int>();
        [JsonProperty("月亮领主前", Order = 57)]
        public HashSet<int> Moonlord { get; set; } = new HashSet<int>();
        #endregion

        #region 封禁数据表结构
        public class Bandata
        {
            [JsonProperty("封禁秒数", Order = 0)]
            public int BanTime { get; set; }

            [JsonProperty("封禁IP", Order = 1)]
            public bool BanIP { get; set; }

            [JsonProperty("封禁账号", Order = 2)]
            public bool BanAccount { get; set; }

            [JsonProperty("封禁设备", Order = 3)]
            public bool BanUUID { get; set; }

            public Bandata(int banTime, bool banIP, bool banAccount, bool banUUID)
            {
                BanTime = banTime;
                BanIP = banIP;
                BanAccount = banAccount;
                BanUUID = banUUID;
            }
        }
        #endregion

        #region 辅助方法 - 根据条件处理物品字典
        private struct DictPair
        {
            public bool Condition;
            public HashSet<int> Set;
        }

        private void ProcessDictPairs(Action<HashSet<int>> action)
        {
            var dict = new DictPair[]{
                new DictPair { Condition = !InBestiary(471), Set = Goblins },//哥布林术士
                new DictPair { Condition = !InBestiary(4), Set = EyeofCthulhu },//克苏鲁之眼
                new DictPair { Condition = !InBestiary(50), Set = SlimeKing },//史莱姆王
                new DictPair { Condition = !InBestiary(668), Set = Deerclops },//鹿角怪
                new DictPair { Condition = !InBestiary(13) && !InBestiary(14) && !InBestiary(15), Set = EaterofWorlds },//世界吞噬怪
                new DictPair { Condition = !InBestiary(266), Set = BrainofCthulhu },//克苏鲁之脑

                new DictPair { Condition = !InBestiary(266) ||
                (!InBestiary(13) && !InBestiary(14) && !InBestiary(15)), Set = Boss2 },//世吞克脑前

                new DictPair { Condition = !InBestiary(222), Set = QueenBee },//蜂王
                new DictPair { Condition = !InBestiary(35), Set = SkeletronHead }, //骷髅王头
                new DictPair { Condition = !InBestiary(113) || !Main.hardMode, Set = WallofFlesh }, //血肉墙或困难模式
                new DictPair { Condition = !InBestiary(657), Set = QueenSlime }, //史莱姆皇后

                new DictPair { Condition = !InBestiary(134), Set = TheDestroyer}, //毁灭者
                new DictPair { Condition = !InBestiary(127), Set = SkeletronPrime}, //机械骷髅王
                new DictPair { Condition = !InBestiary(125) && !InBestiary(126), Set = TheTwins}, //双子魔眼

                new DictPair { Condition = (!InBestiary(125) && !InBestiary(126)) ||
                !InBestiary(127) || !InBestiary(134), Set = MechBossAny }, //任意机械BOSS

                new DictPair { Condition = (!InBestiary(125) && !InBestiary(126)) &&
                !InBestiary(127) && !InBestiary(134), Set = MechBoss}, //所有机械三王

                new DictPair { Condition = !InBestiary(370), Set = Fishron }, //猪龙鱼公爵

                new DictPair { Condition = !InBestiary(262), Set = PlantBoss }, //世纪之花
                new DictPair { Condition = !InBestiary(327), Set = Pumpking }, //南瓜王
                new DictPair { Condition = !InBestiary(325), Set = MourningWood }, //哀木
                new DictPair { Condition = !InBestiary(345), Set = IceQueen }, //冰雪女王
                new DictPair { Condition = !InBestiary(346), Set = SantaNK1 }, //圣诞坦克
                new DictPair { Condition = !InBestiary(344), Set = Everscream }, //常绿尖叫怪

                new DictPair { Condition = !InBestiary(636), Set = EmpressOfLight }, //光之女皇

                new DictPair { Condition = !InBestiary(245) || !InBestiary(246), Set = GolemBoss }, //石巨人身体或头

                new DictPair { Condition = !InBestiary(395), Set = MartianSaucer }, //火星飞碟核心
                new DictPair { Condition = !InBestiary(551), Set = Betsy }, //双足翼龙

                new DictPair { Condition = !InBestiary(439), Set = Cultist },//拜月教
                new DictPair { Condition = !InBestiary(398), Set = Moonlord } }; //月亮领主心脏

            foreach (var pair in dict)
            {
                if (!pair.Condition)
                {
                    continue;
                }

                action(pair.Set);
            }
        }
        #endregion

        #region 清掉落物、扔出与放入箱子物品哈希表处理
        public HashSet<int> GetClearItemIds()
        {
            ProcessDictPairs(set =>
            {
                foreach (var item in set)
                {
                    ClearTable.Add(item);
                }
            });

            return ClearTable;
        }
        #endregion

        #region 通过怪物图鉴判断怪物是否被击败 已解锁物品掉落的程度
        internal bool InBestiary(int type)
        {
            var unlockState = Main.BestiaryDB.FindEntryByNPCID(type).UIInfoProvider.GetEntryUICollectionInfo().UnlockState;
            return unlockState == Terraria.GameContent.Bestiary.BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
        }
        #endregion

        #region 读取与创建配置文件方法
        public static readonly string FilePath = Path.Combine(TShock.SavePath, "检查背包", "检查背包.json");
        public void Write()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static Configuration Read()
        {
            if (!File.Exists(FilePath))
            {
                WriteExample();
            }

            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(FilePath))!;
        }

        //内嵌文件方法
        public static void WriteExample()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var FullName = $"{assembly.GetName().Name}.内嵌文件.检查背包.json";

            using (var stream = assembly.GetManifestResourceStream(FullName))
            using (var reader = new StreamReader(stream!))
            {
                var text = reader.ReadToEnd();
                var config = JsonConvert.DeserializeObject<Configuration>(text);
                config!.Write();
            }
        }
        #endregion
    }
}