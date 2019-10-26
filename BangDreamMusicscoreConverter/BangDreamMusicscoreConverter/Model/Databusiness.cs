using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using BangDreamMusicscoreConverter.DataClass;
using BangDreamMusicscoreConverter.DataClass.Bestdori;
using BangDreamMusicscoreConverter.DataClass.DefaultScore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Note = BangDreamMusicscoreConverter.DataClass.DefaultScore.Note;
using NoteType = BangDreamMusicscoreConverter.DataClass.DefaultScore.NoteType;

namespace BangDreamMusicscoreConverter.Model
{
    public class DataBusiness
    {
        public DefaultScore GetDefaultScore(string scoreString, ConvertTypeFrom convertTypeFrom)
        {
            try
            {
                switch (convertTypeFrom)
                {
                    case ConvertTypeFrom.bestdori: return GetDefaultScoreFromBestdoriScore(scoreString);
                    case ConvertTypeFrom.bangSimulator: return GetDefaultScoreFromBangSimulatorScore(scoreString);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return new DefaultScore();
        }

        /// <summary>
        ///     从simulator谱面文本构造谱面对象
        /// </summary>
        /// <param name="scoreString">谱面文本</param>
        public DefaultScore GetDefaultScoreFromBangSimulatorScore(string scoreString)
        {
            var defaultScore = new DefaultScore();
            var scoreStringArray =
                scoreString.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(scoreStringArray[0], out var delay_ms) && float.TryParse(scoreStringArray[1], out var bpm))
            {
                defaultScore.Delay_ms = delay_ms;
                defaultScore.Bpm = bpm;
            }
            else
            {
                throw new Exception("转换出错，请检查原谱面文本");
            }

            var notes = new List<Note>();
            for (var i = 3; i < scoreStringArray.Length; i++)
            {
                var str = scoreStringArray[i].Split('/');
                if (double.TryParse(str[0], out var time) && Enum.TryParse(str[1], out NoteType noteType) &&
                    int.TryParse(str[2], out var track))
                    notes.Add(new Note
                    {
                        Time = time,
                        NoteType = noteType,
                        Track = track
                    });
                else
                    throw new Exception("转换出错，请检查原谱面文本");
            }

            defaultScore.Notes = notes;
            return defaultScore;
        }

        /// <summary>
        ///     从bestdori谱面文本构造谱面对象
        /// </summary>
        /// <param name="scoreString">谱面文本</param>
        public DefaultScore GetDefaultScoreFromBestdoriScore(string scoreString)
        {
            var defaultScore = new DefaultScore();
            var arrayList = JsonConvert.DeserializeObject<ArrayList>(scoreString);
            var head = new Head
            {
                bpm = ((JObject) arrayList[0])["bpm"].ToObject<float>(),
                beat = ((JObject) arrayList[0])["beat"].ToObject<double>()
            };
            defaultScore.Bpm = head.bpm;
            defaultScore.Delay_ms = (int) (head.beat / head.bpm * 60000);

            //提取note列表
            arrayList.RemoveAt(0);
            var tempJson = JsonConvert.SerializeObject(arrayList);
            var tempList = JsonConvert.DeserializeObject<List<DataClass.Bestdori.Note>>(tempJson);

            var notes = new List<Note>();
            foreach (var note in tempList)
            {
                var tempNote = new Note {Time = note.beat, Track = note.lane};

                if (note.note == DataClass.Bestdori.NoteType.Single &&
                    !note.skill &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.白键;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Single &&
                    note.skill &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.技能;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Single &&
                    !note.skill &&
                    note.flick)
                {
                    tempNote.NoteType = NoteType.粉键;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.A &&
                    note.start &&
                    !note.end &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.滑条a_开始;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.A &&
                    !note.start &&
                    !note.end &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.滑条a_中间;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.A &&
                    !note.start &&
                    note.end &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.滑条a_结束;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.A &&
                    !note.start &&
                    note.end &&
                    note.flick)
                {
                    tempNote.NoteType = NoteType.滑条a_粉键结束;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.B &&
                    note.start &&
                    !note.end &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.滑条b_开始;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.B &&
                    !note.start &&
                    !note.end &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.滑条b_中间;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.B &&
                    !note.start &&
                    note.end &&
                    !note.flick)
                {
                    tempNote.NoteType = NoteType.滑条b_结束;
                    notes.Add(tempNote);
                    continue;
                }

                if (note.note == DataClass.Bestdori.NoteType.Slide &&
                    note.pos == PosType.B &&
                    !note.start &&
                    note.end &&
                    note.flick)
                {
                    tempNote.NoteType = NoteType.滑条b_粉键结束;
                    notes.Add(tempNote);
                    continue;
                }

                throw new Exception("bestdori=>bangSimulator音符转换失败");
            }

            defaultScore.Notes = notes;
            return defaultScore;
        }
    }
}