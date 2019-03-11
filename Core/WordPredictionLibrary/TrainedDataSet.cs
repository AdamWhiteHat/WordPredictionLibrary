using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Text;

namespace WordPredictionLibrary.Core
{
	public class TrainedDataSet
	{
		public decimal UniqueWordCount { get { return _wordDictionary.UniqueWordCount; } }
		public decimal TotalSampleSize { get { return _wordDictionary.TotalSampleSize; } }

		internal WordDictionary _wordDictionary = null;
		private static string AllowedChars = " .abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		#region Constructors

		public TrainedDataSet()
		{
			_wordDictionary = new WordDictionary();
		}

		public TrainedDataSet(WordDictionary dictionary)
		{
			_wordDictionary = dictionary;
		}
		
		#endregion

		#region Suggest & Train

		public string SuggestNext(string currentWord)
		{
			return _wordDictionary.SuggestNextWord(currentWord);
		}

		public void Train(FileInfo paragraphFile)
		{
			List<List<string>> paragraphs = TokenizeTextFile(paragraphFile.FullName);
			_wordDictionary.Train(paragraphs);
		}
			
		#endregion

		public Dictionary<Word, decimal> GetFrequencyDictionary()
		{
			return _wordDictionary.GetFrequencyDictionary();
		}

		public List<Word> GetDistinctSortedWords()
		{
			return _wordDictionary.GetDistinctSortedWordsList();
		}

		public string GetDistinctSortedWordFrequencyString()
		{
			return _wordDictionary.GetDistinctSortedWordFrequencyString();
		}

		internal static StringBuilder ReplaceWhile(string input, string oldValue, string newValue)
		{
			StringBuilder result = new StringBuilder(input);

			int beforeLength = 0;
			int afterLength = 0;			
			do
			{
				beforeLength = result.Length;

				result = result.Replace(oldValue, newValue);

				afterLength = result.Length;
			}
			while((beforeLength - afterLength) > 0);
						
			return result;
		}

		internal static List<List<string>> TokenizeTextFile(string filename)
		{
			if (!File.Exists(filename)) { throw new FileNotFoundException("Cannot parse file that does not exist", filename); }

			// Do replacement of characters here
			StringBuilder massagedFileText = new StringBuilder();
			massagedFileText.Append(File.ReadAllText(filename));
			massagedFileText
				.Replace('!', '.').Replace('?', '.')
				.Replace("'", string.Empty).Replace("`", string.Empty).Replace("\"", " ")
				.Replace("\t", " ").Replace(",", " ").Replace(";", " ").Replace(":", " ").Replace("-", " ")
				;

			massagedFileText
				.Replace("\\", " ").Replace("/", " ")
				.Replace("[", " ").Replace("]", " ").Replace("(", " ").Replace(")", " ")
				.Replace("{", " ").Replace("}", " ").Replace("<", " ").Replace(">", " ")
				.Replace("+", " ").Replace("=", " ").Replace("$", " ").Replace("&", " ")
				.Replace("@", " ").Replace("#", " ").Replace("%", " ")
				.Replace("^", " ").Replace("*", " ").Replace("~", " ")				
				;

			massagedFileText
				.Replace("\r", "").Replace("\n", " ");

			if (massagedFileText.Contains("1st"))
			{
				massagedFileText
					.Replace("1st", "first").Replace("2nd", "second").Replace("3rd", "third").Replace("4th", "fourth").Replace("5th", "fifth")
					.Replace("6th", "sixth").Replace("7th", "seventh").Replace("8th", "eighth").Replace("9th", "ninth").Replace("10th", "tenth")
					.Replace("11th", "eleventh").Replace("12th", "twelfth").Replace("13th", "thirteenth").Replace("14th", "fourteenth").Replace("15th", "fifteenth")
					;
			}

			if (massagedFileText.ToString().Any(c => Char.IsDigit(c)))
			{
				massagedFileText
					.Replace("0", "zero ")
					.Replace("1", "one ")
					.Replace("2", "two ")
					.Replace("3", "three ")
					.Replace("4", "four ")
					.Replace("5", "five ")
					.Replace("6", "six ")
					.Replace("7", "seven ")
					.Replace("8", "eight ")
					.Replace("9", "nine ")					
					;
			}

			bool contractions = true;
			if (contractions)
			{
				massagedFileText
					.Replace("arent", "are not")
					.Replace("cant", "cannot")
					.Replace("couldnt", "could not")
					.Replace("didnt", "did not")
					.Replace("doesnt", "does not")
					.Replace("dont", "do not")
					.Replace("hadnt", "had not")
					.Replace("hasnt", "has not")
					.Replace("havent", "have not")
					.Replace("im", "i am")
					.Replace("ive", "i have")
					.Replace("isnt", "is not")
					.Replace("lets", "let us")
					.Replace("mightnt", "might not")
					.Replace("mustnt", "must not")
					.Replace("shant", "shall not")
					.Replace("shouldnt", "should not")
					.Replace("theyre", "they are")
					.Replace("theyve", "they have")
					.Replace("were", "we are")
					.Replace("weve", "we have")
					.Replace("werent", "were not")
					.Replace("whatre", "what are")
					.Replace("whatve", "what have")
					.Replace("whore", "who are")
					.Replace("whove", "who have")
					.Replace("wont", "will not")
					.Replace("wouldnt", "would not")
					.Replace("youre", "you are")
					.Replace("youve", "you have")
					;
			}

			massagedFileText = ReplaceWhile(massagedFileText.ToString(), "  ", " ");

			string massagedText = massagedFileText.ToString();
			string sanitizedFileText = new string(massagedText.Where(c => AllowedChars.Contains(c)).ToArray());

			if (massagedText != sanitizedFileText)
			{
				int index = 0;
				int offset = 0;
				List<int> diffIndices = new List<int>();
				List<char> diffChars = new List<char>();
				foreach (char c in sanitizedFileText)
				{
					if (c != massagedText[index + offset])
					{
						diffIndices.Add(index + offset);
						diffChars.Add(massagedText[index + offset]);
						offset++;
					}
					index++;
				}

				string diff = string.Join("\n", diffIndices.Select(i => i.ToString())); //new string(diffChars.ToArray());
				throw new Exception(diff);
			}
						
			string outputFilename = "NORMALIZED__" + Path.GetFileName(filename);
			File.WriteAllText(outputFilename, sanitizedFileText);

			//List<string> fileLines = sanitizedFileText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
			//List<List<string>> fileParagraph = fileLines.Select(p => p.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()).ToList();

			List<string> fileSentences = sanitizedFileText.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

			int totalWordCount = 0;
			List<List<string>> result = new List<List<string>>();
			foreach (string sentence in fileSentences)
			{
				List<string> words = sentence.Split(' ').ToList();
				totalWordCount += words.Count;
				result.Add(words);
			}

//#if DEBUG
//			string debugParsedFileFilename = string.Format("Debug.{0}.Parsed.{1}.txt", Path.GetFileNameWithoutExtension(filename), totalWordCount);
//			FileInfo parseFile = new FileInfo(debugParsedFileFilename).RenameIfExists();
//			int counter = 1;
//			File.AppendAllText(
//				parseFile.FullName,
//				string.Join(
//					Environment.NewLine,
//					sentences.Select(l =>
//						string.Format("Sentence{0} = \"{1}\".", counter++, string.Join(" ", l))
//			)	)	);
//#endif
			return result;
		}

