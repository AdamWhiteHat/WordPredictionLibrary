using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace WordPredictionLibrary
{
	public class WordDictionary
	{
		public static string EndPlaceholder = "{{end}}";
		public long TotalWordsProcessed { get { return this.Words.Select(w => (long)w.TotalWordsSeen).Sum(); } }
		public int UniqueWordCount { get { return _internalDictionary == null ? 0 : _internalDictionary.Count; } }
		internal List<Word> Words { get { return _internalDictionary.Values.ToList(); } }

		internal Dictionary<string, Word> _internalDictionary = null;

		public WordDictionary()
		{
			_internalDictionary = new Dictionary<string, Word>();
		}

		public WordDictionary(Dictionary<string, Word> dictionary)
		{
			_internalDictionary = dictionary;
		}
			

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			foreach (Word word in _internalDictionary.OrderByDescending(kvp => kvp.Value.TotalWordsSeen).Select(kvp => kvp.Value))
			{
				result.AppendFormat("[Word \"{0}\": LinkedWords/Total = {1}/{2}, {3}{4}]", word.Value, word.TotalWordsSeen, this.UniqueWordCount, word.ToString(), Environment.NewLine);
			}
			return result.ToString();
		}

		#region Suggest

		public string SuggestNextWord(string fromWord)
		{
			fromWord = fromWord.TryToLower();
			if (_internalDictionary.ContainsKey(fromWord))
			{
				return _internalDictionary[fromWord].SuggestNextWord();
			}
			return string.Empty;
		}

		public IEnumerable<string> Suggest(string fromWord, int quantity)
		{
			fromWord = fromWord.TryToLower();
			if (_internalDictionary.ContainsKey(fromWord))
			{
				return _internalDictionary[fromWord].SuggestNextWords(quantity);
			}
			return new List<string>() { };
		}

		public double GetWordPopularity(string word)
		{
			if (!_internalDictionary.ContainsKey(word))
			{
				return 0;
			}

			Word thisWord = _internalDictionary[word];
			int wordReferenceCount = thisWord.TotalWordsSeen;
			long totalReferenceCount = this.Words
											.Select(w => (long)w.TotalWordsSeen)
											.Sum();

			long averageReferenceCount = totalReferenceCount / this.UniqueWordCount;

			double average = (double)averageReferenceCount;
			double refCount = (wordReferenceCount);

			long lResult = wordReferenceCount / averageReferenceCount;
			double dResult = average / refCount;
			
			return dResult;
		}
		
		public double GetNextWordProbability(string current, string next)
		{
			if (!_internalDictionary.ContainsKey(current) || !_internalDictionary.ContainsKey(next))
			{
				return 0d;
			}
			return _internalDictionary[current].GetNextWordProbability(_internalDictionary[next]);
		}

		#endregion

		#region Train

		public void Train(List<List<string>> paragraph)
		{
			foreach (List<string> sentence in paragraph)
			{
				this.Train(sentence);
			}

			int i = 0;
		}

		public void Train(List<string> sentence)
		{
			if (sentence == null || sentence.Count < 1) { return; }
			sentence = sentence.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			string lastWord = string.Empty;
			foreach (string word in sentence)
			{
				if (string.IsNullOrWhiteSpace(word))
				{
					continue;
				}
				if (!string.IsNullOrEmpty(lastWord))
				{
					Add(lastWord, word);
				}
				lastWord = word;
			}
			Add(lastWord, EndPlaceholder);
		}

		internal void Add(string current, string next)
		{
			if (string.IsNullOrWhiteSpace(current))
			{
				return;
			}
			string currentLower = current.TryToLower();
			string nextLower = next.TryToLower();

			CreateIfRequired(currentLower);
			CreateIfRequired(nextLower);
			Word currentWord = _internalDictionary[currentLower];
			currentWord.AddNextWord(_internalDictionary[nextLower]);
		}

		private void CreateIfRequired(string word)
		{
			string lowerWord = word.TryToLower();
			if (lowerWord == "")
			{
				lowerWord = EndPlaceholder;
			}
			if (!_internalDictionary.ContainsKey(lowerWord))
			{
				_internalDictionary.Add(lowerWord, new Word(lowerWord));
			}
		}

		#endregion

	}
}
