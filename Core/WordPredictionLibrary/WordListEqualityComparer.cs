using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary.Core
{
	public class WordListEqualityComparer : IEqualityComparer<List<string>>
	{
		public bool Equals(List<string> l, List<string> r)
		{
			if (l == null) return (r == null);
			else if (r == null) return false;

			if (!l.Any()) return (!r.Any());
			else if (!r.Any()) return false;
			else if (l.Count != r.Count) return false;

			int max = l.Count;
			int index = -1;
			while (++index < max)
			{
				if (!string.Equals(l[index], r[index], StringComparison.InvariantCultureIgnoreCase))
				{
					return false;
				}
			}
			return true;
		}

		public int GetHashCode(List<string> obj)
		{
			string stringRepresentation = string.Join(delimiter, obj);
			return stringRepresentation.GetHashCode();
		}

		private static string delimiter = char.MinValue.ToString();
	}
}
