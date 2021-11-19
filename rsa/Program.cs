using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace rsa
{
	class Program
	{
		static void Main(string[] args)
		{
			//BigInteger p = 31;
			//BigInteger q = 19;
			//BigInteger p = 1583;
			//BigInteger q = 1609;
			BigInteger p = 1823;
			BigInteger q = 1879;
			var n = p * q;
			var fiN = (p - 1) * (q - 1);

			var collectionOfE = GenerateEnumbers(n, fiN).ToList();
			var random = new Random();
			//var e = collectionOfE[random.Next(0, collectionOfE.Count - 1)];

			var dList = new List<int>();
			var eList = new List<int>(collectionOfE);

			foreach (var ee in collectionOfE)
			{
				bool found = false;
				for (int dd = 3; dd < fiN; dd++)
				{
					//Console.WriteLine($"d: {d}, e: {e}, de: {d*e} mod: {d*e%fiN} ");
					if (ee * dd % fiN == 1)
					{
						dList.Add(dd);
						found = true;
						break;
					}
				}
				if (!found)
					eList.Remove(ee);
			}

			//var collectionOfD = GenerateDnumbers(fiN, collectionOfE).ToList();
			var min = n;
			int d = 0;
			int e = 0;
			for (int i = 0; i < dList.Count; i++)
			{
				if (dList[i] < min)
				{
					min = dList[i];
					d = dList[i];
					e = eList[i];
				}
			}
			Console.WriteLine($"input message to encrypt, p: {p}, q: {q}, e: {e}, d: {d}");
			//var input = Console.ReadLine();

			//var input = File.ReadAllText("input30.txt");
			var input = File.ReadAllText("input50.txt");


			var encryptedMessage = EncryptMessage(input, e, n);

			//foreach (var singleLetter in encryptedMessage)
			//	Console.Write($"{singleLetter} ");

			var decryptedMessage = DecryptMessage(encryptedMessage, d, n);
			Console.WriteLine($"\noutput: {decryptedMessage}");
		}

		//private static IEnumerable<int> GenerateDnumbers(BigInteger fiN, List<int> eList)
		//{
		//	var tempList = new List<int>();
		//	var tempeList = new List<int>();
			
		//	foreach (var e in eList)
		//	{
		//		bool found = false;
		//		for (int d = 3; d < fiN; d++)
		//		{
		//			//Console.WriteLine($"d: {d}, e: {e}, de: {d*e} mod: {d*e%fiN} ");
		//			if (e * d % fiN == 1)
		//			{
		//				tempList.Add(d);
		//				found = true;
		//				continue;
		//			}
		//			if (!found)
		//				tempeList.Remove(e);
		//		}
		//	}
		//}


		private static IEnumerable<int> GenerateEnumbers(BigInteger n, BigInteger fiN)
		{
			for (int i = 3; i < Math.Sqrt((double) n); i++)
			{
				if (IsPrime(i) && fiN % i != 0)
					yield return i;
			}
		}

		private static bool IsPrime(int number)
		{
			bool CalculatePrime(int value)
			{
				var possibleFactors = Math.Sqrt(number);
				for (var factor = 2; factor <= possibleFactors; factor++)
				{
					if (value % factor == 0)
					{
						return false;
					}
				}
				return true;
			}
			return number > 1 && CalculatePrime(number);
		}

		private static IEnumerable<BigInteger> EncryptMessage(string input, int e, BigInteger n)
		{
			foreach (var singleByte in Encoding.ASCII.GetBytes(input))
				yield return BigInteger.Pow(singleByte, e) % n;
		}

		private static string DecryptMessage(IEnumerable<BigInteger> encryptedMessage, int d, BigInteger n)
		{
			var decryptedMessage = "";
			int i = 0;
			foreach (var singleLetter in encryptedMessage)
			{
				Console.WriteLine($"i: {i++}, encMessLen: {encryptedMessage.Count()}");
				var decryptedLetter = BigInteger.Pow(singleLetter, d) % n;
				decryptedMessage += (char) decryptedLetter;
			}
			return decryptedMessage;
		}
	}
}
