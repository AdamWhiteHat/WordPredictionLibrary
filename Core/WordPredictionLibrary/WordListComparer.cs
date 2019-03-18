using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary.Core
{
	public class WordListComparer : IEqualityComparer<List<string>>
	{
		public bool Equals(List<string> l, List<string> r)
		{
			if (l.Count != r.Count)
			{
				return false;
			}
			int max = l.Count;
			int index = 0;

			while (index < max)
			{
				if (l[index] != r[index])
				{
					return false;
				}
				index++;
			}
			return true;
		}

		public int GetHashCode(List<string> obj)
		{
			string stringRepresentation = string.Join("|", obj);
			return CalculateNumericValue(stringRepresentation);
		}

		private static BigInteger baseValue = new BigInteger(257);
		internal static int CalculateNumericValue(string input)
		{
			int counter = 0;
			BigInteger total = new BigInteger(0);
			BigInteger placeValue = new BigInteger(0);
			byte[] inputBytes = Encoding.UTF8.GetBytes(input);
			foreach (byte bite in inputBytes)
			{
				placeValue = BigInteger.Pow(baseValue, counter++);
				total = total + (placeValue * bite);
			}

			UInt32 remainder = (UInt32)(total % new BigInteger(UInt32.MaxValue));
			Int32 result = (Int32)remainder;						
			return result;
		}

		public static string ToBitString(UInt32 value)
		{
			return string.Join(" ", BitConverter.GetBytes(value).Reverse().Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
		}

		public static string ToBitString(Int32 value)
		{
			return string.Join(" ", BitConverter.GetBytes(value).Reverse().Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
		}
	}
}
