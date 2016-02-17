using System;
using System.IO;
using System.Linq;
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
			IsDatasetDirty = false;
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

		bool IsDatasetDirty { get; set; }
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
				if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile))
				{
					dataSet = TrainedDataSet.DeserializeFromXml(selectedFile);
					if (dataSet != null)
					{
						OnDataSetLoaded();
					}
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
				if (IsDatasetDirty)
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
			}
			return true;
		}

		private void OnDataSetLoaded()
		{
			btnTrain.Visible = true;
			labelTotalWords.Visible = true;
			labelUniqueWords.Visible = true;
			IsDatasetDirty = false;
			UpdateLabels();
		}

		private void SaveDataSet()
		{
			string selectedFile = ShowFileDialog(saveFileDialog);
			if (!string.IsNullOrWhiteSpace(selectedFile))
			{
				if (TrainedDataSet.SerializeToXml(dataSet, selectedFile))
				{
					IsDatasetDirty = false;
				}
			}
		}

		private void TrainDataSet()
		{
			string selectedFile = ShowFileDialog(openFileDialog);
			if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile))
			{
				dataSet.Train(new FileInfo(selectedFile));

				IsDatasetDirty = true;
				UpdateLabels();
			}
		}

		private void UpdateLabels()
		{
			labelTotalWords.Text = string.Format("{0} Total Words", dataSet.TotalWordsProcessed);
			labelUniqueWords.Text = string.Format("{0} Unique Words", dataSet.UniqueWordsCataloged);
		}

		private string ShowFileDialog(FileDialog dialog)
		{
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{ 
				return dialog.FileName;
			}
			else
			{ 
				return string.Empty; 
			}
		}
	}
}
