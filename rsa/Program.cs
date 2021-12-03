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
			BigInteger p = 25939;
			BigInteger q = 26557;
			//BigInteger p = 1823;
			//BigInteger q = 1879;
			//BigInteger p = 3671;
			//BigInteger q = 3673;
			BigInteger n = p * q;
			BigInteger fiN = (p - 1) * (q - 1);

			Console.WriteLine("type: 'e' to encrypt with public key, 'd' to encrypt with private key ");
			var option = "";
			while (true)
			{
				option = Console.ReadLine();
				if (option == "e" || option == "d")
					break;
				Console.WriteLine("invalid data provided");
			}

			var collectionOfE = GenerateEandD(n, fiN);
			BigInteger encryptKey, decryptKey;
				;

			if (option == "e")
			{
				encryptKey = collectionOfE.LastOrDefault().Key;
				decryptKey = collectionOfE.LastOrDefault().Value;
			}
			else
			{
				encryptKey = collectionOfE.LastOrDefault().Value;
				decryptKey = collectionOfE.LastOrDefault().Key;
			}



			Console.WriteLine($"input message to encrypt, p: {p}, q: {q}, e: {encryptKey}, d: {decryptKey}");
			var input = File.ReadAllText(fileName20Length);
			//var input = File.ReadAllText(fileName50Length);
			var encryptedMessage = EncryptMessage(input, decryptKey, n);
			
			File.Delete(decrypted);
			File.Delete(encrypted);
			foreach (var singleLetter in encryptedMessage)
				File.AppendAllText(encrypted, $"{singleLetter} ");

			var decryptedMessage = DecryptMessage(encryptedMessage, encryptKey, n);
			File.WriteAllText(decrypted, decryptedMessage);
			Console.WriteLine($"\noutput: {decryptedMessage}");
		}

		public static BigInteger nwd(BigInteger a, BigInteger b)
		{
			BigInteger x = a, y = b;
			while (x != y)
			{
				if (x > y)
					x = x - y;
				else
					y = y - x;
			}
			return x;
		}

		private static Dictionary<BigInteger, BigInteger> GenerateEandD(BigInteger n, BigInteger fiN)
		{
			var result = new Dictionary<BigInteger, BigInteger>();
			int j = 0;
			for (var i = 3; i < 100; i= i+2)
			{
				if (IsPrime(i) && nwd(i, fiN) == 1)
				{
					var d = GenerateDNumber(i, fiN);
					if (d != null)
					{
						result.Add(i, (BigInteger)d);
						j++;
					}
				}
				if(j == 2)
					break;
			}

			return result;
		}

		private static int? GenerateDNumber(int e, BigInteger fiN)
		{
			for (var dd = 3; dd < fiN; dd++)
			{
				if (e * dd % fiN == 1)
				{
					return dd;
				}
			}

			return null;
		}

		private static bool IsPrime(BigInteger number)
		{
			bool CalculatePrime(BigInteger value)
			{
				var possibleFactors = Math.Sqrt((double)number);
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

		private static IEnumerable<BigInteger> EncryptMessage(string input, BigInteger e, BigInteger n)
		{
			foreach (var singleByte in Encoding.ASCII.GetBytes(input))
			{
				BigInteger wynik = 1;
				var x = singleByte % n;
				var byteValue = Convert.ToString((int)e, 2);
				for (int j = byteValue.Length - 1; j >= 0; j--)
				{
					if (byteValue[j] == '0')
					{
						x = x * x % n;
					}
					else
					{
						wynik = wynik * x % n;
						x = x * x % n;
					}
				}
				yield return wynik;
			}
		}

		private static string DecryptMessage(IEnumerable<BigInteger> encryptedMessage, BigInteger d, BigInteger n)
		{
			var decryptedMessage = "";
			int i = 0;
			foreach (var singleLetter in encryptedMessage)
			{
				BigInteger wynik = 1;
				var x = singleLetter % n;
				var byteValue = Convert.ToString((int)d, 2);
				for (int j = byteValue.Length - 1; j >= 0; j--)
				{
					if (byteValue[j] == '0')
					{
						x = x * x % n;
					}
					else
					{
						wynik = wynik * x % n;
						x = x * x% n;
					}
				}

				//Console.WriteLine($"i: {i++}, sing: {singleLetter}, dec: {decryptedLetter}");
				decryptedMessage += (char) wynik;
			}
			return decryptedMessage;
		}

		private static string fileName20Length = "../../../input20.txt";
		private static string fileName50Length = "../../../input50.txt";
		private static string encrypted = "../../../encrypted.txt";
		private static string decrypted = "../../../decrypted.txt";

	}
}
