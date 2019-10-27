using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BangDreamMusicscoreConverter.DataClass.Bestdori;
using Newtonsoft.Json;

namespace BangDreamMusicscoreConverter.DataClass.DefaultScore
{
    /// <summary>
    ///     默认谱面类
    /// </summary>
    public class DefaultScore
    {
        /// <summary>
        ///     延迟时间(单位:毫秒)
        /// </summary>
        public int Delay_ms { get; set; }

        /// <summary>
        ///     每分钟节拍数
        /// </summary>
        public float Bpm { get; set; }

        /// <summary>
        ///     谱面数据
        /// </summary>
        public List<Note> Notes { get; set; }

        /// <summary>
        ///     输出对应格式的谱面文本
        /// </summary>
        /// <param name="convertTypeTo"></param>
        /// <returns></returns>
        public string ToString(ConvertTypeTo convertTypeTo)
        {
            try
            {
                switch (convertTypeTo)
                {
                    case ConvertTypeTo.bestdori: return ToBestdoriScore();
                    case ConvertTypeTo.bangSimulator: return ToBangSimulatorScore();
                    case ConvertTypeTo.bangbangboom: return ToBangbangboomScore();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return "";
        }

        #region 各谱面格式输出方法

        /// <summary>
        ///     输出为bangSimulator播放器格式
        /// </summary>
        /// <returns></returns>
        private string ToBangSimulatorScore()
        {
            var str = "";
            str += $"{Delay_ms}{Environment.NewLine}";
            str += $"{Bpm}{Environment.NewLine}";
            str += $"0/0/0{Environment.NewLine}";
            return Notes.Aggregate(str,
                (current, note) => current + $"{note.Time}/{(int) note.NoteType}/{note.Track}{Environment.NewLine}");
        }

        /// <summary>
        ///     输出为bestdori制谱器格式
        /// </summary>
        /// <returns></returns>
        private string ToBestdoriScore()
        {
            var score = new ArrayList();
            var head = new Head
            {
                beat = Delay_ms / 60000 * Bpm,
                bpm = Bpm
            };
            score.Add(head);
            var tempList = new List<Bestdori.Note>();
            foreach (var note in Notes)
            {
                var tempNote = new Bestdori.Note();
                switch (note.NoteType)
                {
                    case NoteType.白键:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Single
                        };
                        break;
                    case NoteType.粉键:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Single,
                            flick = true
                        };
                        break;
                    case NoteType.技能:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Single,
                            skill = true
                        };
                        break;
                    case NoteType.滑条a_开始:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.A,
                            start = true
                        };
                        break;
                    case NoteType.滑条a_中间:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.A
                        };
                        break;
                    case NoteType.滑条a_结束:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.A,
                            end = true
                        };
                        break;
                    case NoteType.长键_开始:
                    case NoteType.滑条b_开始:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B,
                            start = true
                        };
                        break;
                    case NoteType.滑条b_中间:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B
                        };
                        break;
                    case NoteType.长键_结束:
                    case NoteType.滑条b_结束:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B,
                            end = true
                        };
                        break;
                    case NoteType.滑条a_粉键结束:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.A,
                            end = true,
                            flick = true
                        };
                        break;
                    case NoteType.长键_粉键结束:
                    case NoteType.滑条b_粉键结束:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B,
                            end = true,
                            flick = true
                        };
                        break;
                    case NoteType.改变bpm:
                        break;
                }

                tempList.Add(tempNote);
            }

            //先按时间排，然后让滑条结束音符优先在前，其余按轨道从左到右排
            score.AddRange(tempList.OrderBy(p => p.beat).ThenByDescending(p => p.end).ThenBy(p => p.lane).ToList());

            return JsonConvert.SerializeObject(score, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
        }

        /// <summary>
        ///     输出为bangbangboom制谱器格式
        /// </summary>
        /// <returns></returns>
        private string ToBangbangboomScore()
        {
            var str = "";
            str += "\n\n";
            str += $"+|{Delay_ms / 1000}|{Bpm}|4";
            str += "\n\n";
            var index = 0;
            foreach (var note in Notes)
            {
                switch (note.NoteType)
                {
                    case NoteType.技能:
                    case NoteType.白键:
                        str += $"s|{(int) (note.Time * 24)}:{note.Track - 1}\n";
                        break;
                    case NoteType.粉键:
                        str += $"f|{(int) (note.Time * 24)}:{note.Track - 1}\n";
                        break;
                    case NoteType.滑条a_开始:
                    {
                        var isFlick = 0;
                        for (var i = index + 1; i < Notes.Count; i++)
                        {
                            if (Notes[i].NoteType == NoteType.滑条a_结束 && Notes[i].Time != note.Time)
                            {
                                isFlick = 0;
                                break;
                            }

                            if (Notes[i].NoteType == NoteType.滑条a_粉键结束 && Notes[i].Time != note.Time)
                            {
                                isFlick = 1;
                                break;
                            }
                        }

                        str += $"l|{isFlick}|{(int) (note.Time * 24)}:{note.Track - 1}";
                        for (var i = index + 1; i < Notes.Count; i++)
                        {
                            if (Notes[i].NoteType == NoteType.滑条a_中间 && Notes[i].Time != note.Time)
                            {
                                str += $"|{(int) (Notes[i].Time * 24)}:{Notes[i].Track - 1}";
                                continue;
                            }

                            if ((Notes[i].NoteType == NoteType.滑条a_结束 ||
                                 Notes[i].NoteType == NoteType.滑条a_粉键结束) && Notes[i].Time != note.Time)
                            {
                                str += $"|{(int) (Notes[i].Time * 24)}:{Notes[i].Track - 1}\n";
                                break;
                            }
                        }
                    }
                        break;
                    case NoteType.滑条b_开始:
                    {
                        var isFlick = 0;
                        for (var i = index + 1; i < Notes.Count; i++)
                        {
                            if (Notes[i].NoteType == NoteType.滑条b_结束 && Notes[i].Time != note.Time)
                            {
                                isFlick = 0;
                                break;
                            }

                            if (Notes[i].NoteType == NoteType.滑条b_粉键结束 && Notes[i].Time != note.Time)
                            {
                                isFlick = 1;
                                break;
                            }
                        }

                        str += $"l|{isFlick}|{(int) (note.Time * 24)}:{note.Track - 1}";
                        for (var i = index + 1; i < Notes.Count; i++)
                        {
                            if (Notes[i].NoteType == NoteType.滑条b_中间 && Notes[i].Time != note.Time)
                            {
                                str += $"|{(int) (Notes[i].Time * 24)}:{Notes[i].Track - 1}";
                                continue;
                            }

                            if ((Notes[i].NoteType == NoteType.滑条b_结束 ||
                                 Notes[i].NoteType == NoteType.滑条b_粉键结束) && Notes[i].Time != note.Time)
                            {
                                str += $"|{(int) (Notes[i].Time * 24)}:{Notes[i].Track - 1}\n";
                                break;
                            }
                        }
                    }
                        break;
                    case NoteType.改变bpm:
                        break;
                }

                index++;
            }

            return str;
        }

        #endregion
    }
}