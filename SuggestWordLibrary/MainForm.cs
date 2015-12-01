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
using WordPredictionLibrary;

namespace SuggestWordLibrary
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		TrainedDataSet dataSet { get; set; }

		private void btnOpen_Click(object sender, EventArgs e)
		{
			string selectedFile = OpenFileDialog();
			if (File.Exists(selectedFile))
			{
				dataSet = new TrainedDataSet(selectedFile);
			}
		}

		private string OpenFileDialog()
		{
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				return openFileDialog.FileName;
			else
				return string.Empty;
		}

		
		private void ShowInfo(string format, params object[] args)
		{
			MessageBox.Show(string.Format(format, args), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ShowError(string format, params object[] args)
		{
			MessageBox.Show(string.Format(format, args), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		
		private void btnTrain_Click(object sender, EventArgs e)
		{
			string selectedFile = OpenFileDialog();
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
	}
}
