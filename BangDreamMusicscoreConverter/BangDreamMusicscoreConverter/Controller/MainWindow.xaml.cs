using System;
using System.Windows;
using BangDreamMusicscoreConverter.DataClass;
using BangDreamMusicscoreConverter.Model;

namespace BangDreamMusicscoreConverter
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataBusiness dataBusiness = new DataBusiness();
        private readonly IOBusiness ioBusiness = new IOBusiness();
        private readonly UIBusiness uiBusiness = new UIBusiness();

        /// <summary>
        ///     初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     打开按钮_点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = uiBusiness.OpenFileDialogWindow();
            string musicScore;
            try
            {
                musicScore = ioBusiness.GetTextFromPath(filePath);
            }
            catch (Exception)
            {
                return;
            }

            uiBusiness.ShowText(SourceTextBox, musicScore);
        }

        /// <summary>
        ///     原谱面文本框_拖拽悬浮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceTextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            //只允许拖拽单个文件
            if (((string[]) e.Data.GetData(DataFormats.FileDrop)).Length != 1)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        /// <summary>
        ///     原谱面文本框_拖拽结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceTextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            string musicScore;
            try
            {
                var filePath = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];
                musicScore = ioBusiness.GetTextFromPath(filePath);
            }
            catch (Exception)
            {
                return;
            }

            uiBusiness.ShowText(SourceTextBox, musicScore);
        }

        /// <summary>
        ///     转换按钮_点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            var scoreString = uiBusiness.GetText(SourceTextBox);
            var defaultScore =
                dataBusiness.GetDefaultScore(scoreString, (ConvertTypeFrom) ConvertTypeFromSelector.SelectedIndex);
            uiBusiness.ShowText(ResultTextBox,
                defaultScore.ToString((ConvertTypeTo) ConvertTypeToSelector.SelectedIndex));
        }

        /// <summary>
        ///     复制按钮_点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(uiBusiness.GetText(ResultTextBox));
        }

        /// <summary>
        ///     保存按钮_点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = uiBusiness.SaveFileDialogWindow();
            ioBusiness.SaveTextToPath(filePath, uiBusiness.GetText(ResultTextBox));
        }
    }
}