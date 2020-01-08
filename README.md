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

- 支持bangSimulator谱面、bestdori谱面、bangbangboom谱面、bangCraft工程、bandori database谱面json的互转
  - 偷懒1：bangCraft制谱器自带输出bangSimulator谱面功能，因此程序不提供bangCraft输入（先用制谱器导出bangSimulator再转换）
  - 偷懒2：bandori database网站播放器不提供自制谱上传，因此程序不提供bandori database输出

- 复制和保存谱面文件

<br/>

已知的问题：

- 从BanG! simulator chart_downloader下载的邦邦官谱转换后会乱掉，体现在滑条的头尾不一致（我不知道simulator的long note应该如何处理_(:з」∠)_ ）
 而通过BanG!Craft制作的自制谱面导出为simulator谱面则没有发现这个问题
 
- bangbangboom谱面不标记每个滑条音符的归属，而是将同一滑条内的音符合为一组。导入算法中作ab交替分类处理，因此导入转换成其他谱面后有很大可能出现滑条乱连的情况，需要手动修正

- 各谱面转换的白键和粉键目前没有发现问题，滑条可能会有问题（即使bangbangboom预览时看起来没问题，玩的时候也有可能出现判定错误），需要手动检查和修正
<br/>
<br/>

如果上面还是看不懂那么看这里：
- 如果你想把在某个平台自制的谱放到其他平台上：
  - 用bangCraft做的：bangCraft=>bangSimulator=>转换程序=>bangSimulator/bestdori/bangbangboom
  - 用bestdori/bangbangboom做的：bestdori/bangbangboom=>转换程序=>bangSimulator/bestdori/bangbangboom
  
- 如果你想基于官方谱进行扩展：
  - 用 https://api.bandori.ga/v1/jp/music/chart/(歌曲Id)/(难度) 这个接口获取官谱json（bandori database），举例：https://api.bandori.ga/v1/jp/music/chart/232/expert ，然后 bandori database=>转换程序=>bangCraft/bestdori/bangbangboom
  
- 如果你跟我一样比较喜欢用上古工具bangCraft做谱：
  - bangSimulator/bestdori/bangbangboom/bandori database=>转换程序=>bangCraft，生成的文本放到BanG!Craft_Beta1.5/BanG!Craft_Data/Save里面，不要带txt后缀
  
<br/>
<br/>

因为是上班划水时间做，进度会很慢。哪位大佬有什么建议的欢迎联系

qq : 664823818
