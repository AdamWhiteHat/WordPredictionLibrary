using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace WordPredictionLibrary
{
	public class WordPredictionDictionary
	{
		public int Count { get { return wordDictionary.Count; } }

		internal Dictionary<string, Word> wordDictionary;
		private static string EndPlaceholder = "{{end}}";

		public WordPredictionDictionary()
		{
			wordDictionary = new Dictionary<string, Word>();
		}

		public WordPredictionDictionary(Dictionary<string, Word> dictionary)
			: this()
		{
			wordDictionary = dictionary;
		}
		
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			foreach (Word word in wordDictionary.OrderByDescending(kvp => kvp.Value.TotalWordCount).Select(kvp => kvp.Value))
			{
				result.AppendFormat("[Word \"{0}\": LinkedWords/Total = {1}/{2}, {3}{4}]", word.Value, word.TotalWordCount, this.Count, word.ToString(), Environment.NewLine);
			}
			return result.ToString();
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
			if (string.IsNullOrWhiteSpace(lowerNextWord))
			{
				lowerNextWord = EndPlaceholder;
			}

			Add(lowerWord);
			Add(lowerNextWord);
			wordDictionary[lowerWord].TrainNextWord(wordDictionary[lowerNextWord]);
		}

		private void Add(string word)
		{
			string lowerWord = word.TryToLower();
			if (!string.IsNullOrWhiteSpace(lowerWord) && !wordDictionary.ContainsKey(lowerWord))
			{
				wordDictionary.Add(lowerWord, new Word(lowerWord));
			}
		}

		#endregion
	
	}
}
