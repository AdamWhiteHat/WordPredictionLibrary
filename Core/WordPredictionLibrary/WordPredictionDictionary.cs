using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WordPredictionLibrary
{
	public class WordPredictionDictionary : IXmlSerializable
	{
		public int Count { get { return wordDictionary.Count; } }

		private Dictionary<string, Word> wordDictionary;		
		private static string EndPlaceholder = "{{end}}";

		public WordPredictionDictionary()
		{
			wordDictionary = new Dictionary<string, Word>();
		}

		#region Suggest

		public string SuggestNextWord(string fromWord)
		{
			fromWord = fromWord.TryToLower();
			if (wordDictionary.ContainsKey(fromWord))
			{
				return wordDictionary[fromWord].SuggestNextWord();
			}
			return string.Empty;
		}

		public IEnumerable<string> Suggest(string fromWord, int quantity)
		{
			fromWord = fromWord.TryToLower();
			if (wordDictionary.ContainsKey(fromWord))
			{
				return wordDictionary[fromWord].SuggestNextWords(quantity);
			}
			return new List<string>() { };
		}

		#endregion

		#region Train

		public void Train(IEnumerable<string> sentence)
		{
			string lastWord = string.Empty;
			foreach (string word in sentence)
			{
				if (!string.IsNullOrEmpty(lastWord))
				{
					Add(lastWord, word);
				}
				lastWord = word;
			}
			if (!string.IsNullOrEmpty(lastWord))
			{
				Add(lastWord, EndPlaceholder);
			}
		}

		public void Add(string word, string nextWord)
		{
			string lowerWord = word.TryToLower();
			string lowerNextWord = nextWord.TryToLower();
			Add(lowerWord);
			Add(lowerNextWord);
			wordDictionary[lowerWord].TrainNextWord(wordDictionary[lowerNextWord]);
		}

		private void Add(string word)
		{
			string lowerWord = word.TryToLower();
			if (!wordDictionary.ContainsKey(lowerWord))
			{
				wordDictionary.Add(lowerWord, new Word(lowerWord));
			}
		}

		#endregion

		#region IXmlSerializable

		private static class XmlElementNames
		{
			public static string WordNode = "Word";
			public static string ValueNode = "Value";
			public static string TotalWordCountNode = "TotalWordCount";
			public static string NextWordDictionaryNode = "NextWordDictionary";
			public static string NextWordDictionaryKeyValuePairNode = "KeyValuePair";
			public static string NextWordDictionaryKeyNode = "Key";
			public static string NextWordDictionaryValueNode = "Value";
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			bool isEmpty = reader.IsEmptyElement;

			reader.Read();

			if (isEmpty)
			{
				return;
			}

			try
			{
				wordDictionary = new Dictionary<string, Word>();
				while (reader.MoveToElement())
				{
					reader.ReadStartElement(XmlElementNames.WordNode);
					try
					{
						Word newWord = new Word();

						reader.ReadStartElement(XmlElementNames.ValueNode);
						try
						{
							newWord.Value = reader.ReadElementContentAsString();
						}
						finally
						{
							reader.ReadEndElement();
						}

						reader.ReadStartElement(XmlElementNames.TotalWordCountNode);
						try
						{
							newWord.TotalWordCount = reader.ReadElementContentAsInt();
						}
						finally
						{
							reader.ReadEndElement();
						}
						
						reader.ReadStartElement(XmlElementNames.NextWordDictionaryNode);
						try
						{
							while (reader.NodeType != XmlNodeType.EndElement)
							{
								reader.ReadStartElement(XmlElementNames.NextWordDictionaryKeyValuePairNode);
								try
								{
									string kvpKey = string.Empty;
									int kvpValue = 0;

									reader.ReadStartElement(XmlElementNames.NextWordDictionaryKeyNode);
									try
									{
										kvpKey = reader.ReadElementContentAsString();
									}
									finally
									{
										reader.ReadEndElement();
									}

									reader.ReadStartElement(XmlElementNames.NextWordDictionaryValueNode);
									try
									{
										kvpValue = reader.ReadElementContentAsInt();
									}
									finally
									{
										reader.ReadEndElement();
									}

									this.Add(kvpKey);
									newWord.nextWordFrequencyDictionary.nextWordDictionary.Add(this.wordDictionary[kvpKey], kvpValue);
								}
								finally
								{
									reader.ReadEndElement();
								}

								reader.MoveToContent();
							}
						}
						finally
						{
							reader.ReadEndElement();
						}
						
						wordDictionary.Add(newWord.Value, newWord);
					}
					finally
					{
						reader.ReadEndElement();
					}

					reader.MoveToContent();
				}
			}
			finally
			{
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			foreach (Word wordEntry in wordDictionary.Values.Cast<Word>())
			{
				writer.WriteStartElement(XmlElementNames.WordNode);
				try
				{
					// Value
					writer.WriteStartElement(XmlElementNames.ValueNode);
					try
					{
						writer.WriteValue(wordEntry.Value);
					}
					finally
					{
						writer.WriteEndElement();
					}

					// Total Word Count
					writer.WriteStartElement(XmlElementNames.TotalWordCountNode);
					try
					{
						writer.WriteValue(wordEntry.TotalWordCount);
					}
					finally
					{
						writer.WriteEndElement();
					}

					// Next Word Dictionary
					writer.WriteStartElement(XmlElementNames.NextWordDictionaryNode);
					try
					{
						foreach (KeyValuePair<Word, int> keyValuePair in wordEntry.nextWordFrequencyDictionary.nextWordDictionary)
						{
							// KeyValuePair Node
							writer.WriteStartElement(XmlElementNames.NextWordDictionaryKeyValuePairNode);
							try
							{
								// Key
								writer.WriteStartElement(XmlElementNames.NextWordDictionaryKeyNode);
								try
								{
									writer.WriteString(keyValuePair.Key.Value);
								}
								finally
								{
									writer.WriteEndElement();
								}

								// Value
								writer.WriteStartElement(XmlElementNames.NextWordDictionaryValueNode);
								try
								{
									writer.WriteValue(keyValuePair.Value);
								}
								finally
								{
									writer.WriteEndElement();
								}
							}
							finally
							{
								writer.WriteEndElement();
							}
						}
					}
					finally
					{
						writer.WriteEndElement();
					}
				}
				finally
				{
					writer.WriteEndElement();
				}
			}
		}

		#endregion

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			foreach (Word word in wordDictionary.OrderByDescending(kvp => kvp.Value.TotalWordCount).Select(kvp => kvp.Value))
			{
				result.AppendFormat("[Word \"{0}\": LinkedWords/Total = {1}/{2}, {3}{4}]", word.Value, word.TotalWordCount, this.Count, word.ToString(), Environment.NewLine);
			}
			return result.ToString();
		}
	}
}
