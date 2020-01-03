using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfFileditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// TODO: Convert to config file
		private readonly string TitleBar = "WPF Based File Editor";

		private string _fileName;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void ExitApplication()
		{
			Application.Current.Shutdown();
		}

		private void ClearMainTextBox()
		{
			txtMain.Text = "";
			winMain.Title = TitleBar;
			_fileName = "";
			MenuSave.IsEnabled = false;
		}

		private void LoadFile()
		{
			txtMain.Text = "";

			// Select a file from disk
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			var dialogResult = openFileDialog.ShowDialog();
			if (dialogResult == true)
			{
				_fileName = openFileDialog.FileName;

				winMain.Title = _fileName + "  - " + TitleBar;

				// Read file
				using var streamReader = new StreamReader(_fileName);
				var textFromFile = streamReader.ReadToEnd();
				txtMain.AppendText(textFromFile);

				MenuSave.IsEnabled = true;
			}
			else
			{
				// No file chosen
			}
		}

		private void SaveFile()
		{
			// TODO: Implement "Save As"

			// TODO : This should call the SaveAs function as no file was opened
			if (string.IsNullOrEmpty(_fileName))
				return;

			if (!File.Exists(_fileName))
			{
				MessageBox.Show("Filename does not exist - " + _fileName, "Filename does not exist");
				return;
			}

			File.WriteAllText(_fileName, txtMain.Text);
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			LoadFile();
		}

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			ClearMainTextBox();
		}

		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			ExitApplication();
		}

		private void MenuSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFile();
		}
	}
}