		#region ToString

		public override string ToString()
		{
			return _wordDictionary.ToString();
		}

		#endregion

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
			int totalWordsProcessed = 0; 
			int.TryParse(totalWordsNode.Value, out totalWordsProcessed);

			List<XElement> wordNodes = rootNode.XPathSelectElements(XmlElementNames.WordNode).ToList();
			if (wordNodes == null || wordNodes.Count < 1) { return new TrainedDataSet(); }

			// Create a Word object for each Word before populating NextWordFrequencyDictionary
			Dictionary<string, Word> dictionary = new Dictionary<string, Word>();			
			foreach (XElement wordNode in wordNodes)
			{
				XElement textNode = wordNode.XPathSelectElement(XmlElementNames.ValueNode);
				XElement countNode = wordNode.XPathSelectElement(XmlElementNames.DictionarySizeNode);

				if (textNode == null || countNode == null) { continue; }

				int ttlWordCount = 0; 
				int.TryParse(countNode.Value, out ttlWordCount);

				dictionary.Add(textNode.Value,  new Word(textNode.Value));
			}

			// Now populate NextWordFrequencyDictionary
			foreach (XElement wordNode in wordNodes)
			{
				XElement textNode = wordNode.XPathSelectElement(XmlElementNames.ValueNode);
				XElement dictNode = wordNode.XPathSelectElement(XmlElementNames.DictionaryNode);

				if (textNode == null || dictNode == null) { continue; }
				if (!dictionary.Keys.Contains(textNode.Value)) { continue; }

				Word word = dictionary[textNode.Value];

				List<XElement> kvpNodes = dictNode.XPathSelectElements(XmlElementNames.KeyValuePairNode).ToList();
				foreach (XElement kvpNode in kvpNodes)
				{
					XElement keyNode = kvpNode.XPathSelectElement(XmlElementNames.KeyNode);
					XElement valueNode = kvpNode.XPathSelectElement(XmlElementNames.ValueNode);

					string keyText = keyNode.Value;
					int valueInt = 0; int.TryParse(valueNode.Value, out valueInt);

					if (!dictionary.Keys.Contains(keyText)) { continue; }

					Word keyWord = dictionary[keyText];
					word._nextWordDictionary._internalDictionary.Add(keyWord, valueInt);
				}
			}

			if (dictionary != null) { return new TrainedDataSet(new WordDictionary(dictionary)); }
			else { return new TrainedDataSet(); }
		}

		public static bool SerializeToXml(TrainedDataSet dataset, string filename)
		{
			if (dataset == null	|| dataset._wordDictionary == null
				|| dataset._wordDictionary.UniqueWordCount < 1) { return false; }

			dataset._wordDictionary.OrderInternalDictionary();

			XDocument doc = new XDocument(
				new XElement(XmlElementNames.RootNode,
					new XElement(XmlElementNames.TotalWordsProcessedNode, dataset._wordDictionary.TotalSampleSize),
					dataset._wordDictionary.Words.Select(word =>
						new XElement(XmlElementNames.WordNode,
							new XElement(XmlElementNames.ValueNode, word.Value),
							new XElement(XmlElementNames.DictionarySizeNode, word.AbsoluteFrequency),
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
