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
		public string Value { get; private set; }
		public int TotalWords { get; private set; }
		private NextWordFrequencyDictionary nextWordFrequencyDictionary;
		
		private double BaseProbability
		{
			get
			{
				if (isBaseProbabilityDirty)
				{
					_baseProbablility = 1.0d / TotalWords;
					isBaseProbabilityDirty = false;
				}
				return _baseProbablility;
			}
		}
		private double _baseProbablility;
		private bool isBaseProbabilityDirty;
				
		public Word(string value)
		{
			nextWordFrequencyDictionary = new NextWordFrequencyDictionary();
			TotalWords = 0;
			_baseProbablility = 0;
			isBaseProbabilityDirty = false;

			value = MakeLower(value);
			Value = value;
		}		

		//public void AddNextWord(string word)
		//{
		//	if (nextWordFrequencyDictionary.Contains(word))
		//	{
		//		nextWordFrequencyDictionary[word]++;
		//	}
		//	else
		//	{
		//		nextWordFrequencyDictionary.Add(word);
		//	}
		//
		//	TotalWords++;
		//	isBaseProbabilityDirty = true;
		//}

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

			TotalWords++;
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

		//public double GetNextWordProbability(string word)
		//{
		//	double result = 0;
		//	if (nextWordFrequencyDictionary.Any(kvp => kvp.Key.Value == word))
		//	{
		//		result = BaseProbability * nextWordFrequencyDictionary.Single(kvp => kvp.Key.Value == word).Value;
		//	}
		//	return result;
		//}
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

		private string MakeLower(string input)
		{
			if (!string.IsNullOrWhiteSpace(input))
			{
				input = input.ToLowerInvariant();
			}
			return input;
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
    }
}
