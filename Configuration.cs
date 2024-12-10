using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        [JsonProperty("物品查询", Order = -39)]
        public string Wiki_ID = "https://terraria.wiki.gg/zh/wiki/Item_IDs";
        [JsonProperty("配置说明", Order = -38)]
        public string README = "指令/cbag 指令使用权限:cbag 免疫检查权限:免检背包";
        #endregion

        #region 主体变量
        [JsonProperty("检查开关", Order = -31)]
        public bool Enable = true;
        [JsonProperty("清理玩家身上物品", Order = -30)]
        public bool ClearPlayersSlot = true;
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
        [JsonProperty("清理超数量物品", Order = 0)]
        public bool MaxStackToAir = false;
        [JsonProperty("限制数量", Order = 1)]
        public int MaxStackLimit = 500;
        #endregion

        #region 清掉落物与箱子物品（含超进度）
        [JsonProperty("清理掉落", Order = 3)]
        public bool ClearItemDrop = true;
        [JsonProperty("无法捡起", Order = 4)]
        public bool UnablePickUp = true;
        [JsonProperty("无法捡起半径", Order = 5)]
        public int ClearRange = 10;
        [JsonProperty("清理箱子", Order = 6)]
        public bool ClearChestItem = true;
        [JsonProperty("清掉落与箱子物品表", Order = 7)]
        public Dictionary<int, int> ClearTable { get; set; } = new Dictionary<int, int>();
        [JsonProperty("免清表", Order = 8)]
        public HashSet<int> ExemptItems { get; set; } = new HashSet<int>();
        #endregion

        #region 超进度物品表
        [JsonProperty("全时期", Order = 30)]
        public Dictionary<int, int> Anytime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("哥布林入侵前", Order = 31)]
        public Dictionary<int, int> Goblins { get; set; } = new Dictionary<int, int>();

        [JsonProperty("史莱姆王前", Order = 32)]
        public Dictionary<int, int> SlimeKing { get; set; } = new Dictionary<int, int>();

        [JsonProperty("克苏鲁之眼前", Order = 33)]
        public Dictionary<int, int> EyeofCthulhu { get; set; } = new Dictionary<int, int>();

        [JsonProperty("鹿角怪前", Order = 34)]
        public Dictionary<int, int> Deerclops { get; set; } = new Dictionary<int, int>();

        [JsonProperty("世界吞噬怪前", Order = 35)]
        public Dictionary<int, int> EaterofWorlds { get; set; } = new Dictionary<int, int>();

        [JsonProperty("克苏鲁之脑前", Order = 35)]
        public Dictionary<int, int> BrainofCthulhu { get; set; } = new Dictionary<int, int>();

        [JsonProperty("世吞克脑前", Order = 35)]
        public Dictionary<int, int> Boss2 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("蜂王前", Order = 36)]
        public Dictionary<int, int> QueenBee { get; set; } = new Dictionary<int, int>();

        [JsonProperty("骷髅王前", Order = 37)]
        public Dictionary<int, int> SkeletronHead { get; set; } = new Dictionary<int, int>();

        [JsonProperty("困难模式前", Order = 38)]
        public Dictionary<int, int> WallofFlesh { get; set; } = new Dictionary<int, int>();

        [JsonProperty("史莱姆皇后前", Order = 39)]
        public Dictionary<int, int> QueenSlime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("毁灭者前", Order = 40)]
        public Dictionary<int, int> TheDestroyer { get; set; } = new Dictionary<int, int>();

        [JsonProperty("机械骷髅王前", Order = 41)]
        public Dictionary<int, int> SkeletronPrime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("双子魔眼前", Order = 42)]
        public Dictionary<int, int> TheTwins { get; set; } = new Dictionary<int, int>();

        [JsonProperty("新一王前", Order = 43)]
        public Dictionary<int, int> MechBossAny { get; set; } = new Dictionary<int, int>();

        [JsonProperty("新三王前", Order = 44)]
        public Dictionary<int, int> MechBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("猪龙鱼公爵前", Order = 45)]
        public Dictionary<int, int> Fishron { get; set; } = new Dictionary<int, int>();

        [JsonProperty("世纪之花前", Order = 46)]
        public Dictionary<int, int> PlantBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("南瓜王前", Order = 47)]
        public Dictionary<int, int> Pumpking { get; set; } = new Dictionary<int, int>();

        [JsonProperty("哀木前", Order = 48)]
        public Dictionary<int, int> MourningWood { get; set; } = new Dictionary<int, int>();

        [JsonProperty("冰雪女王前", Order = 49)]
        public Dictionary<int, int> IceQueen { get; set; } = new Dictionary<int, int>();

        [JsonProperty("圣诞坦克前", Order = 50)]
        public Dictionary<int, int> SantaNK1 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("常绿尖叫怪前", Order = 51)]
        public Dictionary<int, int> Everscream { get; set; } = new Dictionary<int, int>();

        [JsonProperty("光之女皇前", Order = 52)]
        public Dictionary<int, int> EmpressOfLight { get; set; } = new Dictionary<int, int>();

        [JsonProperty("石巨人前", Order = 53)]
        public Dictionary<int, int> GolemBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("双足翼龙前", Order = 54)]
        public Dictionary<int, int> Betsy { get; set; } = new Dictionary<int, int>();

        [JsonProperty("火星飞碟前", Order = 55)]
        public Dictionary<int, int> MartianSaucer { get; set; } = new Dictionary<int, int>();

        [JsonProperty("拜月邪教徒前", Order = 56)]
        public Dictionary<int, int> Cultist { get; set; } = new Dictionary<int, int>();

        [JsonProperty("月亮领主前", Order = 57)]
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

        #region 辅助方法 - 根据条件处理物品字典
        private struct DictPair
        {
            public bool Condition;
            public Dictionary<int, int> Dict;
        }

        private void ProcessDictPairs(Action<Dictionary<int, int>> action)
        {
            var dict = new DictPair[]{
                new DictPair { Condition = !InBestiary(471), Dict = Goblins },//哥布林术士
                new DictPair { Condition = !InBestiary(4), Dict = EyeofCthulhu },//克苏鲁之眼
                new DictPair { Condition = !InBestiary(50), Dict = SlimeKing },//史莱姆王
                new DictPair { Condition = !InBestiary(668), Dict = Deerclops },//鹿角怪
                new DictPair { Condition = !InBestiary(13) && !InBestiary(14) && !InBestiary(15), Dict = EaterofWorlds },//世界吞噬怪
                new DictPair { Condition = !InBestiary(266), Dict = BrainofCthulhu },//克苏鲁之脑

                new DictPair { Condition = !InBestiary(266) ||
                (!InBestiary(13) && !InBestiary(14) && !InBestiary(15)), Dict = Boss2 },//世吞克脑前

                new DictPair { Condition = !InBestiary(222), Dict = QueenBee },//蜂王
                new DictPair { Condition = !InBestiary(35), Dict = SkeletronHead }, //骷髅王头
                new DictPair { Condition = !InBestiary(113) || !Main.hardMode, Dict = WallofFlesh }, //血肉墙或困难模式
                new DictPair { Condition = !InBestiary(657), Dict = QueenSlime }, //史莱姆皇后

                new DictPair { Condition = !InBestiary(134), Dict = TheDestroyer}, //毁灭者
                new DictPair { Condition = !InBestiary(127), Dict = SkeletronPrime}, //机械骷髅王
                new DictPair { Condition = !InBestiary(125) && !InBestiary(126), Dict = TheTwins}, //双子魔眼

                new DictPair { Condition = (!InBestiary(125) && !InBestiary(126)) ||
                !InBestiary(127) || !InBestiary(134), Dict = MechBossAny }, //任意机械BOSS

                new DictPair { Condition = (!InBestiary(125) && !InBestiary(126)) &&
                !InBestiary(127) && !InBestiary(134), Dict = MechBoss}, //所有机械三王

                new DictPair { Condition = !InBestiary(370), Dict = Fishron }, //猪龙鱼公爵

                new DictPair { Condition = !InBestiary(262), Dict = PlantBoss }, //世纪之花
                new DictPair { Condition = !InBestiary(327), Dict = Pumpking }, //南瓜王
                new DictPair { Condition = !InBestiary(325), Dict = MourningWood }, //哀木
                new DictPair { Condition = !InBestiary(345), Dict = IceQueen }, //冰雪女王
                new DictPair { Condition = !InBestiary(346), Dict = SantaNK1 }, //圣诞坦克
                new DictPair { Condition = !InBestiary(344), Dict = Everscream }, //常绿尖叫怪

                new DictPair { Condition = !InBestiary(636), Dict = EmpressOfLight }, //光之女皇

                new DictPair { Condition = !InBestiary(245) || !InBestiary(246), Dict = GolemBoss }, //石巨人身体或头

                new DictPair { Condition = !InBestiary(395), Dict = MartianSaucer }, //火星飞碟核心
                new DictPair { Condition = !InBestiary(551), Dict = Betsy }, //双足翼龙

                new DictPair { Condition = !InBestiary(439), Dict = Cultist },//拜月教
                new DictPair { Condition = !InBestiary(398), Dict = Moonlord } }; //月亮领主心脏

            foreach (var pair in dict)
            {
                if (!pair.Condition)
                {
                    continue;
                }

                action(pair.Dict);
            }
        }

        public bool IsEmpty()
        {
            return Current().Count == 0;
        }
        #endregion

        #region 超进度物品的字典处理
        public Dictionary<int, int> Current()
        {
            Dictionary<int, int> Item = new Dictionary<int, int>();

            ProcessDictPairs(dict =>
            {
                foreach (var Items in dict)
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
            });

            return Item;
        }
        #endregion

        #region 清掉落物、扔出与放入箱子物品哈希表处理
        public List<int> GetClearItemIds()
        {
            var ClearItems = new HashSet<int>(ClearTable.Keys);

            ProcessDictPairs(dict =>    
            {
                foreach (var item in dict.Keys)
                {
                    ClearItems.Add(item);
                }
            });

            return ClearItems.ToList();
        }
        #endregion

        #region 通过怪物图鉴判断怪物是否被击败 已解锁物品掉落的程度
        private bool InBestiary(int type)
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