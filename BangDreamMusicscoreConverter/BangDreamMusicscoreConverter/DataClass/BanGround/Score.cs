using System.Collections.Generic;
using System.ComponentModel;

namespace BangDreamMusicscoreConverter.DataClass.BanGround
{
    public class Score
    {
        /// <summary>
        ///     难度名
        /// </summary>
        public string difficulty { get; set; }

        /// <summary>
        ///     难度等级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        ///     偏移(单位ms)
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        ///     物件
        /// </summary>
        public List<note> notes { get; set; }
    }

    public class note
    {
        /// <summary>
        ///     类型
        /// </summary>
        /// <remarks>
        ///     <para>BPM,bpm</para>
        ///     <para>Single,单键/滑条起始</para>
        ///     <para>Flick,滑键/滑条结尾</para>
        ///     <para>SlideTick,滑条节点</para>
        ///     <para>SlideTickEnd,滑条结尾</para>
        /// </remarks>
        public string type { get; set; }

        /// <summary>
        ///     所在位置(长度必为3，表示为带分数)
        /// </summary>
        public List<int> beat { get; set; }

        /// <summary>
        ///     轨道
        /// </summary>
        public int lane { get; set; }

        /// <summary>
        ///     节点栈，拥有此属性的物件视为对应滑条的一部分
        /// </summary>
        public int tickStack { get; set; }

        /// <summary>
        ///     bpm的数值
        /// </summary>
        public float value { get; set; }

        /// <summary>
        ///     不知道干啥的，Wiki没写
        /// </summary>
        public List<object> anims { get; set; }
    }
}