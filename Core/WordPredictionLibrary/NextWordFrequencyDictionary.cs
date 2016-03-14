using System;
using System.Linq;
using System.Collections.Generic;

namespace WordPredictionLibrary
{
	public class NextWordFrequencyDictionary
	{
		internal Dictionary<Word, int> _internalDictionary = null;
		public int UniqueWordCount { get { return _internalDictionary.Count; } }
		public int TotalWordsSeen { get { return _internalDictionary.Values.Sum(); } }
				
		public NextWordFrequencyDictionary()
		{
			_internalDictionary = new Dictionary<Word, int>();
		}

		public NextWordFrequencyDictionary(Dictionary<Word, int> dictionary)
			: this()
		{
			_internalDictionary = dictionary;
		}

		public void Add(Word word)
		{
			if (this.Contains(word))
			{
				_internalDictionary[word] += 1;
			}
			else
			{
				_internalDictionary.Add(word, 1);
			}
		}

		public int this[Word key]
		{
			get { return this.Contains(key) ? _internalDictionary[key] : 0; }
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

		public string GetNextWord()
		{			
			return GetNextWordByFrequencyDescending().FirstOrDefault();
		}

		public IEnumerable<string> TakeTop(int count)
		{
			return GetNextWordByFrequencyDescending().Take(count);
		}

		public IEnumerable<string> GetNextWordByFrequencyDescending()
		{
			return OrderByFrequencyDescending().Select(kvp => kvp.Key.Value);
		}

		private IOrderedEnumerable<KeyValuePair<Word, int>> _orderedDictionary = null;
		public IOrderedEnumerable<KeyValuePair<Word, int>> OrderByFrequencyDescending()
		{
			// If we haven't set FrequencyDictionary yet OR it is out of date (dict has more entries)
			if (_orderedDictionary == null || _internalDictionary.Count > _orderedDictionary.Count())
			{
				_orderedDictionary = _internalDictionary.OrderByDescending(kvp => kvp.Value);
			}
			return _orderedDictionary;
		}		

		internal void OrderInternalDictionary()
		{
			_internalDictionary = OrderByFrequencyDescending().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		public override string ToString()
		{
			return string.Join(
				Environment.NewLine,
				this.OrderByFrequencyDescending().Select(kvp => string.Format("{0}:{1}", kvp.Value, kvp.Key.Value))
			);
		}
	}
}
