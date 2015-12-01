using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPredictionLibrary;

namespace WordPredictionLibrary
{
	public class TrainedDataSet
	{
		public long TotalWordsProcessed { get; private set; }
		public int UniqueWordsCataloged { get { return nextWordDictionary.Count; } }

		private static string AllowedChars = " .abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		
		private WordPredictionDictionary nextWordDictionary;

		public TrainedDataSet(string filename)
		{
			nextWordDictionary = new WordPredictionDictionary();
			
		}

		public void Train(FileInfo paragraphFile)
		{
			List<List<string>> paragraphs = ParseFile(paragraphFile.FullName);
						
			foreach (List<string> sentance in paragraphs)
			{
				nextWordDictionary.Train(sentance);
			}
								
		}

		private List<List<string>> ParseFile(string filename)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("Cannot parse file that does not exist", filename);
			}

			string documentBody = new string(File.ReadAllText(filename).Where(c => AllowedChars.Contains(c)).ToArray());
			List<string> sentences = documentBody.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
			
			List<List<string>> paragraphs = new List<List<string>>();
			foreach (string sentence in sentences)
			{
				List<string> words = sentence.Split(' ').ToList();
				TotalWordsProcessed += words.Count;
				paragraphs.Add(words);
			}

#if DEBUG
			string debugParsedFileFilename = string.Format("Debug.{0}.Parsed.{1}.txt", Path.GetFileNameWithoutExtension(filename), TotalWordsProcessed);
			FileInfo parsedFile = new FileInfo(debugParsedFileFilename).RenameIfExists();

			int counter = 1;
			
			foreach (List<string> sentance in paragraphs)
			{
				File.AppendAllText(parsedFile.FullName, string.Format("Sentence{0} = \"{1}.\";{2}", counter++, string.Join(" ", sentance), Environment.NewLine));
			}
#endif

			return paragraphs;
		}

		public void SerializeToFile(string filename)
		{
			// Part #2
			//		...
			StringBuilder serializedDataSet = new StringBuilder();

			serializedDataSet.AppendFormat("[TrainedDataSet:{0}",Environment.NewLine);
			serializedDataSet.AppendFormat("\tTotalWordsProcessed = {0}, ", TotalWordsProcessed);
			serializedDataSet.AppendFormat("\tUniqueWordsCataloged = {0}, ", UniqueWordsCataloged);
			serializedDataSet.AppendFormat("\tNextWordDictionary = [{0}\t\t{1}{0}\t]", Environment.NewLine, nextWordDictionary.ToString());
			serializedDataSet.AppendFormat("]{0}", Environment.NewLine);

			string debugNextWordDictionaryFilename = string.Format("Debug.{0}.NextWordDictionary.{1}.txt", Path.GetFileNameWithoutExtension(filename), nextWordDictionary.Count);
			FileInfo nextWordDictionary = new FileInfo(debugNextWordDictionaryFilename).RenameIfExists();
			File.AppendAllText(debugNextWordDictionaryFilename, serializedDataSet.ToString());
		}
	}		
}
