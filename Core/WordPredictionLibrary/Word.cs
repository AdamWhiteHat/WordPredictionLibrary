using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary
{
    public class Word : IEquatable<string>
	{
		public string Value { get; internal set; }
		public int TotalWordCount { get; internal set; }
		
		internal NextWordFrequencyDictionary nextWordFrequencyDictionary;
		private bool isBaseProbabilityDirty;
		private double _baseProbablility;
		private double BaseProbability
		{
			get
			{
				if (isBaseProbabilityDirty)
				{
					_baseProbablility = 1.0d / TotalWordCount;
					isBaseProbabilityDirty = false;
				}
				return _baseProbablility;
			}
		}

		public Word()
		{
			TotalWordCount = 0;
			_baseProbablility = 0;
			isBaseProbabilityDirty = false;			
			nextWordFrequencyDictionary = new NextWordFrequencyDictionary();			
		}

		public Word(string value)
			: this()
		{
			Value = value.TryToLower();
		}		

		public void TrainNextWord(Word word)
		{
			if (nextWordFrequencyDictionary.Contains(word))
			{
				nextWordFrequencyDictionary[word]++;
			}
			else
			{
				nextWordFrequencyDictionary.Add(word);
			}

			TotalWordCount++;
			isBaseProbabilityDirty = true;
		}

		public string SuggestNextWord()
		{
			return nextWordFrequencyDictionary.GetNextWord();
		}

		public IEnumerable<string> SuggestNextWords(int count)
		{
			return nextWordFrequencyDictionary.TakeTop(count);
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
			return string.Format("[NextWordFrequencyDictionary = ", this.Value) +
				"[" +
				Environment.NewLine + "\t" +
				string.Join(Environment.NewLine + "\t",
					nextWordFrequencyDictionary.OrderByFrequencyDescending().Select
						(kvp => string.Format("{0}:{1}", (BaseProbability * kvp.Value).ToString(), kvp.Key.Value))
				) + Environment.NewLine +
				"]" + Environment.NewLine;
		}
		//
		//public OrderedDictionary GetNextWordProbabilityDictionary()
		//{
		//	SortedDictionary<double, Word> sortedDict = new SortedDictionary<double, Word>();
		//	foreach (KeyValuePair<Word, int> kvp in nextWordFrequencyDictionary)
		//	{
		//		sortedDict.Add(BaseProbability * kvp.Value, kvp.Key);
		//	}
		//
		//	IOrderedEnumerable<KeyValuePair<double,Word>> orderedDescKvp = sortedDict.OrderByDescending(kvp => kvp.Key);
		//
		//	OrderedDictionary orderedDict = new OrderedDictionary();
		//	foreach (KeyValuePair<double, Word> kvp in orderedDescKvp)
		//	{
		//		orderedDict.Add(kvp.Value, kvp.Key);
		//	}
		//
		//	return orderedDict;
		//}
	}
}
