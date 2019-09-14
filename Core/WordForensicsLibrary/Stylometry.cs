using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WordPredictionLibrary.Core;

namespace WordPredictionLibrary.Forensics
{
	public class Stylometry
	{
		private WordDictionary _wordDictionary;
		private static decimal noMatchValue = 0;
		//  text corpus
		// lexicon, idiom, diction, locution, vernacular, 
		// continuity, congruity, corpus, collocation,  forensic linguistics, article frequency

		public Stylometry(TrainedDataSet modelDatum)
		{
			if (modelDatum == null) { throw new ArgumentNullException("modelDatum"); }
			if (modelDatum._wordDictionary == null) { throw new ArgumentNullException("modelDatum._wordDictionary"); }

			_wordDictionary = modelDatum._wordDictionary;
		}

		public decimal GetUniqueWordCount()
		{
			return _wordDictionary.UniqueWordCount;
		}

		public decimal GetTotalSampleSize()
		{
			return _wordDictionary.TotalSampleSize;
		}

		public decimal GetSequenceMean(List<decimal> probability)
		{
			return probability.Sum() / probability.Count;
		}

		public static List<decimal> GetSequenceDelta(List<decimal> sequence)
		{
			decimal last = 0;
			List<decimal> results = new List<decimal>();
			foreach (decimal number in sequence)
			{
				if (number > last)
				{
					results.Add(number - last);
				}
				else if (number < last)
				{
					decimal diff = last - number;
					results.Add(0 - diff);
				}
				else if (number == last)
				{
					results.Add(0);
				}
				last = number;
			}
			return results;
		}

		//public List<decimal> GetFrequencyDictionarySuggestionDelta(string filePath)
		//{
		//	if (_wordDictionary == null) { throw new ArgumentNullException("_wordDictionary"); }
		//	if (!File.Exists(filePath)) { throw new FileNotFoundException("filePath"); }
		//	List<List<string>> testCorpus = TrainedDataSet.TokenizeTextFile(filePath);

		//	WordDictionary testDict = new WordDictionary();
		//	testDict.Train(testCorpus);

		//	List<Word> wordsOrdered = testDict.Words.OrderByDescending(w => w.AbsoluteFrequency).ToList();
		//	//IOrderedEnumerable<KeyValuePair<string,Word>> orderedDict = testDict._internalDictionary.OrderByDescending(kvp => kvp.Value.TotalWordsSeen);

		//	int counter = 0;
		//	int max = wordsOrdered.Count / 2;
		//	List<Tuple<string, string>> suggestedWords = new List<Tuple<string, string>>();
		//	while (counter < max)
		//	{
		//		wordsOrdered[counter].OrderInternalDictionary();
		//		suggestedWords.Add(new Tuple<string, string>(wordsOrdered[counter].Value, wordsOrdered[counter].SuggestNextWord()));
		//		counter++;
		//	}

		//	List<decimal> result = new List<decimal>();
		//	foreach (Tuple<string, string> pair in suggestedWords)
		//	{
		//		_wordDictionary.GetNextWordProbability(pair.Item1, pair.Item2);
		//	}

		//	return result;
		//}


		delegate decimal GetSequenceDelegate(string current, string next);

		public List<decimal> GetProbabilitySequence(string filePath)
		{
			return GetSequence(filePath, _wordDictionary.GetNextWordProbability);
		}

		public List<decimal> GetFrequencySequence(string filePath)
		{
			return GetSequence(filePath, _wordDictionary.GetNextWordFrequency);
		}

		public List<decimal> GetPopularitySequence(string filePath)
		{
			return GetSequence(filePath, _wordDictionary.GetNextWordPopularity);
		}

		private List<decimal> GetSequence(string filePath, GetSequenceDelegate workerMethod)
		{
			if (workerMethod == null) { throw new ArgumentNullException("workerMethod"); }
			if (!File.Exists(filePath)) { throw new FileNotFoundException("filePath"); }

			List<decimal> results = new List<decimal>();
			foreach (List<string> sentence in TrainedDataSet.TokenizeTextFile(filePath))
			{
				string lastWord = string.Empty;
				foreach (string word in sentence)
				{
					if (string.IsNullOrWhiteSpace(word)) { continue; }
					if (!string.IsNullOrEmpty(lastWord)) { results.Add(workerMethod(lastWord, word)); }
					lastWord = word;
				}
				results.Add(workerMethod(lastWord, WordDictionary.EndPlaceholder));
			}
			return results;
		}

		public decimal GetVariance()
		{
			if (_wordDictionary == null) { return noMatchValue; }
			return _wordDictionary.GetVariance();
		}

		public decimal GetVariance(string word)
		{
			if (_wordDictionary == null) { return noMatchValue; }
			return _wordDictionary._internalDictionary[word].GetVariance();
		}

		public decimal GetStandardDeviation()
		{
			if (_wordDictionary == null) { return noMatchValue; }
			return (decimal)Math.Sqrt((double)GetVariance());
		}

		public decimal GetStandardDeviation(string word)
		{
			if (!_wordDictionary._internalDictionary.ContainsKey(word)) { return noMatchValue; }
			return _wordDictionary._internalDictionary[word].GetStandardDeviation();
		}

		public string GetCollocations()
		{
			_wordDictionary.OrderInternalDictionary(SortCriteria.AbsoluteFrequency, SortDirection.Descending);

			StringBuilder result = new StringBuilder();

			IEnumerable<Word> searchWords = _wordDictionary.Words.Take(20);
			foreach (Word word in searchWords)
			{
				result.AppendFormat("{0}:{1}", word.Value, word._nextWordDictionary._internalDictionary.First().Key.Value);
				result.AppendLine();
			}

			return result.ToString(); ;
		}

		//private decimal GetWordPopularityScore(string word)
		//{
		//	if (_wordDictionary == null) { throw new ArgumentNullException("_wordDictionary"); }
		//	return _wordDictionary.GetVariance(word);
		//}
	}
}
