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
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			LoadFile();
		}

		private void LoadFile()
		{
			txtMain.Text = "";

			// Select a file from disk
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			var dialogResult = openFileDialog.ShowDialog();
			if (dialogResult == true)
			{
				var filename = openFileDialog.FileName;

				// Read file
				using var streamReader = new StreamReader(filename);
				var textFromFile = streamReader.ReadToEnd();
				txtMain.AppendText(textFromFile);
			}
			else
			{
				// No file chosen
			}
		}
	}
}
