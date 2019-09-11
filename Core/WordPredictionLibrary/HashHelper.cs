using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary.Core
{
	public static class HashHelper
	{
		public static int GetHashCode(string input)
		{
			BigInteger total = CalculateNumericValue(input);

			UInt32 remainder = (UInt32)(total % new BigInteger(UInt32.MaxValue)); 

			Int32 result = (Int32)remainder; // Shove 32 bit unsigned value into 32 bit signed value (may be negative).
			return result;
		}

		private static BigInteger CalculateNumericValue(string input)
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
			return total;
		}

		private static BigInteger baseValue = new BigInteger(257);
	}
}
