using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WordPredictionLibrary
{
    public class Word : IEquatable<string>
	{
		public string Value { get; internal set; }
		public int TotalWordsSeen { get { return _nextWordDictionary.TotalWordsSeen; } }
		public int UniqueWordsSeen { get { return _nextWordDictionary.UniqueWordCount; } }
		
		internal NextWordFrequencyDictionary _nextWordDictionary;
	
		public Word()
		{	
			_nextWordDictionary = new NextWordFrequencyDictionary();			
		}

		public Word(string value)
			: this()
		{
			Value = value.TryToLower();
		}

		public void AddNextWord(Word word)
		{
			_nextWordDictionary.Add(word);
		}

		public string SuggestNextWord()
		{
			return _nextWordDictionary.GetNextWord();
		}

		public IEnumerable<string> SuggestNextWords(int count)
		{
			return _nextWordDictionary.TakeTop(count);
		}

		internal void OrderInternalDictionary()
		{
			_nextWordDictionary.OrderInternalDictionary();
		}

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
				return this.Value.Equals(wrd.Value,StringComparison.InvariantCultureIgnoreCase);
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
			return string.Format("[Word = ", this.Value) +
				"[" +
				Environment.NewLine + "\t" +
				string.Join(Environment.NewLine + "\t",
					_nextWordDictionary.OrderByFrequencyDescending().Select
						(kvp => string.Format("{0}:{1}", kvp.Value, kvp.Key.Value))
				) + Environment.NewLine +
				"]" + Environment.NewLine;
		}

		public double GetNextWordProbability(Word nextWord)
		{
			if (!_nextWordDictionary.Contains(nextWord))
			{
				return 0d;
			}
			double count = _nextWordDictionary[nextWord];
			double totalSeen = TotalWordsSeen;

			return count / totalSeen;
		}

		//public OrderedDictionary GetNextWordProbabilityDictionary()
		//{
		//	OrderedDictionary orderedDict = new OrderedDictionary();
		//	foreach (KeyValuePair<Word, int> kvp in nextWordFrequencyDictionary.OrderByFrequencyDescending())
		//		orderedDict.Add(kvp.Key, kvp.Value / TotalWordCount);
		//	return orderedDict;
		//}
	}
}
