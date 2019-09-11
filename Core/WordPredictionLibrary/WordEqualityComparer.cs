using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary.Core
{
	public class WordEqualityComparer : IEqualityComparer<Word>
	{
		public bool Equals(Word x, Word y)
		{
			return string.Equals(x.Value, y.Value, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(Word obj)
		{
			return obj.Value.GetHashCode();
		}
	}
}
