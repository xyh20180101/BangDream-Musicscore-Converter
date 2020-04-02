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
		private readonly DataBusiness _dataBusiness = new DataBusiness();
		private readonly IOBusiness _ioBusiness = new IOBusiness();
		private readonly UIBusiness _uiBusiness = new UIBusiness();

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
            try
            {
				var filePath = _uiBusiness.OpenFileDialogWindow();
			    string musicScore;
                musicScore = _ioBusiness.GetTextFromPath(filePath);
                _uiBusiness.ShowText(SourceTextBox, musicScore);
			}
			catch (Exception ex)
			{
				return;
			}
		}

		/// <summary>
		///     原谱面文本框_拖拽悬浮事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SourceTextBox_PreviewDragOver(object sender, DragEventArgs e)
		{
            try
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
            catch (Exception ex)
            {
				return;
            }
        }

		/// <summary>
		///     原谱面文本框_拖拽结束事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SourceTextBox_PreviewDrop(object sender, DragEventArgs e)
		{
            try
            {
				string musicScore;
				var filePath = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];
				musicScore = _ioBusiness.GetTextFromPath(filePath);
                _uiBusiness.ShowText(SourceTextBox, musicScore);
			}
			catch (Exception ex)
			{
				return;
			}
		}

		/// <summary>
		///     转换按钮_点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ConvertButton_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                var scoreString = _uiBusiness.GetText(SourceTextBox);
                var delayString = _uiBusiness.GetText(DelayTextBox);
                var defaultScore =
                    _dataBusiness.GetDefaultScore(scoreString, (ConvertTypeFrom) ConvertTypeFromSelector.SelectedIndex,
                        delayString);
                if (CheckRepeatCheckBox.IsChecked == true)
                    _dataBusiness.CheckRepeat(defaultScore);
                _dataBusiness.GenLongNote(defaultScore);
                _dataBusiness.FixSamePosSlide(defaultScore);
                _uiBusiness.ShowText(ResultTextBox,
                    defaultScore.ToString((ConvertTypeTo) ConvertTypeToSelector.SelectedIndex));
            }
            catch (Exception ex)
            {
				return;
            }
        }

		/// <summary>
		///     复制按钮_点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CopyButton_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                Clipboard.SetText(_uiBusiness.GetText(ResultTextBox));
            }
            catch(Exception ex)
            {
				return;
            }
        }

		/// <summary>
		///     保存按钮_点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			var filePath = _uiBusiness.SaveFileDialogWindow();
			_ioBusiness.SaveTextToPath(filePath, _uiBusiness.GetText(ResultTextBox));
		}

		/// <summary>
		///     清除按钮_点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			_uiBusiness.ShowText(SourceTextBox, "");
		}
	}
}