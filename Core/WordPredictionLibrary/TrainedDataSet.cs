using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;

namespace WordPredictionLibrary
{
	public class TrainedDataSet
	{
		public long TotalWordsProcessed { get { return _wordDictionary.TotalWordsProcessed; } }
		public int UniqueWordsCataloged { get { return _wordDictionary.UniqueWordCount; } }

		internal WordDictionary _wordDictionary;

		private static string AllowedChars = " .abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public TrainedDataSet()
		{
			_wordDictionary = new WordDictionary();
		}

		public TrainedDataSet(WordDictionary dictionary)
		{
			_wordDictionary = dictionary;
		}

		public string SuggestNext(string currentWord)
		{
			return _wordDictionary.SuggestNextWord(currentWord);
		}

		public void Train(FileInfo paragraphFile)
		{
			List<List<string>> paragraphs = TokenizeTextFile(paragraphFile.FullName);
			_wordDictionary.Train(paragraphs);
		}

		internal static List<List<string>> TokenizeTextFile(string filename)
		{
			if (!File.Exists(filename)) { throw new FileNotFoundException("Cannot parse file that does not exist", filename); }

			string rawFileText = File.ReadAllText(filename);
			// Do replacement of characters here
			string massagedFileText = rawFileText.Replace('!', '.');
			massagedFileText = rawFileText.Replace('?', '.');

			string sanitizedFileText = new string(rawFileText.Where(c => AllowedChars.Contains(c)).ToArray());
			//List<string> fileLines = sanitizedFileText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
			//List<List<string>> fileParagraph = fileLines.Select(p => p.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()).ToList();

			List<string> fileSentences = sanitizedFileText.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

			int totalWordCount = 0;
			List<List<string>> sentences = new List<List<string>>();
			foreach (string sentence in fileSentences)
			{
				List<string> words = sentence.Split(' ').ToList();
				totalWordCount += words.Count;
				sentences.Add(words);
			}

#if DEBUG
			string debugParsedFileFilename = string.Format("Debug.{0}.Parsed.{1}.txt", Path.GetFileNameWithoutExtension(filename), totalWordCount);
			FileInfo parseFile = new FileInfo(debugParsedFileFilename).RenameIfExists();

			int counter = 1;
			File.AppendAllText(
				parseFile.FullName,
				string.Join(
					Environment.NewLine,
					sentences.Select(l =>
						string.Format("Sentence{0} = \"{1}\".", counter++, string.Join(" ", l))
					)
				)
			);
#endif
			return sentences;
		}

		#region Xml Serialization

		private static class XmlElementNames
		{
			public static string KeyNode = "Key";
			public static string WordNode = "Word";
			public static string ValueNode = "Value";
			public static string RootNode = "TrainedDataSet";
			public static string KeyValuePairNode = "KeyValuePair";
			public static string DictionarySizeNode = "DictionarySize";
			public static string DictionaryNode = "NextwordDictionary";
			public static string TotalWordsProcessedNode = "TotalWordsProcessed";
		}

