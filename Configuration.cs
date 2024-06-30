using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Terraria;
using TShockAPI;

namespace CheckBag
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Configuration
    {
        public static readonly string FilePath = Path.Combine(TShock.SavePath, "检查背包", "检查背包.json");

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
        [JsonProperty("检查开关", Order = -29)]
        public bool Enable = true;
        [JsonProperty("清掉落物(超数量/超进度无法捡起)", Order = -28)]
        public bool PlayerUp = true;
        [JsonProperty("限制数量", Order = -28)]
        public int ItemCount = 500;
        [JsonProperty("设置超数量物品数", Order = -28)]
        public bool ItemCountKG = false;
        [JsonProperty("清理超数量物品", Order = -28)]
        public bool TurnToAir = true;
        [JsonProperty("清掉落范围/半径", Order = -27)]
        public int ClearrRange = 10;
        [JsonProperty("检测间隔/秒", Order = -26)]
        public int DetectionInterval = 5;
        [JsonProperty("清理频率(每秒帧率)", Order = -25)]
        public int UpdateRate = 60;

        [JsonProperty("检查超进度次数", Order = -24)]
        public int WarningCount = 150000;
        [JsonProperty("是否封禁(↑调次数)", Order = -23)]
        public bool Ban = false;
        [JsonProperty("封禁时长/分钟(过时解封)", Order = -22)]
        public int BanTime = 10;

        [JsonProperty("超进度是否广播", Order = -21)]
        public bool Message = true;
        [JsonProperty("超进度记录日志", Order = -20)]
        public bool Logs = true;


        [JsonProperty("清掉落表", Order = -11)]
        public Dictionary<int, int> ClearTable { get; set; } = new Dictionary<int, int>();

        [JsonProperty("掉落免清", Order = -10)]
        public HashSet<int> ExemptItems { get; set; } = new HashSet<int>();

        #region 进度物品表
        [JsonProperty("全时期", Order = -9)]
        public Dictionary<int, int> Anytime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("哥布林入侵", Order = -8)]
        public Dictionary<int, int> Goblins { get; set; } = new Dictionary<int, int>();

        [JsonProperty("史王前", Order = -7)]
        public Dictionary<int, int> SlimeKing { get; set; } = new Dictionary<int, int>();

        [JsonProperty("克眼前", Order = -6)]
        public Dictionary<int, int> Boss1 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("鹿角怪前", Order = -5)]
        public Dictionary<int, int> Deerclops { get; set; } = new Dictionary<int, int>();

        [JsonProperty("世吞克脑前", Order = -4)]
        public Dictionary<int, int> Boss2 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("蜂王前", Order = -3)]
        public Dictionary<int, int> QueenBee { get; set; } = new Dictionary<int, int>();

        [JsonProperty("骷髅王前", Order = -2)]
        public Dictionary<int, int> Boss3 { get; set; } = new Dictionary<int, int>();

        [JsonProperty("肉前", Order = -1)]
        public Dictionary<int, int> hardMode { get; set; } = new Dictionary<int, int>();

        [JsonProperty("皇后前", Order = 0)]
        public Dictionary<int, int> QueenSlime { get; set; } = new Dictionary<int, int>();

        [JsonProperty("一王前", Order = 1)]
        public Dictionary<int, int> MechBossAny { get; set; } = new Dictionary<int, int>();

        [JsonProperty("三王前", Order = 2)]
        public Dictionary<int, int> MechBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("猪鲨前", Order = 3)]
        public Dictionary<int, int> Fishron { get; set; } = new Dictionary<int, int>();

        [JsonProperty("光女前", Order = 4)]
        public Dictionary<int, int> EmpressOfLight { get; set; } = new Dictionary<int, int>();

        [JsonProperty("花前", Order = 5)]
        public Dictionary<int, int> PlantBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("石前", Order = 6)]
        public Dictionary<int, int> GolemBoss { get; set; } = new Dictionary<int, int>();

        [JsonProperty("拜月前", Order = 7)]
        public Dictionary<int, int> AncientCultist { get; set; } = new Dictionary<int, int>();

        [JsonProperty("月前", Order = 8)]
        public Dictionary<int, int> Moonlord { get; set; } = new Dictionary<int, int>();
        #endregion

        #region 预设配置
        public void Init()
        {
            ClearTable = new Dictionary<int, int> { { 4956 , 1} };

            ExemptItems = new HashSet<int> { 8 };

            Anytime = new Dictionary<int, int>
            {
                {3617, 1 }
            };

            Goblins = new Dictionary<int, int> {
                {128, 1},
                {405, 1},
                {3993, 1},
                {908, 1},
                {898, 1},
                {1862, 1},
                {5000, 1},
                {1163, 1},
                {983, 1},
                {399, 1},
                {1863, 1},
                {1252, 1},
                {1251, 1},
                {1250, 1},
                {3250, 1},
                {3251, 1},
                {3241, 1},
                {3252, 1},
                {1164, 1},
                {3990, 1},
                {1860, 1},
                {1861, 1},
                {3995, 1},
                {407, 1},
                {395, 1},
                {3122, 1},
                {3121, 1},
                {3036, 1},
                {3123, 1},
                {555, 1},
                {4000, 1},
                {2221, 1},
                {1595, 1},
                {3061, 1},
                {5126, 1},
                {5358, 1},
                {5359, 1},
                {5360, 1},
                {5361, 1},
                {5331, 1}
            };

            SlimeKing = new Dictionary<int, int>
            {
                {3318, 1},
                {2430, 1},
                {256, 1},
                {257, 1},
                {258, 1},
                {3090, 1},
            };

            Boss1 = new Dictionary<int, int>
            {
                {3319, 1},
                {3262, 1},
                {3097, 1},
            };

            Deerclops = new Dictionary<int, int>
            {
                {5111, 1},
                {5098, 1},
                {5095, 1},
                {5117, 1},
                {5118, 1},
                {5119, 1},
            };

            Boss2 = new Dictionary<int, int>
            {
                {3320, 1},
                {3321, 1},
                {174, 1},
                {175, 1},
                {122, 1},
                {120, 1},
                {119, 1},
                {231, 1},
                {232, 1},
                {233, 1},
                {2365, 1},
                {4821, 1},
                {121, 1},
                {3223, 1},
                {3224, 1},
                {3266, 1},
                {3267, 1},
                {3268, 1},
                {102, 1},
                {101, 1},
                {100, 1},
                {103, 1},
                {792, 1},
                {793, 1},
                {794, 1},
                {798, 1},
                {3817, 1},
                {3813, 1},
                {3809, 1},
                {3810, 1},
                {197, 1},
                {123, 1},
                {124, 1},
                {125, 1},
                {127, 1},
                {116, 1},
                {117, 1},
            };

            QueenBee = new Dictionary<int, int> {
                {3322, 1},
                {1123, 1},
                {2888, 1},
                {1121, 1},
                {1132, 1},
                {1130, 1},
                {2431, 1},
                {2502, 1},
                {1249, 1},
                {4007, 1},
                {5294, 1},
                {1158, 1},
                {1430, 1},
            };

            Boss3 = new Dictionary<int, int> {
                {3323, 1},
                {273, 1},
                {329, 1},
                {113, 1},
                {683, 1},
                {157, 1},
                {3019, 1},
                {219, 1},
                {218, 1},
                {220, 1},
                {3317, 1},
                {3282, 1},
                {155, 1},
                {156, 1},
                {397, 1},
                {163, 1},
                {164, 1},
                {151, 1},
                {152, 1},
                {153, 1},
                {5074, 1},
                {1313, 1},
                {2999, 1},
                {3000, 1},
                {890, 1},
                {891, 1},
                {904, 1},
                {2623, 1},
                {327, 1},
            };

            hardMode = new Dictionary<int, int> {
                {3324, 1},
                {2673, 1},
                {3991, 1},
                {3366, 1},
                {400, 1},
                {401, 1},
                {402, 1},
                {403, 1},
                {404, 1},
                {391, 1},
                {778, 1},
                {481, 1},
                {524, 1},
                {376, 1},
                {377, 1},
                {378, 1},
                {379, 1},
                {380, 1},
                {382, 1},
                {777, 1},
                {436, 1},
                {525, 1},
                {371, 1},
                {372, 1},
                {373, 1},
                {374, 1},
                {375, 1},
                {381, 1},
                {776, 1},
                {435, 1},
                {1205, 1},
                {1206, 1},
                {1207, 1},
                {1208, 1},
                {1209, 1},
                {1184, 1},
                {1187, 1},
                {1188, 1},
                {1189, 1},
                {1210, 1},
                {1211, 1},
                {1212, 1},
                {1213, 1},
                {1214, 1},
                {1191, 1},
                {1194, 1},
                {1195, 1},
                {1196, 1},
                {1220, 1},
                {1215, 1},
                {1216, 1},
                {1217, 1},
                {1218, 1},
                {1219, 1},
                {1198, 1},
                {1201, 1},
                {1202, 1},
                {1203, 1},
                {1221, 1},
                {1328, 1},
                {2161, 1},
                {684, 1},
                {685, 1},
                {686, 1},
                {726, 1},
                {1264, 1},
                {676, 1},
                {4911, 1},
                {1306, 1},
                {3783, 1},
                {3776, 1},
                {3777, 1},
                {3778, 1},
                {2607, 1},
                {2370, 1},
                {2371, 1},
                {2372, 1},
                {2551, 1},
                {2366, 1},
                {1308, 1},
                {389, 1},
                {426, 1},
                {3051, 1},
                {422, 1},
                {2998, 1},
                {489, 1},
                {490, 1},
                {491, 1},
                {492, 1},
                {493, 1},
                {785, 1},
                {1165, 1},
                {761, 1},
                {822, 1},
                {485, 1},
                {900, 1},
                {497, 1},
                {861, 1},
                {3013, 1},
                {3014, 1},
                {3015, 1},
                {3016, 1},
                {3992, 1},
                {536, 1},
                {897, 1},
                {527, 1},
                {528, 1},
                {520, 1},
                {521, 1},
                {575, 1},
                {535, 1},
                {860, 1},
                {554, 1},
                {862, 1},
                {1613, 1},
                {1612, 1},
                {892, 1},
                {886, 1},
                {901, 1},
                {893, 1},
                {889, 1},
                {903, 1},
                {888, 1},
                {3781, 1},
                {5354, 1},
                {1253, 1},
                {3290, 1},
                {3289, 1},
                {3316, 1},
                {3315, 1},
                {3283, 1},
                {3054, 1},
                {532, 1},
                {1247, 1},
                {1244, 1},
                {1326, 1},
                {522, 1},
                {519, 1},
                {3010, 1},
                {545, 1},
                {546, 1},
                {1332, 1},
                {1334, 1},
                {1335, 1},
                {3011, 1},
                {1356, 1},
                {1336, 1},
                {1346, 1},
                {1350, 1},
                {1357, 1},
                {1347, 1},
                {1351, 1},
                {526, 1},
                {501, 1},
                {516, 1},
                {502, 1},
                {518, 1},
                {515, 1},
                {3009, 1},
                {534, 1},
                {3211, 1},
                {723, 1},
                {514, 1},
                {1265, 1},
                {3788, 1},
                {3210, 1},
                {2270, 1},
                {434, 1},
                {496, 1},
                {3006, 1},
                {3007, 1},
                {3008, 1},
                {3029, 1},
                {3052, 1},
                {5065, 1},
                {4269, 1},
                {4270, 1},
                {4272, 1},
                {3787, 1},
                {1321, 1},
                {4006, 1},
                {4002, 1},
                {3103, 1},
                {3104, 1},
                {2750, 1},
                {905, 1},
                {2584, 1},
                {854, 1},
                {855, 1},
                {3034, 1},
                {3035, 1},
                {1324, 1},
                {3012, 1},
                {4912, 1},
                {544, 1},
                {556, 1},
                {557, 1},
                {3779, 1}
            };

            QueenSlime = new Dictionary<int, int> {
                {4957, 1},
                {4987, 1},
                {4980, 1},
                {4981, 1},
                {4982, 1},
                {4983, 1},
                {4984, 1},
                {4758, 1},
            };

            MechBossAny = new Dictionary<int, int> {
                {3325, 1},
                {3326, 1},
                {3327, 1},
                {1291, 1},
                {5338, 1},
                {533, 1},
                {4060, 1},
                {561, 1},
                {494, 1},
                {495, 1},
                {4760, 1},
                {506, 1},
                {3284, 1},
                {3287, 1},
                {3288, 1},
                {3286, 1},
                {1515, 1},
                {821, 1},
                {748, 1},
                {4896, 1},
                {4897, 1},
                {4898, 1},
                {4899, 1},
                {4900, 1},
                {4901, 1},
                {558, 1},
                {559, 1},
                {553, 1},
                {4873, 1},
                {551, 1},
                {552, 1},
                {1225, 1},
                {578, 1},
                {4678, 1},
                {550, 1},
                {2535, 1},
                {3353, 1},
                {547, 1},
                {548, 1},
                {549, 1},
                {3856, 1},
                {3835, 1},
                {3836, 1},
                {3854, 1},
                {3823, 1},
                {3852, 1},
                {3797, 1},
                {3798, 1},
                {3799, 1},
                {3800, 1},
                {3801, 1},
                {3802, 1},
                {3803, 1},
                {3804, 1},
                {3805, 1},
                {3811, 1},
                {3806, 1},
                {3807, 1},
                {3808, 1},
                {3812, 1},
            };

            MechBoss = new Dictionary<int, int> {
                {935, 1},
                {2220, 1},
                {936, 1},
                {1343, 1},
                {674, 1},
                {675, 1},
                {990, 1},
                {579, 1},
                {947, 1},
                {1006, 1},
                {1001, 1},
                {1002, 1},
                {1003, 1},
                {1004, 1},
                {1005, 1},
                {1229, 1},
                {1230, 1},
                {1231, 1},
                {1235, 1},
                {1179, 1},
                {1316, 1},
                {1317, 1},
                {1318, 1},
                {5382, 1},
            };

            PlantBoss = new Dictionary<int, int> {
                {3328, 1},
                {1958, 1},
                {1844, 1},
                {4961, 1},
                {3336, 1},
                {963, 1},
                {984, 1},
                {977, 1},
                {3292, 1},
                {3291, 1},
                {1178, 1},
                {3105, 1},
                {3106, 1},
                {2770, 1},
                {1871, 1},
                {1183, 1},
                {4679, 1},
                {4680, 1},
                {1444, 1},
                {1445, 1},
                {1446, 1},
                {3249, 1},
                {1305, 1},
                {757, 1},
                {1569, 1},
                {1571, 1},
                {1572, 1},
                {1156, 1},
                {1260, 1},
                {4607, 1},
                {1508, 1},
                {1946, 1},
                {1947, 1},
                {1931, 1},
                {1928, 1},
                {1929, 1},
                {1930, 1},
                {3997, 1},
                {1552, 1},
                {1546, 1},
                {1547, 1},
                {1548, 1},
                {1549, 1},
                {1550, 1},
                {1866, 1},
                {1570, 1},
                {823, 1},
                {1503, 1},
                {1504, 1},
                {1505, 1},
                {1506, 1},
                {3261, 1},
                {1729, 1},
                {1830, 1},
                {1832, 1},
                {1833, 1},
                {1834, 1},
                {1802, 1},
                {1801, 1},
                {4444, 1},
                {1157, 1},
                {1159, 1},
                {1160, 1},
                {1161, 1},
                {1162, 1},
                {1845, 1},
                {1864, 1},
                {1339, 1},
                {1342, 1},
                {1341, 1},
                {1340, 1},
                {1255, 1},
                {3107, 1},
                {3108, 1},
                {1782, 1},
                {1783, 1},
                {1910, 1},
                {1300, 1},
                {1254, 1},
                {760, 1},
                {759, 1},
                {758, 1},
                {1784, 1},
                {1785, 1},
                {1835, 1},
                {1836, 1},
                {4457, 1},
                {4458, 1},
                {771, 1},
                {772, 1},
                {773, 1},
                {774, 1},
                {4445, 1},
                {4446, 1},
                {4447, 1},
                {4448, 1},
                {4449, 1},
                {4459, 1},
                {1259, 1},
                {3018, 1},
                {1826, 1},
                {1513, 1},
                {938, 1},
                {3998, 1},
                {1327, 1},
                {4812, 1},
                {1301, 1},
                {4005, 1},
            };

            Fishron = new Dictionary<int, int> {
                {3330, 1},
                {2609, 1},
                {2622, 1},
                {2624, 1},
                {2621, 1},
                {2611, 1},
                {3367, 1}
            };

            EmpressOfLight = new Dictionary<int, int> {
                {4782, 1},
                {4823, 1},
                {4715, 1},
                {4914, 1},
                {5005, 1},
                {4989, 1},
                {4923, 1},
                {4952, 1},
                {4953, 1},
                {4811, 1},
            };

            GolemBoss = new Dictionary<int, int> {
                {3329, 1},
                {3860, 1},
                {4807, 1},
                {1248, 1},
                {3337, 1},
                {1294, 1},
                {1122, 1},
                {1295, 1},
                {1296, 1},
                {1297, 1},
                {3110, 1},
                {1865, 1},
                {899, 1},
                {2218, 1},
                {2199, 1},
                {2200, 1},
                {2201, 1},
                {2202, 1},
                {2280, 1},
                {948, 1},
                {3871, 1},
                {3872, 1},
                {3873, 1},
                {3874, 1},
                {3875, 1},
                {3876, 1},
                {3877, 1},
                {3878, 1},
                {3879, 1},
                {3880, 1},
                {3882, 1},
                {1258, 1},
                {2797, 1},
                {2882, 1},
                {2769, 1},
                {2880, 1},
                {2795, 1},
                {2749, 1},
                {2796, 1},
                {3883, 1},
                {3870, 1},
                {3859, 1},
                {3858, 1},
                {3827, 1},
                {1261, 1},
            };


            AncientCultist = new Dictionary<int, int> {
                {3456, 1},
                {3457, 1},
                {3458, 1},
                {3459, 1},
                {3473, 1},
                {3543, 1},
                {3474, 1},
                {3531, 1},
                {3475, 1},
                {3540, 1},
                {3542, 1},
                {3476, 1},
                {3549, 1},
                {3544, 1},
            };

            Moonlord = new Dictionary<int, int> {
                {3332, 1},
                {3601, 1},
                {3460, 1},
                {3467, 1},
                {3567, 1},
                {3568, 1},
                {2757, 1},
                {2758, 1},
                {2759, 1},
                {3469, 1},
                {3381, 1},
                {3382, 1},
                {3383, 1},
                {3470, 1},
                {2760, 1},
                {2761, 1},
                {2762, 1},
                {3471, 1},
                {2763, 1},
                {2764, 1},
                {2765, 1},
                {3468, 1},
                {3466, 1},
                {2776, 1},
                {2781, 1},
                {2786, 1},
                {2774, 1},
                {2779, 1},
                {2784, 1},
                {3464, 1},
                {2768, 1},
                {3384, 1},
                {1131, 1},
                {4954, 1},
                {4956, 1},
                {3065, 1},
                {3063, 1},
                {1553, 1},
                {3930, 1},
                {3570, 1},
                {3389, 1},
                {3541, 1},
                {3569, 1},
                {3571, 1},
                {5335, 1},
                {5364, 1},
            };
        }
        #endregion

        #region 超进度物品
        private struct ConditionDictionaryPair
        {
            public bool Condition;
            public Dictionary<int, int> Dictionary;
        }

        public Dictionary<int, int> Current()
        {
            Dictionary<int, int> Item = new Dictionary<int, int>();
            var dictionaries = new ConditionDictionaryPair[]{
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

            foreach (var pair in dictionaries)
            {
                if (pair.Condition)
                {
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
            }
            return Item;
        }

        public bool IsEmpty()
        {
            return Current().Count == 0;
        }
        #endregion

        #region 加载配置文件方法
        public void Write()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static Configuration Read()
        {
            if (!File.Exists(FilePath))
            {
                var NewConfig = new Configuration();
                NewConfig.Init();
                new Configuration().Write();
                return NewConfig;
            }
            else
            {
                string jsonContent = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Configuration>(jsonContent)!;
            }
        }

        #endregion

    }
}