using Microsoft.Win32;
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

		private void StartApplication()
		{
			ResetInterface();
		}

		private void ExitApplication()
		{
			Application.Current.Shutdown();
		}

		/// <summary>
		/// Resets variables used for UI
		/// </summary>
		private void ResetInterface()
		{
			txtMain.Text = "";
			winMain.Title = TitleBar;
			_fileName = "";
			MenuSave.IsEnabled = false;
			MenuClose.IsEnabled = false;
			lblEncoding.Content = "";
		}

		/// <summary>
		/// Open a OS dialog to select a file
		/// </summary>
		private void LoadFile()
		{
			txtMain.Text = "";

			// Select a file from disk
			OpenFileDialog openFileDialog = new OpenFileDialog();
			var dialogResult = openFileDialog.ShowDialog();
			if (dialogResult == true)
			{
				_fileName = openFileDialog.FileName;

				// Read file
				using var streamReader = new StreamReader(_fileName);
				var textFromFile = streamReader.ReadToEnd();
				txtMain.Text = textFromFile;

				winMain.Title = _fileName + "  - " + TitleBar;
				MenuSave.IsEnabled = true;
				MenuClose.IsEnabled = true;
			}
			else
			{
				// No file chosen
			}
			
			SetEncodingView();
		}

		private void ReloadCurrentFile()
		{
			if (string.IsNullOrEmpty(_fileName))
				return;

			using var streamReader = new StreamReader(_fileName);
			var textFromFile = streamReader.ReadToEnd();
			txtMain.Text = textFromFile;
		}

		/// <summary>
		/// Open a OS dialog to save the file
		/// </summary>
		private void SaveFile()
		{
			// No filename specified, do nothing
			if (string.IsNullOrEmpty(_fileName))
				return;

			if (!File.Exists(_fileName))
			{
				MessageBox.Show("Filename does not exist - " + _fileName, "Filename does not exist");
				return;
			}

			File.WriteAllText(_fileName, txtMain.Text);

			SetEncodingView();
		}

		private void SaveFileAs()
		{
			// TODO Should we allow this if there is no text entered
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Save File As";
			saveFileDialog.ShowDialog();
			var saveFileAsName = saveFileDialog.FileName;

			// Above save dialog deals with file overwriting
			if (File.Exists(saveFileAsName))
			{

			}

			File.WriteAllText(saveFileAsName, txtMain.Text);

			MenuSave.IsEnabled = true;
			MenuClose.IsEnabled = true;
			_fileName = saveFileAsName;
			winMain.Title = saveFileAsName + " - " + TitleBar;
		}

		private void CutSelectedText()
		{
			CopySelectedText();

			// Remove copied text
			DeleteSelectedText();
		}

		private void CopySelectedText()
		{
			var selectedText = txtMain.SelectedText;
			Clipboard.SetText(selectedText);
		}

		private void PasteClipboardText()
		{
			var caretIndex = txtMain.CaretIndex;
			txtMain.Text = txtMain.Text.Substring(0, caretIndex) + Clipboard.GetText() + txtMain.Text.Substring(caretIndex);

			if (caretIndex == 0)
				// Main text was empty on paste
				txtMain.CaretIndex = txtMain.Text.Length;
			else
				txtMain.CaretIndex = caretIndex + 1;
		}

		private void DeleteSelectedText()
		{
			txtMain.Text = txtMain.Text.Substring(0, txtMain.SelectionStart) + txtMain.Text.Substring(txtMain.CaretIndex + txtMain.SelectionLength);
		}

		/// <summary>
		/// Uses the TextBox built in Undo
		/// </summary>
		private void UndoText()
		{
			txtMain.Undo();
		}

		/// <summary>
		/// Uses the TextBox built in Redo
		/// </summary>
		private void RedoText()
		{
			txtMain.Redo();
		}

		private void SelectAllText()
		{
			txtMain.SelectAll();
		}

		private void MenuFileInitialize()
		{
			throw new NotImplementedException();
		}

		private void MenuEditInitialize()
		{
			// Enable/disable paste
			MenuEditPaste.IsEnabled = Clipboard.ContainsText();
			
			// Check if there is selected text, disable copy and cut
			MenuEditCopy.IsEnabled = txtMain.SelectedText.Length > 0;
			MenuEditCut.IsEnabled = txtMain.SelectedText.Length > 0;

			// Undo / Redo
			MenuEditUndo.IsEnabled = txtMain.CanUndo;
			MenuEditRedo.IsEnabled = txtMain.CanRedo;
		}

		private void TextToUppercase()
		{
			// Make the selected text uppercase
			if (txtMain.SelectionLength > 0)
			{
				txtMain.Text = txtMain.Text.Substring(0, txtMain.SelectionStart) + txtMain.SelectedText.ToUpper() + txtMain.Text.Substring(txtMain.CaretIndex + txtMain.SelectionLength);
			}
			else
			{
				// Make the line uppercase	
			}
		}

		private void TextToLowercase()
		{
			if (txtMain.SelectionLength > 0)
			{
				txtMain.Text = txtMain.Text.Substring(0, txtMain.SelectionStart) + txtMain.SelectedText.ToLower() + txtMain.Text.Substring(txtMain.CaretIndex + txtMain.SelectionLength);
			}
		}

		private void SetEncodingView()
		{
			lblEncoding.Content = FileHelper.GetEncoding(_fileName).HeaderName;
		}

		#region Form events
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			LoadFile();
		}

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			ResetInterface();
		}

		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			ExitApplication();
		}

		private void MenuSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFile();
		}

		private void winMain_Loaded(object sender, RoutedEventArgs e)
		{
			StartApplication();
		}

		private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileAs();
		}

		private void MenuEditCopy_Click(object sender, RoutedEventArgs e)
		{
			CopySelectedText();
		}

		private void MenuEditPaste_Click(object sender, RoutedEventArgs e)
		{
			PasteClipboardText();
		}

		private void MenuEditUndo_Click(object sender, RoutedEventArgs e)
		{
			UndoText();
		}

		private void MenuEditRedo_Click(object sender, RoutedEventArgs e)
		{
			RedoText();
		}

		private void MenuEditCut_Click(object sender, RoutedEventArgs e)
		{
			CutSelectedText();
		}
		
		private void MenuFile_Click(object sender, RoutedEventArgs e)
		{
			//MenuFileInitialize();
		}

		private void MenuEdit_Click(object sender, RoutedEventArgs e)
		{
			//MenuEditInitialize();
		}

		private void MenuEditDelete_Click(object sender, RoutedEventArgs e)
		{
			DeleteSelectedText();
		}

		private void MenuEditSelectAll_Click(object sender, RoutedEventArgs e)
		{
			SelectAllText();
		}

		private void MenuFileReloadFile_Click(object sender, RoutedEventArgs e)
		{
			ReloadCurrentFile();
		}

		private void MenuOpUppercase_Click(object sender, RoutedEventArgs e)
		{
			TextToUppercase();
		}

		private void MenuOpLowercase_Click(object sender, RoutedEventArgs e)
		{
			TextToLowercase();
		}
		#endregion
	}
}
