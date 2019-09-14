using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace WordPredictionLibrary.Core
{
	public static class FileInfoExtensionMethods
	{
		public static FileInfo RenameIfExists(this FileInfo source)
		{
			if (source != null)
			{
				int counter = 1;
				string newFilename = source.FullName;

				while (File.Exists(newFilename))
				{
					newFilename = Path.Combine(source.DirectoryName, string.Format("{0} ({1}){2}", Path.GetFileNameWithoutExtension(source.Name), counter++, Path.GetExtension(source.Name)));
				}
				return new FileInfo(newFilename);
			}
			return default(FileInfo);
		}
	}

	public static class StringExtensionMethods
	{
		public static string TryToLower(this string source)
		{
			if (string.IsNullOrWhiteSpace(source))
			{
				return "";
			}
			else
			{
				return source.ToLowerInvariant();
			}
		}
	}

	public static class StringBuilderExtensionMethods
	{
		public static bool Contains(this StringBuilder source, string value)
		{
			if (source == null || string.IsNullOrWhiteSpace(value))
			{
				return false;
			}

			string sourceString = source.ToString();
			if (string.IsNullOrWhiteSpace(sourceString))
			{
				return false;
			}

			return sourceString.Contains(value);
		}
	}

	public static class DictionaryExtensionMethods
	{
		public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderDictionaryBy<TKey, TValue, TSortKey>(this Dictionary<TKey, TValue> source, Func<KeyValuePair<TKey, TValue>, TSortKey> keySelector, SortDirection sortDirection)
		{
			if (sortDirection == SortDirection.Ascending)
			{
				return source.OrderBy(keySelector);
			}
			else
			{
				return source.OrderByDescending(keySelector);
			}
		}
	}
}
