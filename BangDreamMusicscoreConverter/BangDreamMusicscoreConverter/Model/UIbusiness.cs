using System.Windows.Controls;
using Microsoft.Win32;

namespace BangDreamMusicscoreConverter.Model
{
    public class UIBusiness
    {
        /// <summary>
        ///     <para>打开文件选择窗口，返回所选文件路径</para>
        ///     <para>仅显示.txt文件</para>
        /// </summary>
        /// <returns></returns>
        public string OpenFileDialogWindow()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "文本文件(*.txt)|*.txt"
            };
            if (openFileDialog.ShowDialog() == true) return openFileDialog.FileName;
            return "";
        }

        /// <summary>
        ///     打开文件保存窗口，返回保存文件的路径
        /// </summary>
        /// <returns></returns>
        public string SaveFileDialogWindow()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "文本文件(*.txt)|*.txt",
                FileName = "mysorce"
            };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : "";
        }

        /// <summary>
        ///     从文本框获取文本
        /// </summary>
        /// <param name="textBox">目标文本框</param>
        /// <returns></returns>
        public string GetText(TextBox textBox)
        {
            return textBox.Text;
        }

        /// <summary>
        ///     显示文本到文本框
        /// </summary>
        /// <param name="textBox">目标文本框</param>
        /// <param name="text">文本内容</param>
        public void ShowText(TextBox textBox, string text)
        {
            textBox.Text = text;
        }
    }
}