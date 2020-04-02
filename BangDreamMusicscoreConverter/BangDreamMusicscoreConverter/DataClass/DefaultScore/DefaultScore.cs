using BangDreamMusicscoreConverter.DataClass.BanGround;
using BangDreamMusicscoreConverter.DataClass.Bestdori;
using BangDreamMusicscoreConverter.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;

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
                    case ConvertTypeTo.bangCraft: return ToBangCraftScore();
                    case ConvertTypeTo.bms: return ToBMS();
                    case ConvertTypeTo.BanGround: return ToBanGround();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "转换失败");
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
                (current, note) => current + $"{note.Time}/{(int)note.NoteType}/{note.Track}{Environment.NewLine}");
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
            var lastPosType = PosType.B;
            var index = 0;
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
                        tempList.Add(tempNote);
                        break;

                    case NoteType.粉键:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Single,
                            flick = true
                        };
                        tempList.Add(tempNote);
                        break;

                    case NoteType.技能:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Single,
                            skill = true
                        };
                        tempList.Add(tempNote);
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
                        lastPosType = tempNote.pos;
                        tempList.Add(tempNote);
                        break;

                    case NoteType.滑条a_中间:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.A
                        };
                        tempList.Add(tempNote);
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
                        tempList.Add(tempNote);
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
                        tempList.Add(tempNote);
                        break;

                    case NoteType.滑条b_开始:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B,
                            start = true
                        };
                        lastPosType = tempNote.pos;
                        tempList.Add(tempNote);
                        break;

                    case NoteType.滑条b_中间:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B
                        };
                        tempList.Add(tempNote);
                        break;

                    case NoteType.滑条b_结束:
                        tempNote = new Bestdori.Note
                        {
                            lane = note.Track,
                            beat = note.Time,
                            note = Bestdori.NoteType.Slide,
                            pos = PosType.B,
                            end = true
                        };
                        tempList.Add(tempNote);
                        break;

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
                        tempList.Add(tempNote);
                        break;

                    case NoteType.长键_开始:
                        {
                            tempNote = new Bestdori.Note
                            {
                                lane = note.Track,
                                beat = note.Time,
                                note = Bestdori.NoteType.Slide,
                                pos = lastPosType == PosType.A ? PosType.B : PosType.A,
                                start = true
                            };
                            lastPosType = tempNote.pos;

                            for (var i = index + 1; i < Notes.Count; i++)
                            {
                                if (Notes[i].NoteType == NoteType.长键_结束 && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)
                                {
                                    var endNote = new Bestdori.Note
                                    {
                                        lane = Notes[i].Track,
                                        beat = Notes[i].Time,
                                        note = Bestdori.NoteType.Slide,
                                        pos = tempNote.pos,
                                        end = true
                                    };
                                    tempList.Add(endNote);
                                    break;
                                }

                                if (Notes[i].NoteType == NoteType.长键_粉键结束 && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)

                                {
                                    var endNote = new Bestdori.Note
                                    {
                                        lane = Notes[i].Track,
                                        beat = Notes[i].Time,
                                        note = Bestdori.NoteType.Slide,
                                        pos = tempNote.pos,
                                        end = true,
                                        flick = true
                                    };
                                    tempList.Add(endNote);
                                    break;
                                }
                            }
                        }
                        tempList.Add(tempNote);
                        break;

                    case NoteType.改变bpm:
                        break;
                }

                index++;
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
                        str += $"s|{(int)(note.Time * 24)}:{note.Track - 1}\n";
                        break;

                    case NoteType.粉键:
                        str += $"f|{(int)(note.Time * 24)}:{note.Track - 1}\n";
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

                            str += $"l|{isFlick}|{(int)(note.Time * 24)}:{note.Track - 1}";
                            for (var i = index + 1; i < Notes.Count; i++)
                            {
                                if (Notes[i].NoteType == NoteType.滑条a_中间 && Notes[i].Time != note.Time)
                                {
                                    str += $"|{(int)(Notes[i].Time * 24)}:{Notes[i].Track - 1}";
                                    continue;
                                }

                                if ((Notes[i].NoteType == NoteType.滑条a_结束 ||
                                     Notes[i].NoteType == NoteType.滑条a_粉键结束) && Notes[i].Time != note.Time)
                                {
                                    str += $"|{(int)(Notes[i].Time * 24)}:{Notes[i].Track - 1}\n";
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

                            str += $"l|{isFlick}|{(int)(note.Time * 24)}:{note.Track - 1}";
                            for (var i = index + 1; i < Notes.Count; i++)
                            {
                                if (Notes[i].NoteType == NoteType.滑条b_中间 && Notes[i].Time != note.Time)
                                {
                                    str += $"|{(int)(Notes[i].Time * 24)}:{Notes[i].Track - 1}";
                                    continue;
                                }

                                if ((Notes[i].NoteType == NoteType.滑条b_结束 ||
                                     Notes[i].NoteType == NoteType.滑条b_粉键结束) && Notes[i].Time != note.Time)
                                {
                                    str += $"|{(int)(Notes[i].Time * 24)}:{Notes[i].Track - 1}\n";
                                    break;
                                }
                            }
                        }
                        break;

                    case NoteType.长键_开始:
                        {
                            var isFlick = 0;
                            for (var i = index + 1; i < Notes.Count; i++)
                            {
                                if (Notes[i].NoteType == NoteType.长键_结束 && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)
                                {
                                    isFlick = 0;
                                    break;
                                }

                                if (Notes[i].NoteType == NoteType.长键_粉键结束 && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)
                                {
                                    isFlick = 1;
                                    break;
                                }
                            }

                            str += $"l|{isFlick}|{(int)(note.Time * 24)}:{note.Track - 1}";
                            for (var i = index + 1; i < Notes.Count; i++)
                                if ((Notes[i].NoteType == NoteType.长键_结束 ||
                                     Notes[i].NoteType == NoteType.长键_粉键结束) && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)
                                {
                                    str += $"|{(int)(Notes[i].Time * 24)}:{Notes[i].Track - 1}\n";
                                    break;
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

        /// <summary>
        ///     输出为BangCraft谱面工程格式
        /// </summary>
        /// <returns></returns>
        private string ToBangCraftScore()
        {
            var xml = new XmlDocument();
            var Save = xml.CreateElement("Save");
            Save.SetAttribute("name", "BGCdate");
            xml.AppendChild(Save);

            var info = xml.CreateElement("info");
            Save.AppendChild(info);

            var music = xml.CreateElement("music");
            music.InnerText = "test";
            info.AppendChild(music);

            var bpm = xml.CreateElement("bpm");
            bpm.InnerText = Bpm.ToString();
            info.AppendChild(bpm);

            var delay = xml.CreateElement("delay");
            delay.InnerText = Delay_ms.ToString();
            info.AppendChild(delay);

            var bpmP1 = xml.CreateElement("bpmP1");
            bpmP1.InnerText = "";
            info.AppendChild(bpmP1);

            var bpmD1 = xml.CreateElement("bpmD1");
            bpmD1.InnerText = "0";
            info.AppendChild(bpmD1);

            var bpmP2 = xml.CreateElement("bpmP2");
            bpmP2.InnerText = "";
            info.AppendChild(bpmP2);

            var bpmD2 = xml.CreateElement("bpmD2");
            bpmD2.InnerText = "0";
            info.AppendChild(bpmD2);

            var bpmP3 = xml.CreateElement("bpmP3");
            bpmP3.InnerText = "";
            info.AppendChild(bpmP3);

            var bpmD3 = xml.CreateElement("bpmD3");
            bpmD3.InnerText = "0";
            info.AppendChild(bpmD3);

            foreach (var note in Notes)
                //N
                if (note.NoteType == NoteType.白键 || note.NoteType == NoteType.技能)
                {
                    var noteN = xml.CreateElement("noteN");
                    Save.AppendChild(noteN);

                    var lineN = xml.CreateElement("lineN");
                    lineN.InnerText = (note.Track - 4).ToString();
                    noteN.AppendChild(lineN);

                    var posN = xml.CreateElement("posN");
                    posN.InnerText = (note.Time * 2).ToString();
                    noteN.AppendChild(posN);

                    var typeN = xml.CreateElement("typeN");
                    typeN.InnerText = "N";
                    noteN.AppendChild(typeN);
                }

            foreach (var note in Notes)
                //F
                if (note.NoteType == NoteType.粉键)
                {
                    var noteF = xml.CreateElement("noteF");
                    Save.AppendChild(noteF);

                    var lineF = xml.CreateElement("lineF");
                    lineF.InnerText = (note.Track - 4).ToString();
                    noteF.AppendChild(lineF);

                    var posF = xml.CreateElement("posF");
                    posF.InnerText = (note.Time * 2).ToString();
                    noteF.AppendChild(posF);

                    var typeF = xml.CreateElement("typeF");
                    typeF.InnerText = "F";
                    noteF.AppendChild(typeF);
                }

            var index = 0;
            foreach (var note in Notes)
            {
                //LS
                if (note.NoteType == NoteType.滑条a_开始)
                {
                    var noteL = xml.CreateElement("noteL");
                    Save.AppendChild(noteL);

                    var lineL = xml.CreateElement("lineL");
                    lineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(lineL);

                    var posL = xml.CreateElement("posL");
                    posL.InnerText = (note.Time * 2).ToString();
                    noteL.AppendChild(posL);

                    var typeL = xml.CreateElement("typeL");
                    typeL.InnerText = "LS";
                    noteL.AppendChild(typeL);

                    var startlineL = xml.CreateElement("startlineL");
                    startlineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(startlineL);

                    var startposL = xml.CreateElement("startposL");
                    startposL.InnerText = (note.Time * 2).ToString();
                    noteL.AppendChild(startposL);

                    for (var i = index + 1; i < Notes.Count; i++)
                    {
                        if (Notes[i].NoteType == NoteType.滑条a_中间 && Notes[i].Time != note.Time)
                        {
                            var noteM = xml.CreateElement("noteL");
                            Save.AppendChild(noteM);

                            var lineM = xml.CreateElement("lineL");
                            lineM.InnerText = (Notes[i].Track - 4).ToString();
                            noteM.AppendChild(lineM);

                            var posM = xml.CreateElement("posL");
                            posM.InnerText = (Notes[i].Time * 2).ToString();
                            noteM.AppendChild(posM);

                            var typeM = xml.CreateElement("typeL");
                            typeM.InnerText = "LM";
                            noteM.AppendChild(typeM);

                            noteM.AppendChild(startlineL.Clone());

                            noteM.AppendChild(startposL.Clone());
                            continue;
                        }

                        if ((Notes[i].NoteType == NoteType.滑条a_结束 ||
                             Notes[i].NoteType == NoteType.滑条a_粉键结束) && Notes[i].Time != note.Time)
                        {
                            var noteE = xml.CreateElement("noteL");
                            Save.AppendChild(noteE);

                            var lineE = xml.CreateElement("lineL");
                            lineE.InnerText = (Notes[i].Track - 4).ToString();
                            noteE.AppendChild(lineE);

                            var posE = xml.CreateElement("posL");
                            posE.InnerText = (Notes[i].Time * 2).ToString();
                            noteE.AppendChild(posE);

                            var typeE = xml.CreateElement("typeL");
                            typeE.InnerText = Notes[i].NoteType == NoteType.滑条a_结束 ? "LE" : "LF";
                            noteE.AppendChild(typeE);

                            noteE.AppendChild(startlineL.Clone());

                            noteE.AppendChild(startposL.Clone());
                            break;
                        }
                    }
                }

                if (note.NoteType == NoteType.滑条b_开始)
                {
                    var noteL = xml.CreateElement("noteL");
                    Save.AppendChild(noteL);

                    var lineL = xml.CreateElement("lineL");
                    lineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(lineL);

                    var posL = xml.CreateElement("posL");
                    posL.InnerText = (note.Time * 2).ToString();
                    noteL.AppendChild(posL);

                    var typeL = xml.CreateElement("typeL");
                    typeL.InnerText = "LS";
                    noteL.AppendChild(typeL);

                    var startlineL = xml.CreateElement("startlineL");
                    startlineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(startlineL);

                    var startposL = xml.CreateElement("startposL");
                    startposL.InnerText = (note.Time * 2).ToString();
                    noteL.AppendChild(startposL);

                    for (var i = index + 1; i < Notes.Count; i++)
                    {
                        if (Notes[i].NoteType == NoteType.滑条b_中间 && Notes[i].Time != note.Time)
                        {
                            var noteM = xml.CreateElement("noteL");
                            Save.AppendChild(noteM);

                            var lineM = xml.CreateElement("lineL");
                            lineM.InnerText = (Notes[i].Track - 4).ToString();
                            noteM.AppendChild(lineM);

                            var posM = xml.CreateElement("posL");
                            posM.InnerText = (Notes[i].Time * 2).ToString();
                            noteM.AppendChild(posM);

                            var typeM = xml.CreateElement("typeL");
                            typeM.InnerText = "LM";
                            noteM.AppendChild(typeM);

                            noteM.AppendChild(startlineL.Clone());

                            noteM.AppendChild(startposL.Clone());
                            continue;
                        }

                        if ((Notes[i].NoteType == NoteType.滑条b_结束 ||
                             Notes[i].NoteType == NoteType.滑条b_粉键结束) && Notes[i].Time != note.Time)
                        {
                            var noteE = xml.CreateElement("noteL");
                            Save.AppendChild(noteE);

                            var lineE = xml.CreateElement("lineL");
                            lineE.InnerText = (Notes[i].Track - 4).ToString();
                            noteE.AppendChild(lineE);

                            var posE = xml.CreateElement("posL");
                            posE.InnerText = (Notes[i].Time * 2).ToString();
                            noteE.AppendChild(posE);

                            var typeE = xml.CreateElement("typeL");
                            typeE.InnerText = Notes[i].NoteType == NoteType.滑条b_结束 ? "LE" : "LF";
                            noteE.AppendChild(typeE);

                            noteE.AppendChild(startlineL.Clone());

                            noteE.AppendChild(startposL.Clone());
                            break;
                        }
                    }
                }

                if (note.NoteType == NoteType.长键_开始)
                {
                    var noteL = xml.CreateElement("noteL");
                    Save.AppendChild(noteL);

                    var lineL = xml.CreateElement("lineL");
                    lineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(lineL);

                    var posL = xml.CreateElement("posL");
                    posL.InnerText = (note.Time * 2).ToString();
                    noteL.AppendChild(posL);

                    var typeL = xml.CreateElement("typeL");
                    typeL.InnerText = "LS";
                    noteL.AppendChild(typeL);

                    var startlineL = xml.CreateElement("startlineL");
                    startlineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(startlineL);

                    var startposL = xml.CreateElement("startposL");
                    startposL.InnerText = (note.Time * 2).ToString();
                    noteL.AppendChild(startposL);

                    for (var i = index + 1; i < Notes.Count; i++)
                        if ((Notes[i].NoteType == NoteType.长键_结束 ||
                             Notes[i].NoteType == NoteType.长键_粉键结束) && Notes[i].Time != note.Time &&
                            Notes[i].Track == note.Track)
                        {
                            var noteE = xml.CreateElement("noteL");
                            Save.AppendChild(noteE);

                            var lineE = xml.CreateElement("lineL");
                            lineE.InnerText = (Notes[i].Track - 4).ToString();
                            noteE.AppendChild(lineE);

                            var posE = xml.CreateElement("posL");
                            posE.InnerText = (Notes[i].Time * 2).ToString();
                            noteE.AppendChild(posE);

                            var typeE = xml.CreateElement("typeL");
                            typeE.InnerText = Notes[i].NoteType == NoteType.长键_结束 ? "LE" : "LF";
                            noteE.AppendChild(typeE);

                            noteE.AppendChild(startlineL.Clone());

                            noteE.AppendChild(startposL.Clone());
                            break;
                        }
                }

                index++;
            }

            var result = Helper.ConvertXmlDocumentTostring(xml);

            return result;
        }

        /// <summary>
        ///     输出为BMS格式
        /// </summary>
        /// <returns></returns>
        private string ToBMS()
        {
            /*轨道1-7:6123458*/

            /*1通道    白键03 粉键04 技能05 绿条a开始/中间06 绿条a结束07 绿条a粉键结束08 绿条b开始/中间09 绿条b结束0A 绿条b粉键结束0B*/
            /*5通道    长键开始/结束03 长键粉键结束04*/

            var result = "#00001:01" + "\r\n";

            result = result.Insert(0, @"
*---------------------- HEADER FIELD

#PLAYER 1
#GENRE
#TITLE
#ARTIST
" + $"#BPM {Bpm}" + @"
#PLAYLEVEL
#RANK 3

#STAGEFILE

#WAV01 bgm.wav
#WAV03 bd.wav
#WAV04 flick.wav
#WAV05 skill.wav
#WAV06 slide_a.wav
#WAV07 slide_end_a.wav
#WAV08 slide_end_flick_a.wav
#WAV09 slide_b.wav
#WAV0A slide_end_b.wav
#WAV0B slide_end_flick_b.wav
#WAV0C cmd_fever_end.wav
#WAV0D cmd_fever_ready.wav
#WAV0E cmd_fever_start.wav
#WAV0F fever_note.wav
#WAV0G fever_note_flick.wav

#BGM bgm001

*---------------------- MAIN DATA FIELD

");

            var beatCount = 0;

            while (beatCount * 4 < Notes.Last().Time)
            {
                var strList = new List<string>();

                //长键以外
                for (var i = 1; i <= 7; i++)
                {
                    var a = beatCount.ToString().PadLeft(3, '0');
                    var b = "1";
                    var c = "6";
                    switch (i)
                    {
                        case 1:
                            c = "6";
                            break;

                        case 2:
                            c = "1";
                            break;

                        case 3:
                            c = "2";
                            break;

                        case 4:
                            c = "3";
                            break;

                        case 5:
                            c = "4";
                            break;

                        case 6:
                            c = "5";
                            break;

                        case 7:
                            c = "8";
                            break;
                    }

                    var trackNormalNotes = Notes.Where(p =>
                            p.Time >= beatCount * 4 && p.Time < (beatCount + 1) * 4 && p.Track == i &&
                            p.NoteType != NoteType.长键_开始 && p.NoteType != NoteType.长键_结束 &&
                            p.NoteType != NoteType.长键_粉键结束)
                        .OrderBy(p => p.Time)
                        .ToList();
                    if (!trackNormalNotes.Any())
                        continue;
                    var fractions = trackNormalNotes.Select(p => p.Time % 4 / 4).ToList().ConvertToFraction();
                    var d = new string('0', fractions[0].Item2 * 2);
                    for (var j = 0; j < trackNormalNotes.Count(); j++)
                    {
                        d = d.Remove(fractions[j].Item1 * 2, 2);
                        var typeText = "03";
                        switch (trackNormalNotes[j].NoteType)
                        {
                            case NoteType.白键:
                                typeText = "03";
                                break;

                            case NoteType.粉键:
                                typeText = "04";
                                break;

                            case NoteType.技能:
                                typeText = "05";
                                break;

                            case NoteType.滑条a_开始:
                            case NoteType.滑条a_中间:
                                typeText = "06";
                                break;

                            case NoteType.滑条a_结束:
                                typeText = "07";
                                break;

                            case NoteType.滑条a_粉键结束:
                                typeText = "08";
                                break;

                            case NoteType.滑条b_开始:
                            case NoteType.滑条b_中间:
                                typeText = "09";
                                break;

                            case NoteType.滑条b_结束:
                                typeText = "0A";
                                break;

                            case NoteType.滑条b_粉键结束:
                                typeText = "0B";
                                break;

                            case NoteType.长键_开始:
                            case NoteType.长键_结束:
                                typeText = "03";
                                break;

                            case NoteType.长键_粉键结束:
                                typeText = "04";
                                break;
                        }

                        d = d.Insert(fractions[j].Item1 * 2, typeText);
                    }

                    var line = $"#{a}{b}{c}:{d}";

                    strList.Add(line);
                }

                foreach (var str in strList.OrderBy(p => p)) result += str + "\r\n";

                strList = new List<string>();

                //长键
                for (var i = 1; i <= 7; i++)
                {
                    var a = beatCount.ToString().PadLeft(3, '0');
                    var b = "5";
                    var c = "6";
                    switch (i)
                    {
                        case 1:
                            c = "6";
                            break;

                        case 2:
                            c = "1";
                            break;

                        case 3:
                            c = "2";
                            break;

                        case 4:
                            c = "3";
                            break;

                        case 5:
                            c = "4";
                            break;

                        case 6:
                            c = "5";
                            break;

                        case 7:
                            c = "8";
                            break;
                    }

                    var trackNormalNotes = Notes.Where(p =>
                            p.Time >= beatCount * 4 && p.Time < (beatCount + 1) * 4 && p.Track == i && (
                                p.NoteType == NoteType.长键_开始 || p.NoteType == NoteType.长键_结束 ||
                                p.NoteType == NoteType.长键_粉键结束))
                        .OrderBy(p => p.Time)
                        .ToList();
                    if (!trackNormalNotes.Any())
                        continue;

                    var fractions = trackNormalNotes.Select(p => p.Time % 4 / 4).ToList().ConvertToFraction();
                    var d = new string('0', fractions[0].Item2 * 2);
                    for (var j = 0; j < trackNormalNotes.Count(); j++)
                    {
                        d = d.Remove(fractions[j].Item1 * 2, 2);
                        var typeText = "03";
                        switch (trackNormalNotes[j].NoteType)
                        {
                            case NoteType.长键_开始:
                            case NoteType.长键_结束:
                                typeText = "03";
                                break;

                            case NoteType.长键_粉键结束:
                                typeText = "04";
                                break;
                        }

                        d = d.Insert(fractions[j].Item1 * 2, typeText);
                    }

                    var line = $"#{a}{b}{c}:{d}";

                    strList.Add(line);
                }

                foreach (var str in strList.OrderBy(p => p)) result += str + "\r\n";

                result += "\r\n";
                beatCount++;
            }

            return result;
        }

        private string ToBanGround()
        {
            var result = new Score
            {
                difficulty = "Expert",
                level = 29,
                offset = 0,
                notes = new List<note>()
            };

            var list = new List<note>
            {
                new note
                {
                    type = "BPM",
                    beat = new List<int> { 0, 0, 1 },
                    lane = 0,
                    tickStack = -1,
                    value = Bpm,
                    anims = new List<object>()
                }
            };


            var index = 0;
            var lastPosType = PosType.B;
            foreach (var note in Notes)
            {
                var fraction = (note.Time - Math.Floor(note.Time)).ConvertSingleToFraction();
                var beatList = new List<int> 
                {
                    Convert.ToInt32(Math.Floor(note.Time)),
                    fraction.Item1,
                    fraction.Item2
                };

                switch (note.NoteType)
                {
                    case NoteType.白键:
                    case NoteType.技能:
                        list.Add(new note
                        {
                            type = "Single",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = -1,
                            value = 0,
                            anims = new List<object>()
                        });

                        break;

                    case NoteType.粉键:
                        list.Add(new note
                        {
                            type = "Flick",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = -1,
                            value = 0,
                            anims = new List<object>()
                        });
                        break;

                    case NoteType.滑条a_开始:
                        list.Add(new note
                        {
                            type = "Single",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 0,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.A;
                        break;

                    case NoteType.滑条a_中间:
                        list.Add(new note
                        {
                            type = "SlideTick",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 0,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.A;
                        break;

                    case NoteType.滑条a_结束:
                        list.Add(new note
                        {
                            type = "SlideTickEnd",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 0,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.A;
                        break;

                    case NoteType.滑条a_粉键结束:
                        list.Add(new note
                        {
                            type = "Flick",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 0,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.A;
                        break;

                    case NoteType.滑条b_开始:
                        list.Add(new note
                        {
                            type = "Single",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 1,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.B;
                        break;

                    case NoteType.滑条b_中间:
                        list.Add(new note
                        {
                            type = "SlideTick",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 1,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.B;
                        break;

                    case NoteType.滑条b_结束:
                        list.Add(new note
                        {
                            type = "SlideTickEnd",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 1,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.B;
                        break;

                    case NoteType.滑条b_粉键结束:
                        list.Add(new note
                        {
                            type = "Flick",
                            beat = beatList,
                            lane = note.Track - 1,
                            tickStack = 1,
                            value = 0,
                            anims = new List<object>()
                        });
                        lastPosType = PosType.B;
                        break;

                    case NoteType.长键_开始:
                        {
                            var tickStackValue = lastPosType == PosType.A ? 1 : 0;
                            list.Add(new note
                            {
                                type = "Single",
                                beat = beatList,
                                lane = note.Track - 1,
                                tickStack = tickStackValue,
                                value = 0,
                                anims = new List<object>()
                            });
                            lastPosType = tickStackValue == 0 ? PosType.A : PosType.B;

                            for (var i = index + 1; i<Notes.Count; i++)
                            {
                                if (Notes[i].NoteType == NoteType.长键_结束 && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)
                                {
                                    var temp = (Notes[i].Time - Math.Floor(Notes[i].Time)).ConvertSingleToFraction();
                                    beatList = new List<int>
                                    {
                                        Convert.ToInt32(Math.Floor(Notes[i].Time)),
                                        temp.Item1,
                                        temp.Item2
                                    };
                                    list.Add(new note
                                    {
                                        type = "SlideTickEnd",
                                        beat = beatList,
                                        lane = note.Track - 1,
                                        tickStack = tickStackValue,
                                        value = 0,
                                        anims = new List<object>()
                                    });
                                    break;
                                }

                                if (Notes[i].NoteType == NoteType.长键_粉键结束 && Notes[i].Time != note.Time &&
                                    Notes[i].Track == note.Track)
                                {
                                    var temp = (Notes[i].Time - Math.Floor(Notes[i].Time)).ConvertSingleToFraction();
                                    beatList = new List<int>
                                    {
                                        Convert.ToInt32(Math.Floor(Notes[i].Time)),
                                        temp.Item1,
                                        temp.Item2
                                    };
                                    list.Add(new note
                                    {
                                        type = "Flick",
                                        beat = beatList,
                                        lane = note.Track - 1,
                                        tickStack = tickStackValue,
                                        value = 0,
                                        anims = new List<object>()
                                    });
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

            result.notes = list;

            var serializerSettings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
            return JsonConvert.SerializeObject(result);
        }

        #endregion 各谱面格式输出方法
    }
}