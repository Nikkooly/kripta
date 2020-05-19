using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sWatch = new Stopwatch();
            for (; ; )
            {
                Console.WriteLine("Выберите операцию");
                Console.WriteLine("1. RC4");
                Console.WriteLine("2. RSA");
                Console.WriteLine("3 Эль-гамаль");
                Console.WriteLine("4 Линейный конгруэнтный генератор");
                int key = Convert.ToInt32(Console.ReadLine());
                switch (key)
                {
                    case 1:                      
                        sWatch.Start();
                        byte[] keys = ASCIIEncoding.ASCII.GetBytes("122");

                        RC4 encoder = new RC4(keys);
                        string testString = "Yarmolik Nickolay Sergeevich";
                        byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(testString);
                        byte[] result = encoder.Encode(testBytes, testBytes.Length);
                        string encryptedString = ASCIIEncoding.ASCII.GetString(result);
                        Console.WriteLine(encryptedString);
                        RC4 decoder = new RC4(keys);
                        byte[] decryptedBytes = decoder.Decode(result, result.Length);
                        string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);
                        Console.WriteLine(decryptedString);
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 2:
                        Console.WriteLine("Введите p (большое число)");
                        long ps = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Введите n (большое число)");
                        long n = Convert.ToInt32(Console.ReadLine());
                        sWatch.Start();
                        RSA rsa = new RSA();
                        rsa.Encrypt(ps, n);
                        rsa.Decrypt();
                       // RSA.Decrypt();
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 3:
                        sWatch.Start();
                        var strIn = "Ярмолик Николай Сергеевич";
                        var p = Numbers.GetNextPrimeAfter(1);
                        var g = Numbers.GetPRoot(p);
                        var x = Numbers.Rand(1, p - 1);
                        ElGamal elGamal = new ElGamal();
                        elGamal.Crypt(p, g, x, strIn);                       
                        elGamal.Decrypt(p, x);
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 4:
                        sWatch.Start();
                        testSpeedRand();
                        sWatch.Stop();
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        sWatch.Reset();
                        sWatch.Start();
                        testSpeedRandStandart();
                        sWatch.Stop();
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    default:
                        Console.WriteLine("Введите корректные данные");
                        break;
                }
            }
        }
        private static void testSpeedRandStandart()
        {
            var rnd = new Random();
            for (var n = 0; n < 100000000; n++)
                rnd.Next();
        }
        private static  void testSpeedRand()
        {
            var rnd = new Rnd();
            for (var n = 0; n < 100000000; n++)
                rnd.Next();
        }
    }
}
