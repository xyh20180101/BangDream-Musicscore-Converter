using System.Collections.Generic;
using System.Windows.Documents;

namespace BangDreamMusicscoreConverter.DataClass.BangCraft
{
	public class BangCraftRoot
	{
		public string name { get; set; }

		public BangCraftInfo info { get; set; }

		public List<BangCraftNote> noteN { get; set; }

		public List<BangCraftNote> noteF { get; set; }

		public List<BangCraftNote> noteL { get; set; }
	}
}