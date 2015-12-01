using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary
{
	public class WordPredictionDictionary
	{
		private Word lastWord { get; set; }
		private Dictionary<string, Word> wordDictionary;

		public int Count { get { return wordDictionary.Count; } }
		public static string EndPlaceholder = "{{end}}";

		public WordPredictionDictionary()
		{
			wordDictionary = new Dictionary<string, Word>();
		}

		public string ToString()
		{
			StringBuilder result = new StringBuilder();
			
			foreach (Word word in wordDictionary.OrderByDescending(kvp => kvp.Value.TotalWords).Select(kvp => kvp.Value))
			{
				result.AppendFormat("[Word \"{0}\": LinkedWords/Total = {1}/{2}, {3}{4}]", word.Value, word.TotalWords, this.Count, word.ToString(), Environment.NewLine);
			}
			return result.ToString();
		}

		public string SuggestNextWord(string fromWord)
		{
			fromWord = MakeLower(fromWord);
			if (wordDictionary.ContainsKey(fromWord))
			{
				return wordDictionary[fromWord].SuggestNextWord();
			}
			return string.Empty;
		}

		public IEnumerable<string> Suggest(string fromWord, int count)
		{
			fromWord = MakeLower(fromWord);
			if (wordDictionary.ContainsKey(fromWord))
			{
				return wordDictionary[fromWord].SuggestNextWords(count);
			}
			return new List<string>() { };
		}

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
			word = MakeLower(word);
			nextWord = MakeLower(nextWord);
			Add(word);
			Add(nextWord);
			wordDictionary[word].TrainNextWord(wordDictionary[nextWord]);
		}

		void Add(string word)
		{
			word = MakeLower(word);
			if (!wordDictionary.ContainsKey(word))
			{
				wordDictionary.Add(word, new Word(word));
			}
		}

		private string MakeLower(string input)
		{
			if (!string.IsNullOrWhiteSpace(input))
			{
				input = input.ToLowerInvariant();
			}
			return input;
		}
	}

}
