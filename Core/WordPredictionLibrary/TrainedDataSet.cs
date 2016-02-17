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
		public long TotalWordsProcessed { get; internal set; }
		public int UniqueWordsCataloged { get { return nextWordDictionary.Count; } }

		private WordPredictionDictionary nextWordDictionary;

		private static string AllowedChars = " .abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public TrainedDataSet()
		{
			TotalWordsProcessed = 0;
			nextWordDictionary = new WordPredictionDictionary();
		}

		public TrainedDataSet(WordPredictionDictionary dictionary)
			: this()
		{
			nextWordDictionary = dictionary;
		}

		public void Train(FileInfo paragraphFile)
		{
			List<List<string>> paragraphs = ParseTrainingFile(paragraphFile.FullName);

			foreach (List<string> sentance in paragraphs)
			{
				nextWordDictionary.Train(sentance);
			}
		}

		private List<List<string>> ParseTrainingFile(string filename)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("Cannot parse file that does not exist", filename);
			}

			string documentBody = new string(File.ReadAllText(filename).Where(c => AllowedChars.Contains(c)).ToArray());
			List<string> sentences = documentBody.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

			List<List<string>> paragraphs = new List<List<string>>();
			foreach (string sentence in sentences)
			{
				List<string> words = sentence.Split(' ').ToList();
				TotalWordsProcessed += words.Count;
				paragraphs.Add(words);
			}

#if DEBUG
			string debugParsedFileFilename = string.Format("Debug.{0}.Parsed.{1}.txt", Path.GetFileNameWithoutExtension(filename), TotalWordsProcessed);
			FileInfo parsedFile = new FileInfo(debugParsedFileFilename).RenameIfExists();

			int counter = 1;

			foreach (List<string> sentance in paragraphs)
			{
				File.AppendAllText(parsedFile.FullName, string.Format("Sentence{0} = \"{1}.\";{2}", counter++, string.Join(" ", sentance), Environment.NewLine));
			}
#endif

			return paragraphs;
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
				newWord.TotalWordCount = ttlWordCount;

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
					word.nextWordFrequencyDictionary.nextWordDictionary.Add(keyWord, valueInt);
				}
			}

			if (dictionary != null)
			{
				TrainedDataSet result = new TrainedDataSet(new WordPredictionDictionary(dictionary));
				result.TotalWordsProcessed = totalWordsProcessed;
				return result;
			}
			else
			{
				return new TrainedDataSet();
			}
		}

		public static bool SerializeToXml(TrainedDataSet dataset, string filename)
		{
			if (dataset.nextWordDictionary.wordDictionary == null || dataset.nextWordDictionary.wordDictionary.Count < 1)
			{
				return false;
			}

			// Sort every Word's internal dictionary
			foreach (Word word in dataset.nextWordDictionary.wordDictionary.Select(kvp => kvp.Value))
			{
				word.OrderInternalDictionary();
			}

			// Sort the NextWordDictionary
			dataset.nextWordDictionary = 
				new WordPredictionDictionary(
					dataset.nextWordDictionary.wordDictionary.OrderByDescending(kvp => kvp.Value.TotalWordCount).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
				);


			XDocument doc = new XDocument(
				new XElement(XmlElementNames.RootNode,
					new XElement(XmlElementNames.TotalWordsProcessedNode, dataset.TotalWordsProcessed),
					dataset.nextWordDictionary.wordDictionary.Values.Select(word =>
						new XElement(XmlElementNames.WordNode,
							new XElement(XmlElementNames.ValueNode, word.Value),
							new XElement(XmlElementNames.DictionarySizeNode, word.TotalWordCount),
							new XElement(XmlElementNames.DictionaryNode,
								word.nextWordFrequencyDictionary.nextWordDictionary.Select(kvp =>
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
