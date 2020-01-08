using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using BangDreamMusicscoreConverter.DataClass.Bestdori;
using BangDreamMusicscoreConverter.Model;
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
                    case ConvertTypeTo.bangCraft: return ToBangCraftScore();
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

        /// <summary>
        /// 输出为BangCraft谱面工程格式
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
	        {
                //N
		        if (note.NoteType == NoteType.白键|| note.NoteType == NoteType.技能)
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
	        }

	        foreach (var note in Notes)
	        {
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
			        startposL.InnerText = ((note.Time * 2)).ToString();
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
					        posM.InnerText = ((Notes[i].Time * 2)).ToString();
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
					        posE.InnerText = ((Notes[i].Time * 2)).ToString();
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
                    posL.InnerText = ((note.Time * 2)).ToString();
                    noteL.AppendChild(posL);

                    var typeL = xml.CreateElement("typeL");
                    typeL.InnerText = "LS";
                    noteL.AppendChild(typeL);

                    var startlineL = xml.CreateElement("startlineL");
                    startlineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(startlineL);

                    var startposL = xml.CreateElement("startposL");
                    startposL.InnerText = ((note.Time * 2)).ToString();
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
                            posM.InnerText = ((Notes[i].Time * 2)).ToString();
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
                            posE.InnerText = ((Notes[i].Time * 2)).ToString();
                            noteE.AppendChild(posE);

                            var typeE = xml.CreateElement("typeL");
                            typeE.InnerText = (Notes[i].NoteType == NoteType.滑条b_结束) ? "LE" : "LF";
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
                    posL.InnerText = ((note.Time * 2)).ToString();
                    noteL.AppendChild(posL);

                    var typeL = xml.CreateElement("typeL");
                    typeL.InnerText = "LS";
                    noteL.AppendChild(typeL);

                    var startlineL = xml.CreateElement("startlineL");
                    startlineL.InnerText = (note.Track - 4).ToString();
                    noteL.AppendChild(startlineL);

                    var startposL = xml.CreateElement("startposL");
                    startposL.InnerText = ((note.Time * 2)).ToString();
                    noteL.AppendChild(startposL);

                    for (var i = index + 1; i < Notes.Count; i++)
                    {
	                    if ((Notes[i].NoteType == NoteType.长键_结束 ||
                             Notes[i].NoteType == NoteType.长键_粉键结束) && Notes[i].Time != note.Time && Notes[i].Track==note.Track)
                        {
                            var noteE = xml.CreateElement("noteL");
                            Save.AppendChild(noteE);

                            var lineE = xml.CreateElement("lineL");
                            lineE.InnerText = (Notes[i].Track - 4).ToString();
                            noteE.AppendChild(lineE);

                            var posE = xml.CreateElement("posL");
                            posE.InnerText = ((Notes[i].Time * 2)).ToString();
                            noteE.AppendChild(posE);

                            var typeE = xml.CreateElement("typeL");
                            typeE.InnerText = (Notes[i].NoteType == NoteType.长键_结束) ? "LE" : "LF";
                            noteE.AppendChild(typeE);

                            noteE.AppendChild(startlineL.Clone());

                            noteE.AppendChild(startposL.Clone());
                            break;
                        }
                    }
                }

                index++;
	        }

            var result = Helper.ConvertXmlDocumentTostring(xml);

            return result;
        }

        #endregion
    }
}