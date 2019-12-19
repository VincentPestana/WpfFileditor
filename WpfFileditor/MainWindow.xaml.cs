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

		private void btnLoadFile_Click(object sender, RoutedEventArgs e)
		{
			txtMain.Text = "";
			// Read file
			using var sr = new StreamReader(@"C:\Users\Neo\Documents\Visual Studio 2017\Projects\WpfFileditor\WpfFileditor\test.txt");
			var nextLine = sr.ReadLine();
			do
			{
				txtMain.AppendText(nextLine);
				nextLine = sr.ReadLine();
			} while (nextLine != null);

		}
	}
}
