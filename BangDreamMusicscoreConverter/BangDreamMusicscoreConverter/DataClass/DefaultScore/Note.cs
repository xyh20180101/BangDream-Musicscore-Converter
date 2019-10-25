namespace BangDreamMusicscoreConverter.DataClass.DefaultScore
{
    /// <summary>
    /// DefaultScore的音符信息
    /// </summary>
    public class Note
    {
        /// <summary>
        /// 时间
        /// </summary>
        public double Time { get; set; }

        /// <summary>
        /// 音符类型
        /// </summary>
        public NoteType NoteType { get; set; }

        /// <summary>
        /// 所在轨道
        /// </summary>
        public int Track { get; set; }
    }
}
