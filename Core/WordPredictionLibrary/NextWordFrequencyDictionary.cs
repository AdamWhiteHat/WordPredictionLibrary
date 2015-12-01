using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary
{
	public class NextWordFrequencyDictionary
	{
		private Dictionary<Word, int> nextWordDictionary;
		public int Count { get { return nextWordDictionary.Count; } }

		public string ToString()
		{
			return string.Join(Environment.NewLine,
				nextWordDictionary.OrderByDescending(kvp => kvp.Value).Select(kvp => string.Format("{0}:{1}",kvp.Value,kvp.Key.Value))
			);
		}

		public NextWordFrequencyDictionary()
		{
			nextWordDictionary = new Dictionary<Word, int>();
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
			word = MakeLower(word);
			if (!this.Contains(word))
			{
				nextWordDictionary.Add(new Word(word), 1);
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
			key = MakeLower(key);
			return nextWordDictionary.Any(kvp => kvp.Key.Equals(key));
		}

		private string MakeLower(string input)
		{
			if (!string.IsNullOrWhiteSpace(input))
			{
				input = input.ToLowerInvariant();
			}
			return input;
		}

		#endregion
	}
}
