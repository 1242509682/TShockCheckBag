using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using TShockAPI;

namespace CheckBag
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Configuration
    {
        #region 使用说明
        [JsonProperty("物品查询", Order = -39)]
        public string Wiki_ID = "https://terraria.wiki.gg/zh/wiki/Item_IDs";
        [JsonProperty("配置说明", Order = -38)]
        public string README = "指令/cbag（权限: cbag 和 免检背包）";
        [JsonProperty("配置说明2", Order = -37)]
        public string README2 = "[检查次数] 影响清理《超进度物品》与 [是否封禁]达标次数";
        [JsonProperty("配置说明3", Order = -36)]
        public string README3 = "[清掉落物]除了《全时期》其他任何表都会受影响，再也不用捡垃圾了（泪目）";
        [JsonProperty("配置说明4", Order = -35)]
        public string README4 = "[限制数量] 可限制玩家丢出的此数值任何物品无法捡起，可强制修改超过此数值的所有玩家物品; ";
        [JsonProperty("配置说明5", Order = -34)]
        public string README5 = "《掉落免清》可无视捡起，阻止不了[清超数量]与[超进度]";
        #endregion

        #region 主体变量
        [JsonProperty("检查开关", Order = -30)]
        public bool Enable = true;
        [JsonProperty("检测间隔秒数", Order = -29)]
        public int Interval = 1;
        [JsonProperty("更新频率", Order = -28)]
        public int UpdateRate = 30;
        [JsonProperty("检查次数", Order = -27)]
        public int CheckCount = 150000;
        [JsonProperty("是否封禁(↑调次数)", Order = -26)]
        public bool Ban = false;
        [JsonProperty("惩罚封禁表", Order = -25)]
        public List<Bandata> Banlist { get; set; } = new List<Bandata>();
        [JsonProperty("超进度是否广播", Order = -24)]
        public bool Message = true;
        [JsonProperty("超进度记录日志", Order = -22)]
        public bool Logs = true;
        #endregion

        #region 超数量
        [JsonProperty("限制数量", Order = 0)]
        public int MaxStackLimit = 500;
        [JsonProperty("设置成限制数量", Order = 1)]
        public bool SetMaxStack = false;
        [JsonProperty("清理超数量物品", Order = 2)]
        public bool MaxStackToAir = false; 
        #endregion

        #region 清掉落
        [JsonProperty("清掉落物(无法捡起)", Order = 3)]
        public bool ClearItemDrop = true;
        [JsonProperty("清掉落半径", Order = 4)]
        public int ClearRange = 10;
        [JsonProperty("清掉落表", Order = 5)]
        public Dictionary<int, int> ClearTable { get; set; } = new Dictionary<int, int>();
        [JsonProperty("掉落免清", Order = 6)]
        public HashSet<int> ExemptItems { get; set; } = new HashSet<int>(); 
        #endregion

        #region 进度物品表
        [JsonProperty("全时期", Order = 30)]
        public Dictionary<int, int> Anytime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("哥布林入侵", Order = 31)]
        public Dictionary<int, int> Goblins { get; set; } = new Dictionary<int, int>();

        [JsonProperty("史王前", Order = 32)]
        public Dictionary<int, int> SlimeKing { get; set; } = new Dictionary<int, int>();

        [JsonProperty("克眼前", Order = 33)]
        public Dictionary<int, int> Boss1 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("鹿角怪前", Order = 34)]
        public Dictionary<int, int> Deerclops { get; set; } = new Dictionary<int, int>();

        [JsonProperty("世吞克脑前", Order = 35)]
        public Dictionary<int, int> Boss2 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("蜂王前", Order = 36)]
        public Dictionary<int, int> QueenBee { get; set; } = new Dictionary<int, int>();

        [JsonProperty("骷髅王前", Order = 37)]
        public Dictionary<int, int> Boss3 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("肉前", Order = 38)]
        public Dictionary<int, int> hardMode { get; set; } = new Dictionary<int, int>();

        [JsonProperty("皇后前", Order = 39)]
        public Dictionary<int, int> QueenSlime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("一王前", Order = 40)]
        public Dictionary<int, int> MechBossAny { get; set; } = new Dictionary<int, int>();

        [JsonProperty("三王前", Order = 41)]
        public Dictionary<int, int> MechBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("猪鲨前", Order = 42)]
        public Dictionary<int, int> Fishron { get; set; } = new Dictionary<int, int>();

        [JsonProperty("光女前", Order = 43)]
        public Dictionary<int, int> EmpressOfLight { get; set; } = new Dictionary<int, int>();

        [JsonProperty("花前", Order = 44)]
        public Dictionary<int, int> PlantBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("石前", Order = 45)]
        public Dictionary<int, int> GolemBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("拜月前", Order = 46)]
        public Dictionary<int, int> AncientCultist { get; set; } = new Dictionary<int, int>();

        [JsonProperty("月前", Order = 47)]
        public Dictionary<int, int> Moonlord { get; set; } = new Dictionary<int, int>();
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

        #region 超进度物品表
        private struct ConditionDictionaryPair
        {
            public bool Condition;
            public Dictionary<int, int> Dictionary;
        }

        public Dictionary<int, int> Current()
        {
            Dictionary<int, int> Item = new Dictionary<int, int>();
            var dict = new ConditionDictionaryPair[]{
                new ConditionDictionaryPair { Condition = !NPC.downedSlimeKing, Dictionary = SlimeKing },
                new ConditionDictionaryPair { Condition = !NPC.downedGoblins, Dictionary = Goblins },
                new ConditionDictionaryPair { Condition = !NPC.downedBoss1, Dictionary =Boss1 },
                new ConditionDictionaryPair { Condition = ! NPC.downedDeerclops, Dictionary = Deerclops },
                new ConditionDictionaryPair { Condition = ! NPC.downedBoss2, Dictionary = Boss2 },
                new ConditionDictionaryPair { Condition = ! NPC.downedQueenBee, Dictionary = QueenBee },
                new ConditionDictionaryPair { Condition = ! NPC.downedBoss3, Dictionary = Boss3 },
                new ConditionDictionaryPair { Condition = !Main.hardMode, Dictionary =hardMode},
                new ConditionDictionaryPair { Condition = ! NPC.downedQueenSlime, Dictionary = QueenSlime },
                new ConditionDictionaryPair { Condition = ! NPC.downedMechBossAny, Dictionary = MechBossAny },
                new ConditionDictionaryPair { Condition = !NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3, Dictionary =MechBoss},
                new ConditionDictionaryPair { Condition = ! NPC.downedFishron, Dictionary = Fishron },
                new ConditionDictionaryPair { Condition = ! NPC.downedEmpressOfLight, Dictionary = EmpressOfLight },
                new ConditionDictionaryPair { Condition = ! NPC.downedPlantBoss, Dictionary = PlantBoss },
                new ConditionDictionaryPair { Condition = ! NPC.downedGolemBoss, Dictionary = GolemBoss },
                new ConditionDictionaryPair { Condition = ! NPC.downedAncientCultist, Dictionary = AncientCultist },
                new ConditionDictionaryPair { Condition = ! NPC.downedMoonlord, Dictionary = Moonlord } };

            foreach (var pair in dict)
            {
                if (!pair.Condition)
                {
                    continue;
                }

                foreach (var Items in pair.Dictionary)
                {
                    if (!Item.ContainsKey(Items.Key))
                    {
                        Item.Add(Items.Key, Items.Value);
                    }
                    else
                    {
                        Item[Items.Key] += Items.Value;
                    }
                }
            }
            return Item;
        }

        public bool IsEmpty()
        {
            return Current().Count == 0;
        }
        #endregion

        #region 清掉落表
        public List<int> GetClearItemIds()
        {
            var ClearItems = new HashSet<int>(ClearTable.Keys);
            var dict = new ConditionDictionaryPair[]{
                new ConditionDictionaryPair { Condition = ! NPC.downedSlimeKing, Dictionary = SlimeKing },
                new ConditionDictionaryPair { Condition = ! NPC.downedGoblins, Dictionary = Goblins },
                new ConditionDictionaryPair { Condition = ! NPC.downedBoss1, Dictionary = Boss1 },
                new ConditionDictionaryPair { Condition = ! NPC.downedDeerclops, Dictionary = Deerclops },
                new ConditionDictionaryPair { Condition = ! NPC.downedBoss2, Dictionary = Boss2 },
                new ConditionDictionaryPair { Condition = ! NPC.downedQueenBee, Dictionary = QueenBee },
                new ConditionDictionaryPair { Condition = ! NPC.downedBoss3, Dictionary = Boss3 },
                new ConditionDictionaryPair { Condition = ! Main.hardMode, Dictionary = hardMode},
                new ConditionDictionaryPair { Condition = ! NPC.downedQueenSlime, Dictionary = QueenSlime },
                new ConditionDictionaryPair { Condition = ! NPC.downedMechBossAny, Dictionary = MechBossAny },
                new ConditionDictionaryPair { Condition = ! NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3, Dictionary = MechBoss},
                new ConditionDictionaryPair { Condition = ! NPC.downedFishron, Dictionary = Fishron },
                new ConditionDictionaryPair { Condition = ! NPC.downedEmpressOfLight, Dictionary = EmpressOfLight },
                new ConditionDictionaryPair { Condition = ! NPC.downedPlantBoss, Dictionary = PlantBoss },
                new ConditionDictionaryPair { Condition = ! NPC.downedGolemBoss, Dictionary = GolemBoss },
                new ConditionDictionaryPair { Condition = ! NPC.downedAncientCultist, Dictionary = AncientCultist },
                new ConditionDictionaryPair { Condition = ! NPC.downedMoonlord, Dictionary = Moonlord } };

            foreach (var pair in dict)
            {
                if (!pair.Condition)
                {
                    continue;
                }

                foreach (var item in pair.Dictionary.Keys)
                {
                    ClearItems.Add(item);
                }
            }

            return ClearItems.ToList();
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