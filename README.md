有大佬做了网页版bestdori和bangbangboom互转，推荐使用：
https://player.bangbangboom.ml/
<br/>
<br/>
<br/>
<br/>

# BangDream-Musicscore-Converter
# 邦邦谱面转换器

[下载](https://github.com/xyh20180101/BangDream-Musicscore-Converter/releases)
<br/>
<br/>

运行需要.NET Framework 4.7.2(转换程序) .NET core 3.1(bestdori谱面获取)  

## 界面
![image](https://raw.githubusercontent.com/xyh20180101/BangDream-Musicscore-Converter/master/README_img/img2.png)
<br/>
<br/>
<br/>
## 支持的功能

- 导入谱面文件，支持拖拽

- 从bestdori自制谱id获取谱面文件

- 谱面重复note检测

- 复制和保存谱面文件

- 支持 **bangSimulator**谱面、**bestdori**谱面、**bangbangboom**谱面、**bangCraft**工程、**bandori database**谱面json、**BMS**谱面的转换
  - 偷懒1：程序没有bangCraft输入（建议先用bangCraft制谱器导出bangSimulator再转换）
  - 偷懒2：程序没有bandori database输出

```C#
/// <summary>
///     转换输入类型
/// </summary>
public enum ConvertTypeFrom
{
	bestdori = 0,
	bangbangboom = 1,
	bangSimulator = 2,
	bandori = 3
}

/// <summary>
///     转换输出类型
/// </summary>
public enum ConvertTypeTo
{
	bestdori = 0,
	bangbangboom = 1,
	bangSimulator = 2,
	bangCraft = 3,
	bms = 4
}
```
<br/>

## 已知的问题

- **强烈建议谱面制作和转换时delay设为0，通过编辑音频达到同步，否则极大概率出现错误**

- 从BanG! simulator chart_downloader下载的邦邦官谱滑条有问题，不要用那个，正确获取官谱的方法下面有说

- 各谱面转换的白键和粉键目前没有发现问题，滑条可能会有问题，音符可能重复，**一定要人工检查和修正**
  - 即使bangbangboom预览时看起来没问题，玩的时候也有可能出现判定错误
  - 如果用黑科技玩bestdori的谱，一定不能有重叠音符
<br/>

## 如果上面还是看不懂那么看这里
- 如果你想把在某个平台自制的谱放到其他平台上：
  - 用bangCraft做的：bangCraft=>bangSimulator=>转换程序=>bangSimulator/bestdori/bangbangboom
  - 用bestdori/bangbangboom做的：bestdori/bangbangboom=>转换程序=>bangSimulator/bestdori/bangbangboom
  
- 如果你想基于官方谱进行扩展：
  - 用 `https://api.bandori.ga/v1/jp/music/chart/(歌曲Id)/(难度)` 这个接口获取官谱json（bandori database），举例：https://api.bandori.ga/v1/jp/music/chart/232/expert ，然后 bandori database=>转换程序=>bangCraft/bestdori/bangbangboom
  
- 如果你跟我一样比较喜欢用上古工具bangCraft做谱：
  - bangSimulator/bestdori/bangbangboom/bandori database=>转换程序=>bangCraft，生成的文本放到BanG!Craft_Beta1.5/BanG!Craft_Data/Save里面，不要带txt后缀
  
- 如果你想用官谱替换黑科技
  - 需要的工具包自行获取，这里只提供BMS谱面转换，免去非要从bestdori上传谱面再导入的麻烦。导出后还需要修改#BGM bgmxxx序号
  
<br/>
<br/>

因为是上班划水时间做，进度会很慢。哪位大佬有什么建议的欢迎联系

qq : 664823818
