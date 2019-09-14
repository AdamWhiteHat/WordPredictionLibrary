using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WordPredictionLibrary.Core
{
	public class NextWordFrequencyDictionary
	{
		public int DistinctWordCount { get { return _internalDictionary.Count; } }
		public decimal TotalWordCount { get { return _internalDictionary.Values.Sum(); } }

		internal Dictionary<Word, decimal> _internalDictionary = null;
		private bool isOrdered = false;
		private static decimal noMatchValue = 0;

		#region Constructors

		public NextWordFrequencyDictionary()
			: this(new Dictionary<Word, decimal>())
		{
		}

		public NextWordFrequencyDictionary(Dictionary<Word, decimal> dictionary)
		{
			_internalDictionary = dictionary;
			isOrdered = false;
		}

		#endregion

		#region ToString Override

		public IEnumerable<string> FormatAsString()
		{
			if (!_internalDictionary.Values.Any())
			{
				yield break;
			}

			int padding = 0;
			decimal counter = _internalDictionary.Values.Max();

			while (counter-- > 0)
			{
				IEnumerable<KeyValuePair<Word, decimal>> matches = _internalDictionary.Where(kvp => kvp.Value == counter);
				if (!matches.Any())
				{
					continue;
				}

				padding++;
				string paddingString = new string(Enumerable.Repeat<char>(' ', padding).ToArray());

				foreach (KeyValuePair<Word, decimal> kvp in matches)
				{
					yield return string.Format("{0}{1}:{2}   ", paddingString, kvp.Value, kvp.Key.Value);
				}
			}
			yield break;
		}

		#endregion

		#region Dictionary Altering Methods

		public void Add(Word word)
		{
			isOrdered = false;
			if (this.Contains(word))
			{
				_internalDictionary[word] += 1;
			}
			else
			{
				_internalDictionary.Add(word, 1);
			}
		}

		public decimal this[Word key]
		{
			get { return this.Contains(key) ? _internalDictionary[key] : noMatchValue; }
		}

		public bool Contains(Word key)
		{
			return _internalDictionary.ContainsKey(key);
		}

		public bool Contains(string key)
		{
			string lowerKey = key.TryToLower();
			return _internalDictionary.Any(kvp => kvp.Key.Equals(lowerKey));
		}

		#endregion

		#region Suggest Methods

		public string GetNextWord()
		{
			return GetNextWordByFrequencyDescending().FirstOrDefault();
		}

		public string GetNextWord(string previousWord)
		{
			return GetNextWordByFrequencyDescending(previousWord).FirstOrDefault(); ;
		}

		public IEnumerable<string> TakeTop(int count)
		{
			if (count == -1)
			{
				return GetNextWordByFrequencyDescending();
			}
			return GetNextWordByFrequencyDescending().Take(count);
		}

		public IEnumerable<string> TakeTop(string previousWord, int count)
		{
			if (count == -1)
			{
				return GetNextWordByFrequencyDescending(previousWord);
			}
			return GetNextWordByFrequencyDescending(previousWord).Take(count);
		}


		internal void OrderInternalDictionary(SortCriteria sortCriteria, SortDirection sortDirection)
		{
			if (_internalDictionary != null && _internalDictionary.Any())
			{
				if (!isOrdered)
				{
					isOrdered = true;
					IOrderedEnumerable<KeyValuePair<Word, decimal>> ordered = null;

					if (sortCriteria == SortCriteria.AbsoluteFrequency)
					{
						ordered = _internalDictionary.OrderDictionaryBy(kvp => kvp.Key.AbsoluteFrequency, sortDirection);
					}
					else if (sortCriteria == SortCriteria.NextWordCount)
					{
						ordered = _internalDictionary.OrderDictionaryBy(kvp => kvp.Key.NextWordDistinctCount, sortDirection);
					}
					else if (sortCriteria == SortCriteria.StandardDeviation)
					{
						ordered = _internalDictionary.OrderDictionaryBy(kvp => kvp.Key.GetStandardDeviation(), sortDirection);
					}
					else if (sortCriteria == SortCriteria.Variance)
					{
						ordered = _internalDictionary.OrderDictionaryBy(kvp => kvp.Key.GetVariance(), sortDirection);
					}

					_internalDictionary = ordered.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
				}
			}
		}

		public IEnumerable<string> GetNextWordByFrequencyDescending()
		{
			OrderInternalDictionary(SortCriteria.AbsoluteFrequency, SortDirection.Descending);

			if (_internalDictionary != null)
			{
				return _internalDictionary.Select(kvp => kvp.Key.Value);
			}
			else
			{
				return new List<string>();
			}
		}

		public IEnumerable<string> GetNextWordByFrequencyDescending(string previousWord)
		{
			OrderInternalDictionary(SortCriteria.AbsoluteFrequency, SortDirection.Descending);
			return FilterDictionaryByPreviousWord(previousWord).Select(kvp => kvp.Key.Value);
		}

		#endregion

		#region Get Dictionary

		public Dictionary<Word, decimal> FilterDictionaryByPreviousWord(string previousWord)
		{
			Dictionary<Word, decimal> result = _internalDictionary.Where(w => w.Key.Value == previousWord).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			return result;
		}

		/// <summary>
		/// The frequency is a value between 0 and 1 that is a proportional fraction of 
		/// the number of times it was observed out of all the data observed.
		/// </summary>
		public Dictionary<Word, decimal> GetFrequencyDictionary()
		{
			decimal baseProbability = 1 / TotalWordCount;

			Dictionary<Word, decimal> frequencyDict = _internalDictionary.ToDictionary(k => k.Key, v => baseProbability * v.Value);
			IOrderedEnumerable<KeyValuePair<Word, decimal>> orderedFreqKvp = frequencyDict.OrderDictionaryBy(kvp => kvp.Value, SortDirection.Descending);

			return orderedFreqKvp.ToDictionary(k => k.Key, v => v.Value);
		}

		#endregion

	}
}
