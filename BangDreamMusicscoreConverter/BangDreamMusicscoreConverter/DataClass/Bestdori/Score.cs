using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BangDreamMusicscoreConverter.DataClass.Bestdori
{
    /// <summary>
    ///     bestdori谱面头部信息
    /// </summary>
    public class Head
    {
        /// <summary>
        /// </summary>
        public string cmd = "BPM";

        /// <summary>
        ///     种类
        /// </summary>
        public string type = "System";

        /// <summary>
        ///     所在节拍
        /// </summary>
        [DefaultValue(-1)]
        public double beat { get; set; }

        /// <summary>
        ///     每分钟节拍数
        /// </summary>
        public float bpm { get; set; }
    }

    /// <summary>
    ///     bestdori谱面音符信息
    /// </summary>
    public class Note
    {
        /// <summary>
        ///     种类
        /// </summary>
        public string type = "Note";

        /// <summary>
        ///     轨道
        /// </summary>
        public int lane { get; set; }

        /// <summary>
        ///     所在节拍
        /// </summary>
        public double beat { get; set; }

        /// <summary>
        ///     音符类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public NoteType note { get; set; }

        /// <summary>
        ///     是否技能键
        /// </summary>
        public bool skill { get; set; }

        /// <summary>
        ///     是否滑键
        /// </summary>
        public bool flick { get; set; }

        /// <summary>
        ///     滑条类别
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PosType pos { get; set; }

        /// <summary>
        ///     是否滑条开始
        /// </summary>
        public bool start { get; set; }

        /// <summary>
        ///     是否滑条结束
        /// </summary>
        public bool end { get; set; }
    }

    public enum NoteType
    {
        Single = 1,
        Slide = 2
    }

    public enum PosType
    {
        A = 1,
        B = 2
    }
}