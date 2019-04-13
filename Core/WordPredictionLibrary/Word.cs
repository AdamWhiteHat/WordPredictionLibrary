using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace WordPredictionLibrary.Core
{
	public class Word : IEquatable<string>
	{
		public string Value { get; internal set; }
		public decimal NextWordDistinctCount { get { return _nextWordDictionary.DistinctWordCount; } }
		public decimal AbsoluteFrequency { get { return _nextWordDictionary.TotalWordCount; } }

		public static decimal noMatchValue = 0;

		internal NextWordFrequencyDictionary _nextWordDictionary;

		internal Dictionary<List<string>, int> _previousWordsDictionary;


		#region Constructors

		public Word()
		{
			_previousWordsDictionary = new Dictionary<List<string>, int>(new WordListComparer());
			_nextWordDictionary = new NextWordFrequencyDictionary();
		}

		public Word(string value)
			: this()
		{
			Value = value.TryToLower();
		}

		#endregion

		#region Equals & IEquatable Overrides

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			else if (ReferenceEquals(this, obj))
			{
				return true;
			}
			else if (obj is string)
			{
				string str = (string)obj;
				return this.Value.Equals(str, StringComparison.InvariantCultureIgnoreCase);
			}
			else if (obj is Word)
			{
				Word wrd = (Word)obj;
				return this.Value.Equals(wrd.Value, StringComparison.InvariantCultureIgnoreCase);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public bool Equals(Word wrd)
		{
			return this.Value.Equals(wrd.Value, StringComparison.InvariantCultureIgnoreCase);
		}

		public bool Equals(string str)
		{
			return this.Value.Equals(str, StringComparison.InvariantCultureIgnoreCase);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			result.AppendFormat(
						"[\"{0}\" - {1}/{2}:",
						Value.ToUpperInvariant(),
						NextWordDistinctCount,
						AbsoluteFrequency
			);
			result.AppendLine("");
			result.Append(_nextWordDictionary.ToString());
			result.Append("],");

			return result.ToString();
		}

		#endregion

		#region Dictionary Altering Methods

		public void AddPreviousWords(List<string> previousWords)
		{
			if (_previousWordsDictionary.ContainsKey(previousWords))
			{
				_previousWordsDictionary[previousWords] += 1;
			}
			else
			{
				_previousWordsDictionary.Add(previousWords, 1);
			}
		}

		public void AddNextWord(Word word)
		{
			_nextWordDictionary.Add(word);
		}

		public string SuggestNextWord()
		{
			return _nextWordDictionary.GetNextWord();
		}

		public string SuggestNextWord(string previousWord)
		{
			return _nextWordDictionary.GetNextWord(previousWord);
		}

		public IEnumerable<string> SuggestNextWords(int count)
		{
			return _nextWordDictionary.TakeTop(count);
		}

		public IEnumerable<string> SuggestNextWords(string previousWord, int count)
		{
			return _nextWordDictionary.TakeTop(previousWord, count);
		}

		#endregion

		#region Get Dictionary

		Dictionary<Word, decimal> _freqDict = null;
		public Dictionary<Word, decimal> GetFrequencyDictionary()
		{
			if (_freqDict == null && _nextWordDictionary.DistinctWordCount > 0)
			{
				_freqDict = _nextWordDictionary.GetFrequencyDictionary();
				return _freqDict;
			}
			return new Dictionary<Word, decimal>();
		}

		public NextWordFrequencyDictionary GetNextWordDictionary()
		{
			return _nextWordDictionary;
		}

		#endregion

		#region Suggest

		public decimal GetNextWordProbability(Word nextWord)
		{
			if (!_nextWordDictionary.Contains(nextWord)) { return noMatchValue; }

			decimal nextWordOccurrences = _nextWordDictionary[nextWord];
			decimal absoluteFrequency = AbsoluteFrequency;

			return nextWordOccurrences / absoluteFrequency;
		}

		public decimal GetNextWordFrequency(Word nextWord)
		{
			if (!_nextWordDictionary.Contains(nextWord)) { return noMatchValue; }

			return this.GetFrequencyDictionary()[nextWord];
		}

		#endregion

		#region Statistics

		public decimal GetVariance()
		{
			if (_nextWordDictionary == null) { return noMatchValue; }

			decimal sum = _nextWordDictionary._internalDictionary.Sum(kvp => (long)kvp.Value);
			decimal mean = sum / AbsoluteFrequency;

			decimal squaredDeviations = _nextWordDictionary._internalDictionary.Sum(kvp => (decimal)Math.Pow((double)(kvp.Value - mean), 2));
			decimal variance = squaredDeviations / mean;
			return variance;
		}

		public decimal GetStandardDeviation()
		{
			if (_nextWordDictionary == null) { return noMatchValue; }

			return (decimal)Math.Sqrt((double)GetVariance());
		}

		#endregion

		#region Order

		public void OrderInternalDictionary()
		{
			if (_nextWordDictionary._internalDictionary.Any())
			{
				Dictionary<Word, decimal> nextDict = _nextWordDictionary._internalDictionary.OrderByFrequencyDescending().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
				_nextWordDictionary = new NextWordFrequencyDictionary(nextDict);
			}

			if (_previousWordsDictionary.Any())
			{
				Dictionary<List<string>, int> previousDict = _previousWordsDictionary.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value, new WordListComparer());
				_previousWordsDictionary = previousDict;
			}
		}

		#endregion

	}
}
