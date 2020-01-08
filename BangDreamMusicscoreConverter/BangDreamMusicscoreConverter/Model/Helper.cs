using System.IO;
using System.Text;
using System.Xml;

namespace BangDreamMusicscoreConverter.Model
{
	public static class Helper
	{
		public static string ConvertXmlDocumentTostring(XmlDocument xmlDocument)
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlWriter writer = XmlWriter.Create(memoryStream, new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true,
				Encoding = Encoding.UTF8
			});
			xmlDocument.Save(writer);
			StreamReader streamReader = new StreamReader(memoryStream);
			memoryStream.Position = 0;
			string xmlString = streamReader.ReadToEnd();
			streamReader.Close();
			memoryStream.Close();
			return xmlString;
		}
    }
}