## 检查背包[超进度物品检测]（原名CheckBag）

## 介绍
作者: [hufang360](https://github.com/hufang360/TShockCheckBag)
修改：羽学  
更新日期:2024年6月30日 16:36  
插件版本:2.2.0  
本插件可通过修改【检查背包.json】配置文件来控制：  
检测频率、检测次数、封禁时间（封禁只封UUID，时间一过自动解封)  
并加入了常见的进度项,默认生成的配置文件完善了大部分进度物品身份证.  

## 关于默认生成的配置文件说明
```
由于陨石锭可以在地狱金箱子里能找到、Tshock的醉酒世界特性有骷髅王前会刷怪的BUG  
使得玩家老板前就可以获得：  
金钥匙、邪眼、村正、钴护盾、骨头、死灵套、骨鞭、陨石套等超进度物品  
并考虑到微光湖的存在将:【扩音器、邪眼、反诅咒咒语】设为【骷髅王进度】  
同时未把【新六矿近战系武器、锤斧类工具】加入到默认的配置文件中  
使用者可根据自己服务器情况自由修改配置文件调整合理的进度物品身份证明  
修改完配置文件后可使用【/reload】重载配置文件实时同步服务器  
```

## 指令
```
/cbag b，列出封禁记录  
/cbag i，列出违规物品  
/reload
```

## 权限
```
cbag  
免检背包
```

## 授权示意：
```
以上指令权限名为 `cbag` 可分配给玩家使用：  
/group addperm default cbag

免检权限名【免检背包】，如需免检指定玩家可使用以下指令
1.新增一个item组，拥有【免检背包】权限
/group add item 免检背包

2.item组继承default所拥有的权限：
/group parent item default

3.将指定玩家分配到item组：
/user group 玩家名字 item
```
## 更新日志
```
2.2.0更新日志
重构了部分方法，将ItemData换成了Dictionary字典键值
加入了【清掉落物】设定，超过指定数量内的所有物品/非进度内的物品无法捡起
加入了对广播、日志、封禁的配置开关,嫌清背包速度不够快的可以减少【检测间隔】
不建议动清理频率（对人多的服务器，高频检测是负担）

2.1.2更新日志
加入了清理所选物品
检查格子非空判定（修复不会清理物品问题）

2.1.1更新日志
修复了关服报错问题（卸载事件钩子写错了）

2.1.0更新日志
配置文件加入了个封禁缓存功能，
该功能基于恋恋修复5号包源码修改而来
移除了/cbag reload指令，重载只需要用tshock自带的/reload指令
测试发现：不开强制开荒清理物品功能会失效望周知

2.0.0更新日志  
经玩家反馈，检测背包的速度太慢特此更新：在配置文件加入了个【每秒更新频率】，数字越少检测速度越快
  
1.9.0更新日志
修复了日志会额外写一个.txt文件在服务器根目录的问题
【检查日志】记录换行间隔不再那么长

1.8.0更新日志
更新将违规信息写入【检查背包】文件夹里的【检查日志】
修改了默认生成的配置文件
【新一王后】→【肉后】：神灯烈焰
考虑tshock醉酒世界BUG骷髅王前掉骨头因素
【骷髅王后】→【世吞克脑后】：虚空保险库、虚空袋、闭合的虚空袋

1.7.0更新日志：
全时期封禁加入：广播盒
优化了/cbag i的显示方式为：每行1个物品，每页15行，只显示当前进度会检测的物品
超进度物品警告方法从私聊玩家改为了发包给所有在线玩家

1.6.0更新日志：
根据Dont dig up地图种子重调了默认配置文件以下物品进度项：
移除：魔法飞刀
骷髅王后进度：海蓝权杖、邪恶三叉戟、泡泡枪、金钥匙
世吞克脑后：陨石锭、星星炮、流星套、太空枪

1.5.0更新日志：
加入了【免检背包】权限名，不再对【未登录】用户进行装备检查

1.4.0更新日志：
将默认生成的配置里：抑郁球、血脉球从【世吞前】进度移除，
根据wiki把各种悠悠球产生时期明确排列了对应时期的进度锁。

1.3.0更新日志：
去除所有重复的.json文件内的超进度物品ID。

1.2.0更新日志：
修复服主超管免检权，将config配置文件补充完整的进度ID

1.1.0更新日志：
完善了checkbag插件的配置文件，
将原有的/checkbag指令与权限重新缩短为：/cbag
将watcher插件的清理违禁物品方式加入到了【检查背包】插件中
配置加入了【哥布林前】、【克眼】、【猪鲨】等更多进度限制
```

