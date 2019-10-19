using Microsoft.Win32;
using System.Windows.Controls;

namespace BangDreamMusicscoreConverter.Model
{
    public class UIBusiness
    {
        /// <summary>
        /// <para>打开文件选择窗口，返回所选文件路径</para>
        /// <para>仅显示.txt文件</para>
        /// </summary>
        /// <returns></returns>
        public string OpenFileDialogWindow()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "文本文件(*.txt)|*.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }

        /// <summary>
        /// 显示文本到文本框
        /// </summary>
        /// <param name="textbox">目标文本框</param>
        /// <param name="text">文本内容</param>
        public void ShowText(TextBox textbox,string text)
        {
            textbox.Text = text;
        }
    }
}
