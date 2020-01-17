using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace BangDreamMusicscoreConverter.Model
{
	public static class Helper
	{
		public static string ConvertXmlDocumentTostring(XmlDocument xmlDocument)
		{
			var memoryStream = new MemoryStream();
			var writer = XmlWriter.Create(memoryStream, new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true,
				Encoding = Encoding.UTF8
			});
			xmlDocument.Save(writer);
			var streamReader = new StreamReader(memoryStream);
			memoryStream.Position = 0;
			var xmlString = streamReader.ReadToEnd();
			streamReader.Close();
			memoryStream.Close();
			return xmlString;
		}

		public static List<(int,int)> ConvertToFraction(this List<double> numList,int denominator = 960)
		{
			var intList = numList.Select(num => Convert.ToInt32(num * 960)).OrderBy(p => p).ToList();
			var factorList = new List<int>();
			var not0min = 1;
			try
			{
				not0min = intList.First(p => p != 0);
			}
			catch
			{
				return new List<(int, int)> {(0, 1)};
			}

			for (var i = not0min ; i > 0; i--)
			{
				if (not0min % i == 0)
				{
					factorList.Add(i);
				}
			}

			var maxFactor = 1;
			foreach (var factor in factorList)
			{
				if(denominator%factor!=0)
					continue;

				var b = intList.All(num => num % factor == 0);

				if (b)
				{
					maxFactor = factor;
					break;
				}
			}

			return intList.Select(num => (num / maxFactor, denominator / maxFactor)).ToList();
		}
	}
}