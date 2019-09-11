using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using WordPredictionLibrary.Core;
using WordPredictionLibrary.Forensics;
using Microsoft.Msagl;
using Microsoft.Msagl.Core;
using Microsoft.Msagl.Layout;
using Microsoft.Msagl.Drawing;

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

		private void tbOutput_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A) // CTRL + A, Select all
			{
				tbOutput.SelectAll();
			}
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
			if (dataSet != null && dataSet.TotalSampleSize > 1)
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
			labelTotalWords.Text = string.Format("{0} Total Words", dataSet.TotalSampleSize);
			labelUniqueWords.Text = string.Format("{0} Unique Words", dataSet.UniqueWordCount);
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

		private void btnSuggest_Click(object sender, EventArgs e)
		{
			string lastWord = extractLastWord(tbOutput.Text);
			string suggestedWord = dataSet.SuggestNext(lastWord);

			tbOutput.AppendText(string.Concat(" ", suggestedWord));
		}

		private string extractLastWord(string text)
		{
			string input = text.TrimEnd(' ', '\t', '\n');
			if (!string.IsNullOrWhiteSpace(input))
			{
				int indexOfLastWord = input.LastIndexOf(' ');
				if (indexOfLastWord == -1)
				{
					indexOfLastWord = 0;
				}
				string lastWord = tbOutput.Text.Substring(indexOfLastWord).Trim();
				return lastWord;
			}
			return string.Empty;
		}

		private void btnForensics_Click(object sender, EventArgs e)
		{
			string selectedFile = ShowFileDialog(openFileDialog);
			if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile))
			{
				Stylometry forensics = new Stylometry(dataSet);

				List<decimal> seqProbabilty = forensics.GetProbabilitySequence(selectedFile);
				List<decimal> seqFrequency = forensics.GetFrequencySequence(selectedFile);
				List<decimal> seqPopularity = forensics.GetPopularitySequence(selectedFile);

				tbOutput.Text += string.Format("Sequence Probability Mean: {0} ({1})", forensics.GetSequenceMean(seqProbabilty), seqProbabilty.Sum()) + Environment.NewLine;
				tbOutput.Text += string.Format("Sequence Popularity Mean: {0} ({1})", forensics.GetSequenceMean(seqPopularity), seqPopularity.Sum()) + Environment.NewLine;
				tbOutput.Text += string.Format("Sequence Frequency Mean: {0} ({1})", forensics.GetSequenceMean(seqFrequency), seqFrequency.Sum()) + Environment.NewLine;
				tbOutput.Text += "-------------------------------" + Environment.NewLine;
				tbOutput.Text += "UniqueWordCount: " + forensics.GetUniqueWordCount() + Environment.NewLine;
				tbOutput.Text += "TotalSampleSize: " + forensics.GetTotalSampleSize() + Environment.NewLine;
				tbOutput.Text += "Variance: " + forensics.GetVariance() + Environment.NewLine;
				tbOutput.Text += "StandardDeviation: " + forensics.GetStandardDeviation() + Environment.NewLine;
				tbOutput.Text += "-------------------------------" + Environment.NewLine + Environment.NewLine;
				tbOutput.Text += Path.GetFileNameWithoutExtension(selectedFile).ToUpperInvariant() + Environment.NewLine;
				tbOutput.Text += string.Join(Environment.NewLine, seqPopularity.Select(d => d.ToString())) + Environment.NewLine;
				//tbOutput.Text += string.Concat(Environment.NewLine, Environment.NewLine, Environment.NewLine,
				//				Environment.NewLine, Environment.NewLine, Environment.NewLine,
				//				"############################################", Environment.NewLine, 
				//				"############################################", Environment.NewLine, 
				//				"############################################", Environment.NewLine,
				//				Environment.NewLine, Environment.NewLine, "SequenceFREQUENCY",Environment.NewLine,
				//				string.Join(Environment.NewLine, seqFrequency.Select(d => d.ToString())),
				//				Environment.NewLine, Environment.NewLine, Environment.NewLine);
				//
				//tbOutput.Text += "SequenceProbabiltyDelta" + Environment.NewLine;
				//List<decimal> seqProbabiltyDelta = Stylometry.GetSequenceDelta(seqProbabilty);
				//tbOutput.Text += string.Join(Environment.NewLine, seqProbabiltyDelta.Select(d => d.ToString()));
				//tbOutput.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
				//
				//tbOutput.Text += "SequenceFREQUENCYDelta" + Environment.NewLine;
				//List<decimal> seqFrequencyDelta = Stylometry.GetSequenceDelta(seqFrequency);
				//tbOutput.Text += string.Join(Environment.NewLine, seqFrequencyDelta.Select(d => d.ToString()));
				//tbOutput.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;

				IsDatasetDirty = true;
				UpdateLabels();
			}
		}

		private void btnVisualizeDict_Click(object sender, EventArgs e)
		{
			string lastWord = extractLastWord(tbOutput.Text);

			if (string.IsNullOrWhiteSpace(lastWord))
			{
				tbOutput.Lines = dataSet.GetDistinctSortedWordStrings().Take(5000).ToArray(); // dataSet.GetDistinctSortedWordFrequencyString();
			}
			else
			{
				Dictionary<string, Word> dict = dataSet.GetInternalDictionary();
				Word word = dict[lastWord];

				tbOutput.AppendText($"{Environment.NewLine}\t");
				tbOutput.AppendText(string.Join($"{Environment.NewLine}\t", word.SuggestNextWords(-1)));
			}
		}

		private void btnDumpAll_Click(object sender, EventArgs e)
		{
			string selectedFile = ShowFileDialog(openFileDialog);
			if (!string.IsNullOrWhiteSpace(selectedFile))
			{
				File.WriteAllText(selectedFile, string.Empty); // Truncate the file if it already exists since we 

				IEnumerable<string> lines = dataSet.GetEntireDictionaryString();

				StringBuilder stringBuilder = new StringBuilder((int)Math.Min(10000, dataSet.UniqueWordCount));
				foreach (string line in lines)
				{
					stringBuilder.AppendLine(line);
					if (stringBuilder.Length > 10000)
					{
						File.AppendAllText(selectedFile, stringBuilder.ToString());
						stringBuilder.Clear();
					}
				}

				File.AppendAllText(selectedFile, stringBuilder.ToString());
				stringBuilder.Clear();
				stringBuilder = null;
			}
		}

		#region Graph View Visualizer Methods

		private List<string> ExclusionWordList = new List<string>();

		private void btnLoadWordExlusionList_Click(object sender, EventArgs e)
		{
			string selectedFile = ShowFileDialog(openFileDialog);
			if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile))
			{
				IEnumerable<string> exclusionWords = File.ReadAllLines(selectedFile);
				exclusionWords = exclusionWords.Select(str => str.Trim());
				exclusionWords = exclusionWords.Where(str => !string.IsNullOrWhiteSpace(str));
				exclusionWords = exclusionWords.Select(str => str.ToLowerInvariant());
				exclusionWords = exclusionWords.Except(new string[] { "{{start}}", "{{end}}" });
				exclusionWords = exclusionWords.Distinct();

				if (exclusionWords.Any())
				{
					ExclusionWordList.AddRange(exclusionWords);
					ExclusionWordList = ExclusionWordList.Distinct().ToList();
				}
			}
		}

		private void btnViewGraph_Click(object sender, EventArgs e)
		{
			int breadth = int.Parse(tbBreath.Text);
			int depth = int.Parse(tbDepth.Text);

			Graph graph = BuildGraphFromDataset(dataSet, breadth, depth);

			GraphVisualizer visualizerForm = new GraphVisualizer(graph);
			visualizerForm.Show(this);
		}

		private Graph BuildGraphFromDataset(TrainedDataSet dataset, int breadth = -1, int depth = -1)
		{
			Graph graph = new Graph("WordsGraph");

			BuildGraphRecursive(ref graph, dataset.Find(WordDictionary.StartPlaceholder), breadth, depth);

			return graph;
		}

		private void BuildGraphRecursive(ref Graph graph, Word word, int breadth, int depth)
		{
			if (depth == 0)
			{
				return;
			}

			string fromNode = word.Value;

			IEnumerable<Word> nextWords = word.GetFrequencyDictionary().Select(kvp => kvp.Key);
			nextWords = nextWords.Where(wrd => !ExclusionWordList.Contains(wrd.Value));

			if (!nextWords.Any())
			{
				return;
			}

			if (breadth != -1)
			{
				nextWords = nextWords.Take(breadth);
			}

			foreach (Word nextWord in nextWords)
			{
				graph.AddEdge(fromNode, nextWord.Value);
				BuildGraphRecursive(ref graph, nextWord, breadth - 1, depth - 1);
			}
		}

		private void btnPopulateList_Click(object sender, EventArgs e)
		{
			IEnumerable<Word> words = dataSet.GetDistinctSortedWords().Where(wrd => !ExclusionWordList.Contains(wrd.Value));

			if (words.Any())
			{
				listWords.Items.Clear();
				foreach (Word word in words)
				{
					listWords.Items.Add(word.Value);
				}
			}
		}

		private void btnViewSelectedWordGraph_Click(object sender, EventArgs e)
		{
			if (listWords.SelectedIndex == -1)
			{
				ShowInfo("Select a word from the ListBox above first.");
				return;
			}

			dataSet.OrderInternalDictionary();

			List<string> selectedItems = listWords.SelectedItems.OfType<object>().Select(obj => obj.ToString()).ToList();
			List<Word> selectedWords = selectedItems.Select(itm => dataSet.Find(itm)).ToList();
			selectedWords = selectedWords.Where(wrd => !ExclusionWordList.Contains(wrd.Value)).ToList();

			List<Word> copy = selectedWords.ToList();

			bool includePreviousWords = cbShowPriorWords.Checked;
			bool includeNextWords = cbShowSubsequentWords.Checked;

			Graph graph = new Graph("WordsGraph");

			// First add all the selected words to the graph as nodes.
			// It is necessary to make sure all the nodes exist first, so that graph.FindNode() below works and doesn't throw an exception.
			foreach (Word currentWord in selectedWords)
			{
				graph.AddNode(currentWord.Value); // Add node, just in case there are no connections (edges) to add, the word node will still show up.
			}

			foreach (Word currentWord in selectedWords)
			{
				string centerNodeWord = currentWord.Value;

				// Construct list of previous words to include in the graph for the current word
				List<string> previousWords = new List<string>();
				if (includePreviousWords)
				{
					previousWords = dataSet.GetDistinctSortedWords().Where(wrd => wrd.GetNextWordDictionary().Contains(centerNodeWord)).Select(wrd => wrd.Value).ToList();
				}
				else
				{
					// If user elected not to include (all) previous words,
					// we still want to include any connections amongst the subset of words selected for explicit inclusion in this graph.
					List<Word> search = copy.Except(new Word[] { currentWord }).ToList();
					previousWords = search.Where(wrd => wrd.GetNextWordDictionary().Contains(centerNodeWord)).Select(wrd => wrd.Value).ToList();
				}

				// Construct list of next words to include in the graph for the current word
				List<string> nextWords = new List<string>();
				if (includeNextWords)
				{
					nextWords = currentWord.GetNextWordDictionary().GetNextWordByFrequencyDescending().ToList();
				}
				else
				{
					// If user elected not to include (all) next words,
					// we still want to include any connections amongst the subset of words selected for explicit inclusion in this graph.
					List<string> search = copy.Except(new Word[] { currentWord }).Select(wrd => wrd.Value).ToList();
					nextWords = currentWord.GetNextWordDictionary().GetNextWordByFrequencyDescending().Where(nxtWrd => search.Contains(nxtWrd)).ToList();
				}

				// Go ahead and add all the words gathered in the previous steps to the graph now.
				foreach (string previousWord in previousWords)
				{
					Edge newEdge = new Edge(previousWord, string.Empty, centerNodeWord);
					bool alreadyContainsEdge = graph.Edges.Contains(newEdge, new GraphEdgeComparer());
					if (!alreadyContainsEdge) // Don't add the same edge more than once.
					{
						graph.AddEdge(previousWord, centerNodeWord); // Previous Word -> Current Word
					}
				}
				foreach (string nextWord in nextWords)
				{
					Edge newEdge = new Edge(centerNodeWord, string.Empty, nextWord);
					bool alreadyContainsEdge = graph.Edges.Contains(newEdge, new GraphEdgeComparer());
					if (!alreadyContainsEdge) // Don't add the same edge more than once.
					{
						graph.AddEdge(centerNodeWord, nextWord); // Current Word -> Next Word
					}
				}
			}

			GraphVisualizer visualizerForm = new GraphVisualizer(graph);
			visualizerForm.Show(this);
		}

		#endregion

	}
}
