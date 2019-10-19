﻿using System.IO;
using System.Text;

namespace BangDreamMusicscoreConverter.Model
{
    public class IOBusiness
    {
        /// <summary>
        /// 返回路径文件中的内容(字符串)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public string GetTextFromPath(string filePath)
        {
            var streamReader = new StreamReader(filePath, Encoding.Default);
            var text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }
    }
}
