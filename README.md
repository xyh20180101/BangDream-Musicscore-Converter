有大佬做了网页版bestdori和bangbangboom互转，推荐使用：
https://player.bangbangboom.ml/
<br/>
<br/>
<br/>
<br/>

# BangDream-Musicscore-Converter
邦邦谱面转换器

[下载](https://github.com/xyh20180101/BangDream-Musicscore-Converter/releases)
<br/>
<br/>

运行需要.NET Framework 4.7.2  

界面：
![image](https://raw.githubusercontent.com/xyh20180101/BangDream-Musicscore-Converter/master/README_img/img1.png)
<br/>
<br/>
<br/>
开发中，目前支持的功能：

- 导入谱面文件，支持拖拽

- 复制和保存谱面文件

- 支持 **bangSimulator**谱面、**bestdori**谱面、**bangbangboom**谱面、**bangCraft**工程、**bandori database**谱面json 的互转
  - 偷懒1：程序没有bangCraft输入（建议先用bangCraft制谱器导出bangSimulator再转换）
  - 偷懒2：程序没有bandori database输出

<br/>

已知的问题：

- 从BanG! simulator chart_downloader下载的邦邦官谱滑条有问题，不要用那个，正确获取官谱的方法下面有说

- 各谱面转换的白键和粉键目前没有发现问题，滑条可能会有问题，音符可能重复，**一定要人工检查和修正**
  - 即使bangbangboom预览时看起来没问题，玩的时候也有可能出现判定错误
  - 如果用黑科技玩bestdori的谱，一定不能有重叠音符，可以用μBMSC查找重叠点
<br/>
<br/>

如果上面还是看不懂那么看这里：
- 如果你想把在某个平台自制的谱放到其他平台上：
  - 用bangCraft做的：bangCraft=>bangSimulator=>转换程序=>bangSimulator/bestdori/bangbangboom
  - 用bestdori/bangbangboom做的：bestdori/bangbangboom=>转换程序=>bangSimulator/bestdori/bangbangboom
  
- 如果你想基于官方谱进行扩展：
  - 用 `https://api.bandori.ga/v1/jp/music/chart/(歌曲Id)/(难度)` 这个接口获取官谱json（bandori database），举例：https://api.bandori.ga/v1/jp/music/chart/232/expert ，然后 bandori database=>转换程序=>bangCraft/bestdori/bangbangboom
  
- 如果你跟我一样比较喜欢用上古工具bangCraft做谱：
  - bangSimulator/bestdori/bangbangboom/bandori database=>转换程序=>bangCraft，生成的文本放到BanG!Craft_Beta1.5/BanG!Craft_Data/Save里面，不要带txt后缀
  
<br/>
<br/>

因为是上班划水时间做，进度会很慢。哪位大佬有什么建议的欢迎联系

qq : 664823818
