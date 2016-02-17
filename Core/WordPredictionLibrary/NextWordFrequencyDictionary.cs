using System;
using System.Linq;
using System.Collections.Generic;

namespace WordPredictionLibrary
{
	public class NextWordFrequencyDictionary
	{
		internal Dictionary<Word, int> nextWordDictionary;
		public int Count { get { return nextWordDictionary.Count; } }

		public NextWordFrequencyDictionary()
		{
			nextWordDictionary = new Dictionary<Word, int>();
		}

		public NextWordFrequencyDictionary(Dictionary<Word, int> dictionary)
			: this()
		{
			nextWordDictionary = dictionary;
		}

		public IEnumerable<string> GetNextWordByFrequencyDescending()
		{
			return OrderByFrequencyDescending().Select(kvp => kvp.Key.Value);
		}

		public IOrderedEnumerable<KeyValuePair<Word, int>> OrderByFrequencyDescending()
		{
			return nextWordDictionary.OrderByDescending(kvp => kvp.Value);
		}

		public string GetNextWord()
		{			
			return GetNextWordByFrequencyDescending().FirstOrDefault();
		}

		public IEnumerable<string> TakeTop(int count)
		{
			return GetNextWordByFrequencyDescending().Take(count);
		}

		public override string ToString()
		{
			return string.Join(Environment.NewLine,
				nextWordDictionary.OrderByDescending(kvp => kvp.Value).Select(kvp => string.Format("{0}:{1}", kvp.Value, kvp.Key.Value))
			);
		}
		
		#region Word Implementation

		public int this[Word key]
		{
			get { return this.Contains(key) ? nextWordDictionary[key] : 0; }
			set { if (this.Contains(key)) { nextWordDictionary[key] = value; } }
		}

		public void Add(Word word)
		{			
			if (!this.Contains(word))
			{
				nextWordDictionary.Add(word, 1);
			}
		}

		public bool Contains(Word key)
		{		
			return nextWordDictionary.ContainsKey(key);
		}
		
		#endregion

		#region String Implementation

		public void Add(string word)
		{
			string lowerWord = word.TryToLower();
			if (!this.Contains(lowerWord))
			{
				nextWordDictionary.Add(new Word(lowerWord), 1);
			}
		}
				
		//public int this[string key] 
		//{
		//	get 
		//	{
		//		return this.Contains(key) ? nextWordDictionary.Single(kvp => kvp.Key.Equals(key)).Value : 0;
		//	}
		//	set
		//	{
		//		if (this.Contains(key))
		//		{
		//			nextWordDictionary[nextWordDictionary.Keys.Where(wrd => wrd.Equals(key)).First()] = value;
		//		}
		//	}
		//}
		
		public bool Contains(string key)
		{
			string lowerKey = key.TryToLower();
			return nextWordDictionary.Any(kvp => kvp.Key.Equals(lowerKey));
		}

		#endregion
	}
}
