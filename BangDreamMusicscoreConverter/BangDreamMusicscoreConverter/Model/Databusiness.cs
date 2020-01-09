using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		private double _delay;

		/// <summary>
		///     构造谱面对象
		/// </summary>
		/// <param name="scoreString">谱面文本</param>
		/// <param name="convertTypeFrom">转换类型</param>
		/// <returns></returns>
		public DefaultScore GetDefaultScore(string scoreString, ConvertTypeFrom convertTypeFrom, string delayString)
		{
			try
			{
				_delay = double.Parse(delayString);
				switch (convertTypeFrom)
				{
					case ConvertTypeFrom.bestdori: return GetDefaultScoreFromBestdoriScore(scoreString);
					case ConvertTypeFrom.bangSimulator: return GetDefaultScoreFromBangSimulatorScore(scoreString);
					case ConvertTypeFrom.bangbangboom: return GetDefaultScoreFromBangbangboomScore(scoreString);
					case ConvertTypeFrom.bandori: return GetDefaultScoreFromBandoriJson(scoreString);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "请确认输入格式是否正确");
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
						Time = time + _delay,
						NoteType = noteType,
						Track = track
					});
				else
					throw new Exception("转换出错，请检查原谱面文本");
			}

			//先按时间排，然后按轨道从左到右排
			defaultScore.Notes = notes.OrderBy(p => p.Time).ThenBy(p => p.Track).ToList();
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
				var tempNote = new Note {Time = note.beat + _delay, Track = note.lane};

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

			//先按时间排，然后按轨道从左到右排
			defaultScore.Notes = notes.OrderBy(p => p.Time).ThenBy(p => p.Track).ToList();
			return defaultScore;
		}

		/// <summary>
		///     从bangbangboom谱面文本构造谱面对象
		/// </summary>
		/// <param name="scoreString">谱面文本</param>
		public DefaultScore GetDefaultScoreFromBangbangboomScore(string scoreString)
		{
			var defaultScore = new DefaultScore();
			var noteArray = scoreString.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			var head = noteArray[0].Split('|');
			defaultScore.Delay_ms = (int) (double.Parse(head[1]) * 1000);
			defaultScore.Bpm = float.Parse(head[2]);

			var notes = new List<Note>();
			var isA = true;
			for (var i = 1; i < noteArray.Length; i++)
			{
				var noteInfo = noteArray[i].Split('|');
				//白键
				if (noteInfo[0] == "s")
				{
					var noteTimeAndTrack = noteInfo[1].Split(':');
					notes.Add(new Note
					{
						NoteType = NoteType.白键,
						Time = double.Parse(noteTimeAndTrack[0]) / 24 + _delay,
						Track = int.Parse(noteTimeAndTrack[1]) + 1
					});
					continue;
				}

				//粉键
				if (noteInfo[0] == "f")
				{
					var noteTimeAndTrack = noteInfo[1].Split(':');
					notes.Add(new Note
					{
						NoteType = NoteType.粉键,
						Time = double.Parse(noteTimeAndTrack[0]) / 24 + _delay,
						Track = int.Parse(noteTimeAndTrack[1]) + 1
					});
					continue;
				}

				//非粉滑条
				if (noteInfo[0] == "l" && noteInfo[1] == "0")
				{
					var startTimeAndTrack = noteInfo[2].Split(':');
					notes.Add(new Note
					{
						NoteType = isA ? NoteType.滑条a_开始 : NoteType.滑条b_开始,
						Time = double.Parse(startTimeAndTrack[0]) / 24 + _delay,
						Track = int.Parse(startTimeAndTrack[1]) + 1
					});
					for (var j = 3; j < noteInfo.Length - 1; j++)
					{
						var noteTimeAndTrack = noteInfo[j].Split(':');
						notes.Add(new Note
						{
							NoteType = isA ? NoteType.滑条a_中间 : NoteType.滑条b_中间,
							Time = double.Parse(noteTimeAndTrack[0]) / 24 + _delay,
							Track = int.Parse(noteTimeAndTrack[1]) + 1
						});
					}

					var endTimeAndTrack = noteInfo[noteInfo.Length - 1].Split(':');
					notes.Add(new Note
					{
						NoteType = isA ? NoteType.滑条a_结束 : NoteType.滑条b_结束,
						Time = double.Parse(endTimeAndTrack[0]) / 24 + _delay,
						Track = int.Parse(endTimeAndTrack[1]) + 1
					});
					isA = !isA;
					continue;
				}

				//粉滑条
				if (noteInfo[0] == "l" && noteInfo[1] == "1")
				{
					var startTimeAndTrack = noteInfo[2].Split(':');
					notes.Add(new Note
					{
						NoteType = isA ? NoteType.滑条a_开始 : NoteType.滑条b_开始,
						Time = double.Parse(startTimeAndTrack[0]) / 24 + _delay,
						Track = int.Parse(startTimeAndTrack[1]) + 1
					});
					for (var j = 3; j < noteInfo.Length - 1; j++)
					{
						var noteTimeAndTrack = noteInfo[j].Split(':');
						notes.Add(new Note
						{
							NoteType = isA ? NoteType.滑条a_中间 : NoteType.滑条b_中间,
							Time = double.Parse(noteTimeAndTrack[0]) / 24 + _delay,
							Track = int.Parse(noteTimeAndTrack[1]) + 1
						});
					}

					var endTimeAndTrack = noteInfo[noteInfo.Length - 1].Split(':');
					notes.Add(new Note
					{
						NoteType = isA ? NoteType.滑条a_粉键结束 : NoteType.滑条b_粉键结束,
						Time = double.Parse(endTimeAndTrack[0]) / 24 + _delay,
						Track = int.Parse(endTimeAndTrack[1]) + 1
					});
					isA = !isA;
				}
			}

			//先按时间排，然后按轨道从左到右排
			defaultScore.Notes = notes.OrderBy(p => p.Time).ThenBy(p => p.Track).ToList();
			return defaultScore;
		}

		/// <summary>
		///     从bandori database谱面json构造谱面对象
		/// </summary>
		/// <param name="scoreString">谱面文本</param>
		public DefaultScore GetDefaultScoreFromBandoriJson(string scoreString)
		{
			var defaultScore = new DefaultScore();
			var score = JsonConvert.DeserializeObject<dynamic>(scoreString);

			defaultScore.Bpm = score.metadata.bpm;
			defaultScore.Delay_ms = 0;
			var noteList = new List<Note>();

			foreach (var note in score.objects)
			{
				if (note.type == "System")
					continue;
				var defaultNote = new Note
				{
					Time = note.beat + _delay,
					Track = note.lane
				};

				if ((note.effect == "Single" || note.effect == "FeverSingle") && note.property == "Single")
				{
					defaultNote.NoteType = NoteType.白键;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "Skill" && note.property == "Single")
				{
					defaultNote.NoteType = NoteType.技能;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "Flick" && note.property == "Single")
				{
					defaultNote.NoteType = NoteType.粉键;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "SlideStart_A" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条a_开始;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "Slide_A" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条a_中间;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "SlideEnd_A" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条a_结束;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "SlideEndFlick_A" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条a_粉键结束;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "SlideStart_B" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条b_开始;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "Slide_B" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条b_中间;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "SlideEnd_B" && note.property == "Slide")
				{
					defaultNote.NoteType = NoteType.滑条b_结束;
					noteList.Add(defaultNote);
					continue;
				}

				if ((note.effect == "Single" || note.effect == "Skill" || note.effect == "FeverSingle") &&
				    note.property == "LongStart")
				{
					defaultNote.NoteType = NoteType.长键_开始;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "Single" && note.property == "LongEnd")
				{
					defaultNote.NoteType = NoteType.长键_结束;
					noteList.Add(defaultNote);
					continue;
				}

				if (note.effect == "Flick" && note.property == "LongEnd")
				{
					defaultNote.NoteType = NoteType.长键_粉键结束;
					noteList.Add(defaultNote);
				}
			}

			defaultScore.Notes = noteList;

			return defaultScore;
		}
	}
}