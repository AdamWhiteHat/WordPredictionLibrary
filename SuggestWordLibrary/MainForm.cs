using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordPredictionLibrary;

namespace SuggestWordLibrary
{
	public partial class MainForm : Form
	{
		#region Winform Event Handlers

		public MainForm()
		{
			InitializeComponent();
			dataSet = new TrainedDataSet();
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			NewDataSet();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveDataSet();
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			OpenDataSet();
		}

		private void btnTrain_Click(object sender, EventArgs e)
		{
			TrainDataSet();
		}

		private void ShowInfo(string format, params object[] args)
		{
			MessageBox.Show(string.Format(format, args), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ShowError(string format, params object[] args)
		{
			MessageBox.Show(string.Format(format, args), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		#endregion

		TrainedDataSet dataSet { get; set; }

		private void NewDataSet()
		{
			if (AskIfSaveFirst())
			{
				dataSet = new TrainedDataSet();
				OnDataSetLoaded();
			}
		}

		private void OpenDataSet()
		{
			if (AskIfSaveFirst())
			{
				string selectedFile = ShowFileDialog(openFileDialog);
				if (File.Exists(selectedFile))
				{
					dataSet = new TrainedDataSet(selectedFile);
					OnDataSetLoaded();
				}
			}
		}

		/// <summary>
		/// Asks to save the work. Returns false if Cancel was pressed.
		/// </summary>		
		private bool AskIfSaveFirst()
		{
			if (dataSet != null && dataSet.TotalWordsProcessed > 1)
			{
				DialogResult result = MessageBox.Show("Do you wish to save current Trained Data Set?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				switch (result)
				{
					case DialogResult.Yes:
						SaveDataSet();
						break;

					case DialogResult.No:
						break;

					case DialogResult.Cancel:
					default:
						return false;
				}
			}
			return true;
		}

		private void OnDataSetLoaded()
		{
			btnTrain.Visible = true;
			labelTotalWords.Visible = true;
			labelUniqueWords.Visible = true;
		}

		private void SaveDataSet()
		{
			string selectedFile = ShowFileDialog(saveFileDialog);
			if (!string.IsNullOrWhiteSpace(selectedFile))
			{
				dataSet.SerializeToFile(selectedFile);
			}
		}

		private void TrainDataSet()
		{
			string selectedFile = ShowFileDialog(openFileDialog);
			if (!File.Exists(selectedFile))
			{
				ShowError("File does not exist: \"{0}\"", selectedFile);
				return;
			}
			else
			{
				dataSet.Train(new FileInfo(selectedFile));

				ShowInfo("TotalWordsProcessed (NextWordDictionary.Count): {0}", dataSet.TotalWordsProcessed);
				ShowInfo("UniqueWordsCataloged: \"{0}\"", dataSet.UniqueWordsCataloged);
			}
		}
		
		private string ShowFileDialog(FileDialog dialog)
		{
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				return dialog.FileName;
			else
				return string.Empty;
		}
	}
}
