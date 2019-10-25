using BangDreamMusicscoreConverter.DataClass.DefaultScore;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BangDreamMusicscoreConverter.Model
{
    public class DataBusiness
    {
        /// <summary>
        /// 从谱面文本构造谱面对象
        /// </summary>
        /// <param name="scoreString">谱面文本</param>
        public DefaultScore GetDefaultScoreFromString(string scoreString)
        {
            var defaultScore = new DefaultScore();
            var scoreStringArray = scoreString.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(scoreStringArray[0], out int delay_ms) && float.TryParse(scoreStringArray[1], out float bpm))
            {
                defaultScore.Delay_ms = delay_ms;
                defaultScore.Bpm = bpm;
            }
            else
                MessageBox.Show("转换出错，请检查原谱面文本");

            var notes = new List<Note>();
            for (int i = 3; i < scoreStringArray.Length; i++)
            {
                var str = scoreStringArray[i].Split('/');
                if (double.TryParse(str[0], out double time) && Enum.TryParse(str[1], out NoteType noteType) && int.TryParse(str[2], out int track))
                    notes.Add(new Note
                    {
                        Time = time,
                        NoteType = noteType,
                        Track = track
                    });
                else
                    MessageBox.Show("转换出错，请检查原谱面文本");
            }
            defaultScore.Notes = notes;
            return defaultScore;
        }
    }
}
