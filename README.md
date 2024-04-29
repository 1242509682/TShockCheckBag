## 检查背包[超进度物品检测]（原名检查包）

## 介绍
作者: hufang360  
修改：羽学  
更新日期:2024年4月11日 23:00  
插件版本:2.0.0  
本插件可通过修改【检查背包。json】配置文件来控制：  
检测频率、警告次数、封禁时间（封禁只封UUID，时间一过自动解封)  
并加入了常见的进度项,默认生成的配置文件完善了大部分进度物品身份证.  

## 关于默认生成的配置文件说明
```
由于陨石锭可以在地狱金箱子里能找到、Tshock的醉酒世界特性有骷髅王前会刷怪的BUG  
使得玩家老板前就可以获得：  
金钥匙、邪眼、村正、钴护盾、骨头、死灵套、骨鞭、陨石套等超进度物品  
并考虑到微光湖的存在将:【扩音器、邪眼、反诅咒咒语】设为【骷髅王进度】  
同时未把【新六矿近战系武器、锤斧类工具】加入到默认的配置文件中  
使用者可根据自己服务器情况自由修改配置文件调整合理的进度物品身份证明  
修改完配置文件后可使用【/cbag重新加载】重载配置文件实时同步服务器  
```

## 指令
```
/cbag b，列出封禁记录  
/cbag i，列出违规物品  
/cbag重新加载，重载配置  
```

## 权限
```
cbag  
免检背包
```

## 授权示意：
```
以上指令权限名为【中央银行】可分配给玩家使用：  
/group addperm默认cbag

免检权限名【免检背包】,如需免检指定玩家可使用以下指令  
1.新增一个项目组,拥有【免检背包】权限  
/group添加项目免检背包

2 .项目组继承系统默认值所拥有的权限：  
/group父项默认值  

3.将指定玩家分配到项目组：  
/用户组玩家名字项目  
```

## 配置文件
位于`。/tshock/检查背包/检查背包.json`,若不存在则会自动创建。  

示例：  
```json  
{
"配置说明": "重载配置文件请输入：/cbag reload",
"物品查询": "https://terraria.wiki.gg/zh/wiki/Item_IDs",
"检查间隔(秒)": 5,
"更新频率(越小越快)": 60,
"封禁时长(分钟)": 10,
"警告次数(封禁)": 15,
"全时期": [  
    {  
"id":74，  
"数量": 999,  
"名称": "铂金币"  
    }  
  ],  
"骷髅王前": [
    {
"id":29，
"数量": 20,
"名称": "生命水晶"
    }
  ],
"肉前": [],
  "一王前": [
    {
      "id": 1291,
      "数量": 2,
      "名称": "生命果"
    }
  ],
  "三王前": [],
  "猪鲨前": [],
  "花前": [],
  "石前": []
}
```
`检测间隔`的单位是秒。

`封禁时长`的单位是分钟。

`更新频率`的单位是每秒次数（默认1秒60次）

`警告次数`表示第x次检测到次违规物品时，即封禁该玩家，封禁只封UUID,封禁时间结束自动解封。

## 更新日志
```
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
<br>