## 配置文件
位于`/tshock/检查背包/检查背包.json`,若不存在则会自动创建。  

```json  
{
  "物品查询": "https://terraria.wiki.gg/zh/wiki/Item_IDs",
  "配置说明": "指令/cbag（权限: cbag 和 免检背包）",
  "配置说明2": "[检查次数]影响[清背包次数]，[清掉落物]除了[全时期表]都受影响,",
  "配置说明3": "达到[掉落数量]就算不在表里的物品ID也会清(玩家无法捡起)，[掉落免清表]可无视数量",
  "检查开关": true,
  "清掉落物": true,
  "掉落数量": 500,
  "清除范围/半径": 10,
  "检测间隔/秒": 1,
  "清理频率(每秒帧率)": 60,
  "检查次数": 150000,
  "是否封禁(↑调次数)": false,
  "封禁时长/分钟(过时解封)": 10,
  "是否广播": true,
  "记录日志": true,
  "清掉落表": {
    "74":200
  },
  "掉落免清": [
    8
  ],
  "全时期": {
    "74": 500,
    "75": 999,
    "3617": 1
  },
  "哥布林入侵": {
    "128": 1,
    "405": 1,
    "3993": 1,
    "908": 1,
    "898": 1,
    "1862": 1,
    "5000": 1,
    "1163": 1,
    "983": 1,
    "399": 1,
    "1863": 1,
    "1252": 1,
    "1251": 1,
    "1250": 1,
    "3250": 1,
    "3251": 1,
    "3241": 1,
    "3252": 1,
    "1164": 1,
    "3990": 1,
    "1860": 1,
    "1861": 1,
    "3995": 1,
    "407": 1,
    "395": 1,
    "3122": 1,
    "3121": 1,
    "3036": 1,
    "3123": 1,
    "555": 1,
    "4000": 1,
    "2221": 1,
    "1595": 1,
    "3061": 1,
    "5126": 1,
    "5358": 1,
    "5359": 1,
    "5360": 1,
    "5361": 1,
    "5331": 1
  },
  "史王前": {
    "3318": 1,
    "2430": 1,
    "256": 1,
    "257": 1,
    "258": 1,
    "3090": 1
  },
  "克眼前": {
    "3319": 1,
    "3262": 1,
    "3097": 1
  },
  "鹿角怪前": {
    "5111": 1,
    "5098": 1,
    "5095": 1,
    "5117": 1,
    "5118": 1,
    "5119": 1
  },
  "世吞克脑前": {
    "3320": 1,
    "3321": 1,
    "174": 1,
    "175": 1,
    "122": 1,
    "120": 1,
    "119": 1,
    "231": 1,
    "232": 1,
    "233": 1,
    "2365": 1,
    "4821": 1,
    "121": 1,
    "3223": 1,
    "3224": 1,
    "3266": 1,
    "3267": 1,
    "3268": 1,
    "102": 1,
    "101": 1,
    "100": 1,
    "103": 1,
    "792": 1,
    "793": 1,
    "794": 1,
    "798": 1,
    "3817": 1,
    "3813": 1,
    "3809": 1,
    "3810": 1,
    "197": 1,
    "123": 1,
    "124": 1,
    "125": 1,
    "127": 1,
    "116": 1,
    "117": 1
  },
  "蜂王前": {
    "3322": 1,
    "1123": 1,
    "2888": 1,
    "1121": 1,
    "1132": 1,
    "1130": 1,
    "2431": 1,
    "2502": 1,
    "1249": 1,
    "4007": 1,
    "5294": 1,
    "1158": 1,
    "1430": 1
  },
  "骷髅王前": {
    "3323": 1,
    "273": 1,
    "329": 1,
    "113": 1,
    "683": 1,
    "157": 1,
    "3019": 1,
    "219": 1,
    "218": 1,
    "220": 1,
    "3317": 1,
    "3282": 1,
    "155": 1,
    "156": 1,
    "397": 1,
    "163": 1,
    "164": 1,
    "151": 1,
    "152": 1,
    "153": 1,
    "5074": 1,
    "1313": 1,
    "2999": 1,
    "3000": 1,
    "890": 1,
    "891": 1,
    "904": 1,
    "2623": 1,
    "327": 1
  },
  "肉前": {
    "3324": 1,
    "2673": 1,
    "3991": 1,
    "3366": 1,
    "400": 1,
    "401": 1,
    "402": 1,
    "403": 1,
    "404": 1,
    "391": 1,
    "778": 1,
    "481": 1,
    "524": 1,
    "376": 1,
    "377": 1,
    "378": 1,
    "379": 1,
    "380": 1,
    "382": 1,
    "777": 1,
    "436": 1,
    "525": 1,
    "371": 1,
    "372": 1,
    "373": 1,
    "374": 1,
    "375": 1,
    "381": 1,
    "776": 1,
    "435": 1,
    "1205": 1,
    "1206": 1,
    "1207": 1,
    "1208": 1,
    "1209": 1,
    "1184": 1,
    "1187": 1,
    "1188": 1,
    "1189": 1,
    "1210": 1,
    "1211": 1,
    "1212": 1,
    "1213": 1,
    "1214": 1,
    "1191": 1,
    "1194": 1,
    "1195": 1,
    "1196": 1,
    "1220": 1,
    "1215": 1,
    "1216": 1,
    "1217": 1,
    "1218": 1,
    "1219": 1,
    "1198": 1,
    "1201": 1,
    "1202": 1,
    "1203": 1,
    "1221": 1,
    "1328": 1,
    "2161": 1,
    "684": 1,
    "685": 1,
    "686": 1,
    "726": 1,
    "1264": 1,
    "676": 1,
    "4911": 1,
    "1306": 1,
    "3783": 1,
    "3776": 1,
    "3777": 1,
    "3778": 1,
    "2607": 1,
    "2370": 1,
    "2371": 1,
    "2372": 1,
    "2551": 1,
    "2366": 1,
    "1308": 1,
    "389": 1,
    "426": 1,
    "3051": 1,
    "422": 1,
    "2998": 1,
    "489": 1,
    "490": 1,
    "491": 1,
    "492": 1,
    "493": 1,
    "785": 1,
    "1165": 1,
    "761": 1,
    "822": 1,
    "485": 1,
    "900": 1,
    "497": 1,
    "861": 1,
    "3013": 1,
    "3014": 1,
    "3015": 1,
    "3016": 1,
    "3992": 1,
    "536": 1,
    "897": 1,
    "527": 1,
    "528": 1,
    "520": 1,
    "521": 1,
    "575": 1,
    "535": 1,
    "860": 1,
    "554": 1,
    "862": 1,
    "1613": 1,
    "1612": 1,
    "892": 1,
    "886": 1,
    "901": 1,
    "893": 1,
    "889": 1,
    "903": 1,
    "888": 1,
    "3781": 1,
    "5354": 1,
    "1253": 1,
    "3290": 1,
    "3289": 1,
    "3316": 1,
    "3315": 1,
    "3283": 1,
    "3054": 1,
    "532": 1,
    "1247": 1,
    "1244": 1,
    "1326": 1,
    "522": 1,
    "519": 1,
    "3010": 1,
    "545": 1,
    "546": 1,
    "1332": 1,
    "1334": 1,
    "1335": 1,
    "3011": 1,
    "1356": 1,
    "1336": 1,
    "1346": 1,
    "1350": 1,
    "1357": 1,
    "1347": 1,
    "1351": 1,
    "526": 1,
    "501": 1,
    "516": 1,
    "502": 1,
    "518": 1,
    "515": 1,
    "3009": 1,
    "534": 1,
    "3211": 1,
    "723": 1,
    "514": 1,
    "1265": 1,
    "3788": 1,
    "3210": 1,
    "2270": 1,
    "434": 1,
    "496": 1,
    "3006": 1,
    "3007": 1,
    "3008": 1,
    "3029": 1,
    "3052": 1,
    "5065": 1,
    "4269": 1,
    "4270": 1,
    "4272": 1,
    "3787": 1,
    "1321": 1,
    "4006": 1,
    "4002": 1,
    "3103": 1,
    "3104": 1,
    "2750": 1,
    "905": 1,
    "2584": 1,
    "854": 1,
    "855": 1,
    "3034": 1,
    "3035": 1,
    "1324": 1,
    "3012": 1,
    "4912": 1,
    "544": 1,
    "556": 1,
    "557": 1,
    "3779": 1
  },
  "皇后前": {
    "4957": 1,
    "4987": 1,
    "4980": 1,
    "4981": 1,
    "4982": 1,
    "4983": 1,
    "4984": 1,
    "4758": 1
  },
  "一王前": {
    "3325": 1,
    "3326": 1,
    "3327": 1,
    "1291": 1,
    "5338": 1,
    "533": 1,
    "4060": 1,
    "561": 1,
    "494": 1,
    "495": 1,
    "4760": 1,
    "506": 1,
    "3284": 1,
    "3287": 1,
    "3288": 1,
    "3286": 1,
    "1515": 1,
    "821": 1,
    "748": 1,
    "4896": 1,
    "4897": 1,
    "4898": 1,
    "4899": 1,
    "4900": 1,
    "4901": 1,
    "558": 1,
    "559": 1,
    "553": 1,
    "4873": 1,
    "551": 1,
    "552": 1,
    "1225": 1,
    "578": 1,
    "4678": 1,
    "550": 1,
    "2535": 1,
    "3353": 1,
    "547": 1,
    "548": 1,
    "549": 1,
    "3856": 1,
    "3835": 1,
    "3836": 1,
    "3854": 1,
    "3823": 1,
    "3852": 1,
    "3797": 1,
    "3798": 1,
    "3799": 1,
    "3800": 1,
    "3801": 1,
    "3802": 1,
    "3803": 1,
    "3804": 1,
    "3805": 1,
    "3811": 1,
    "3806": 1,
    "3807": 1,
    "3808": 1,
    "3812": 1
  },
  "三王前": {
    "935": 1,
    "2220": 1,
    "936": 1,
    "1343": 1,
    "674": 1,
    "675": 1,
    "990": 1,
    "579": 1,
    "947": 1,
    "1006": 1,
    "1001": 1,
    "1002": 1,
    "1003": 1,
    "1004": 1,
    "1005": 1,
    "1229": 1,
    "1230": 1,
    "1231": 1,
    "1235": 1,
    "1179": 1,
    "1316": 1,
    "1317": 1,
    "1318": 1,
    "5382": 1
  },
  "猪鲨前": {
    "3330": 1,
    "2609": 1,
    "2622": 1,
    "2624": 1,
    "2621": 1,
    "2611": 1,
    "3367": 1
  },
  "光女前": {
    "4782": 1,
    "4823": 1,
    "4715": 1,
    "4914": 1,
    "5005": 1,
    "4989": 1,
    "4923": 1,
    "4952": 1,
    "4953": 1,
    "4811": 1
  },
  "花前": {
    "3328": 1,
    "1958": 1,
    "1844": 1,
    "4961": 1,
    "3336": 1,
    "963": 1,
    "984": 1,
    "977": 1,
    "3292": 1,
    "3291": 1,
    "1178": 1,
    "3105": 1,
    "3106": 1,
    "2770": 1,
    "1871": 1,
    "1183": 1,
    "4679": 1,
    "4680": 1,
    "1444": 1,
    "1445": 1,
    "1446": 1,
    "3249": 1,
    "1305": 1,
    "757": 1,
    "1569": 1,
    "1571": 1,
    "1572": 1,
    "1156": 1,
    "1260": 1,
    "4607": 1,
    "1508": 1,
    "1946": 1,
    "1947": 1,
    "1931": 1,
    "1928": 1,
    "1929": 1,
    "1930": 1,
    "3997": 1,
    "1552": 1,
    "1546": 1,
    "1547": 1,
    "1548": 1,
    "1549": 1,
    "1550": 1,
    "1866": 1,
    "1570": 1,
    "823": 1,
    "1503": 1,
    "1504": 1,
    "1505": 1,
    "1506": 1,
    "3261": 1,
    "1729": 1,
    "1830": 1,
    "1832": 1,
    "1833": 1,
    "1834": 1,
    "1802": 1,
    "1801": 1,
    "4444": 1,
    "1157": 1,
    "1159": 1,
    "1160": 1,
    "1161": 1,
    "1162": 1,
    "1845": 1,
    "1864": 1,
    "1339": 1,
    "1342": 1,
    "1341": 1,
    "1340": 1,
    "1255": 1,
    "3107": 1,
    "3108": 1,
    "1782": 1,
    "1783": 1,
    "1910": 1,
    "1300": 1,
    "1254": 1,
    "760": 1,
    "759": 1,
    "758": 1,
    "1784": 1,
    "1785": 1,
    "1835": 1,
    "1836": 1,
    "4457": 1,
    "4458": 1,
    "771": 1,
    "772": 1,
    "773": 1,
    "774": 1,
    "4445": 1,
    "4446": 1,
    "4447": 1,
    "4448": 1,
    "4449": 1,
    "4459": 1,
    "1259": 1,
    "3018": 1,
    "1826": 1,
    "1513": 1,
    "938": 1,
    "3998": 1,
    "1327": 1,
    "4812": 1,
    "1301": 1,
    "4005": 1
  },
  "石前": {
    "3329": 1,
    "3860": 1,
    "4807": 1,
    "1248": 1,
    "3337": 1,
    "1294": 1,
    "1122": 1,
    "1295": 1,
    "1296": 1,
    "1297": 1,
    "3110": 1,
    "1865": 1,
    "899": 1,
    "2218": 1,
    "2199": 1,
    "2200": 1,
    "2201": 1,
    "2202": 1,
    "2280": 1,
    "948": 1,
    "3871": 1,
    "3872": 1,
    "3873": 1,
    "3874": 1,
    "3875": 1,
    "3876": 1,
    "3877": 1,
    "3878": 1,
    "3879": 1,
    "3880": 1,
    "3882": 1,
    "1258": 1,
    "2797": 1,
    "2882": 1,
    "2769": 1,
    "2880": 1,
    "2795": 1,
    "2749": 1,
    "2796": 1,
    "3883": 1,
    "3870": 1,
    "3859": 1,
    "3858": 1,
    "3827": 1,
    "1261": 1
  },
  "拜月前": {
    "3456": 1,
    "3457": 1,
    "3458": 1,
    "3459": 1,
    "3473": 1,
    "3543": 1,
    "3474": 1,
    "3531": 1,
    "3475": 1,
    "3540": 1,
    "3542": 1,
    "3476": 1,
    "3549": 1,
    "3544": 1
  },
  "月前": {
    "3332": 1,
    "3601": 1,
    "3460": 1,
    "3467": 1,
    "3567": 1,
    "3568": 1,
    "2757": 1,
    "2758": 1,
    "2759": 1,
    "3469": 1,
    "3381": 1,
    "3382": 1,
    "3383": 1,
    "3470": 1,
    "2760": 1,
    "2761": 1,
    "2762": 1,
    "3471": 1,
    "2763": 1,
    "2764": 1,
    "2765": 1,
    "3468": 1,
    "3466": 1,
    "2776": 1,
    "2781": 1,
    "2786": 1,
    "2774": 1,
    "2779": 1,
    "2784": 1,
    "3464": 1,
    "2768": 1,
    "3384": 1,
    "1131": 1,
    "4954": 1,
    "4956": 1,
    "3065": 1,
    "3063": 1,
    "1553": 1,
    "3930": 1,
    "3570": 1,
    "3389": 1,
    "3541": 1,
    "3569": 1,
    "3571": 1,
    "5335": 1,
    "5364": 1
  }
}
```
<br>