		public static TrainedDataSet DeserializeFromXml(string filename)
		{
			if (!File.Exists(filename)) { return new TrainedDataSet(); }

			XDocument doc = XDocument.Parse(File.ReadAllText(filename), LoadOptions.None);
			if (doc == null) { return new TrainedDataSet(); }

			XElement rootNode = doc.XPathSelectElement(XmlElementNames.RootNode);
			if (rootNode == null) { return new TrainedDataSet(); }

			XElement totalWordsNode = rootNode.XPathSelectElement(XmlElementNames.TotalWordsProcessedNode);
			if (totalWordsNode == null) { return new TrainedDataSet(); }
			int totalWordsProcessed = 0; int.TryParse(totalWordsNode.Value, out totalWordsProcessed);

			List<XElement> wordNodes = rootNode.XPathSelectElements(XmlElementNames.WordNode).ToList();
			if (wordNodes == null || wordNodes.Count < 1) { return new TrainedDataSet(); }

			// Dictionary
			Dictionary<string, Word> dictionary = new Dictionary<string, Word>();

			// Create a Word object for each Word before populating NextWordFrequencyDictionary
			foreach (XElement wordNode in wordNodes)
			{
				XElement textNode = wordNode.XPathSelectElement(XmlElementNames.ValueNode);
				XElement countNode = wordNode.XPathSelectElement(XmlElementNames.DictionarySizeNode);

				if (textNode == null || countNode == null)
				{
					continue;
				}

				string text = textNode.Value;
				int ttlWordCount = 0; int.TryParse(countNode.Value, out ttlWordCount);

				Word newWord = new Word(text);
				//newWord.TotalWordsSeen = ttlWordCount;

				dictionary.Add(text, newWord);
			}

			// Now populate NextWordFrequencyDictionary
			foreach (XElement wordNode in wordNodes)
			{
				XElement textNode = wordNode.XPathSelectElement(XmlElementNames.ValueNode);
				XElement dictNode = wordNode.XPathSelectElement(XmlElementNames.DictionaryNode);

				if (textNode == null || dictNode == null)
				{
					continue;
				}

				string text = textNode.Value;

				if (!dictionary.Keys.Contains(text))
				{
					continue;
				}

				Word word = dictionary[text];

				List<XElement> kvpNodes = dictNode.XPathSelectElements(XmlElementNames.KeyValuePairNode).ToList();
				foreach (XElement kvpNode in kvpNodes)
				{
					XElement keyNode = kvpNode.XPathSelectElement(XmlElementNames.KeyNode);
					XElement valueNode = kvpNode.XPathSelectElement(XmlElementNames.ValueNode);

					string keyText = keyNode.Value;
					int valueInt = 0; int.TryParse(valueNode.Value, out valueInt);

					if (!dictionary.Keys.Contains(keyText))
					{
						continue;
					}

					Word keyWord = dictionary[keyText];
					word._nextWordDictionary._internalDictionary.Add(keyWord, valueInt);
				}
			}

			if (dictionary != null)
			{
				TrainedDataSet result = new TrainedDataSet(new WordDictionary(dictionary));
				return result;
			}
			else
			{
				return new TrainedDataSet();
			}
		}

		public static bool SerializeToXml(TrainedDataSet dataset, string filename)
		{
			if (dataset == null || dataset._wordDictionary == null || dataset._wordDictionary.UniqueWordCount < 1)
			{
				return false;
			}

			// Sort every Word's internal dictionary
			foreach (Word word in dataset._wordDictionary.Words)
			{
				word.OrderInternalDictionary();
			}

			// Sort the NextWordDictionary
			dataset._wordDictionary =
				new WordDictionary(
					dataset._wordDictionary._internalDictionary.OrderByDescending(kvp => kvp.Value.TotalWordsSeen).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
				);

			XDocument doc = new XDocument(
				new XElement(XmlElementNames.RootNode,
					new XElement(XmlElementNames.TotalWordsProcessedNode, dataset._wordDictionary.TotalWordsProcessed),
					dataset._wordDictionary.Words.Select(word =>
						new XElement(XmlElementNames.WordNode,
							new XElement(XmlElementNames.ValueNode, word.Value),
							new XElement(XmlElementNames.DictionarySizeNode, word.TotalWordsSeen),
							new XElement(XmlElementNames.DictionaryNode,
								word._nextWordDictionary._internalDictionary.Select(kvp =>
									new XElement(XmlElementNames.KeyValuePairNode,
										new XElement(XmlElementNames.KeyNode, kvp.Key.Value),
										new XElement(XmlElementNames.ValueNode, kvp.Value)
									)
								)
							)
						)
					)
				)
			);

			if (doc != null)
			{
				doc.Save(filename, SaveOptions.None);
				return File.Exists(filename);
			}

			return false;
		}

		#endregion

	}
}
