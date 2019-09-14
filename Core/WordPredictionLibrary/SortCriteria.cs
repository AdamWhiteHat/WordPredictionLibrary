using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary.Core
{
	public enum SortCriteria
	{
		AbsoluteFrequency,
		NextWordCount,
		Variance,
		StandardDeviation
	}

	public enum SortDirection
	{
		Ascending,
		Descending
	}

	public delegate IOrderedEnumerable<TSource> OrderByDelegate<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
}
