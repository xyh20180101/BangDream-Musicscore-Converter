using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GetScoreFromBestdori
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string Url = "https://bestdori.com/api/post/details?id=";
		private const string Url2 = "https://api.bandori.ga/v1/jp/music/chart/";
		private readonly HttpClient _httpClient;

		public MainWindow()
		{
			InitializeComponent();
			_httpClient = new HttpClient();
			UrlLabel.Content = Url;
		}

		/// <summary>
		///     获取按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetScoreButton_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                var response = _httpClient.GetAsync(Url + IdTextBox.Text).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var jObject = JsonConvert.DeserializeObject<dynamic>(result);
                ResultTextBox.Text = jObject.post.notes.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

		/// <summary>
		///     获取按钮2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetScoreButton2_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                var a = LevelComboBox.Text;
                var response = _httpClient.GetAsync($"{Url2}{IdTextBox2.Text}/{a}")
                    .Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var jObject = JsonConvert.DeserializeObject<dynamic>(result);
                ResultTextBox.Text = jObject.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
		}

		/// <summary>
		///     清空按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			ResultTextBox.Text = "";
		}

		/// <summary>
		///     复制按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CopyButton_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(ResultTextBox.Text);
		}

		/// <summary>
		///     保存按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var saveFileDialog = new SaveFileDialog
				{
					FileName = "bestdori" + IdTextBox.Text,
					DefaultExt = "txt",
					Filter = "文本文件(*.txt)|*.txt|所有文件|*.*",
					AddExtension = true,
					InitialDirectory = Environment.CurrentDirectory
				};
				if (saveFileDialog.ShowDialog() == true)
				{
					using var streamWriter = new StreamWriter(saveFileDialog.FileName);
					streamWriter.Write(ResultTextBox.Text);
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
			}
		}
	}
}