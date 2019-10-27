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

1、导入谱面文件，支持拖拽

2、支持bangSimulator谱面、bestdori谱面、bangbangboom谱面互转

3、复制和保存谱面文件

<br/>

已知的问题：

1、从BanG! simulator chart_downloader下载的邦邦官谱转换后会乱掉，体现在滑条的头尾不一致（我不知道simulator的long note应该如何处理_(:з」∠)_ ）
 而通过BanG!Craft制作的自制谱面导出为simulator谱面则没有发现这个问题
 
2、bangbangboom谱面不标记每个滑条音符的归属，而是将同一滑条内的音符合为一组。导入算法中作ab交替分类处理，因此导入转换成其他谱面后有很大可能出现滑条乱连的情况，需要手动修正

3、暂时不支持转换bangCraft的谱面

4、各谱面转换的白键和粉键目前没有发现问题，滑条可能会有问题（即使bangbangboom预览时看起来没问题，玩的时候也有可能出现判定错误），需要手动检查和修正
<br/>
<br/>


哪位大佬有什么建议的欢迎联系

qq : 664823818
