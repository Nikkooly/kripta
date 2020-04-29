using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WPFSerpent.Source.Models;

namespace ConsoleApp2
{
    class Program
    {
        public static string keyEncode;
        public static string keyDecode;
        static byte[] decryptedText;
        static byte[] encryptedText;
        static byte[] decrypted;
        static byte[] encrypted;
        public static string s = "";
        public static string keyForTwoFish = "Ярмолик";
        public static void readFile(string name)
        {
            StreamReader sr = new StreamReader(name);
            while (!sr.EndOfStream)
            {
                s += sr.ReadLine();
            }
            sr.Close();
        }
        public static void writeFile(string name,string text)
        {
            StreamWriter swg = new StreamWriter(name);
            swg.WriteLine(text);
            swg.Close();
            Process.Start(name);
        }
        static void Main(string[] args)
        {
            DESAlgorithm des = new DESAlgorithm();
            TripleDES tripleDES = new TripleDES();
            SerpentCipher serpent = new SerpentCipher();
            AESAlgorithm aes = new AESAlgorithm();
            Stopwatch sWatch = new Stopwatch();
            for (; ; )
            {
                Console.WriteLine("Выберите алгоритм шифрования");
                Console.WriteLine("1 Алгоритм DES");
                Console.WriteLine("2 Алгоритм DES3");
                Console.WriteLine("3 Алгоритм AES");
                Console.WriteLine("4 Алгоритм Twofish");
                Console.WriteLine("5 Алгоритм Serpent");
                int choose = Convert.ToInt32(Console.ReadLine());
                switch (choose)
                {
                    case 1:
                        {
                            s = "";
                            string key = "Безопасность";
                            sWatch.Start();
                            readFile("in.txt");
                            s = des.StringToRightLength(s);
                            des.CutStringIntoBlocks(s);
                            key = des.CorrectKeyWord(key, s.Length / (2 * des.Blocks.Length));
                            keyEncode = key;
                            key = des.StringToBinaryFormat(key);

                            for (int j = 0; j < des.quantityOfRounds; j++)
                            {
                                for (int i = 0; i < des.Blocks.Length; i++)
                                    des.Blocks[i] = des.EncodeDES_One_Round(des.Blocks[i], key);

                                key = des.KeyToNextRound(key);
                            }
                            key = des.KeyToPrevRound(key);
                            keyDecode = des.StringFromBinaryToNormalFormat(key);
                            string result = "";
                            for (int i = 0; i < des.Blocks.Length; i++)
                                result += des.Blocks[i];
                            writeFile("out1.txt", des.StringFromBinaryToNormalFormat(result));
                        };

                        {
                            s = "";
                            string key = des.StringToBinaryFormat(keyDecode);
                            readFile("out1.txt");
                            s = des.StringToBinaryFormat(s);

                            des.CutBinaryStringIntoBlocks(s);

                            for (int j = 0; j < des.quantityOfRounds; j++)
                            {
                                for (int i = 0; i < des.Blocks.Length; i++)
                                    des.Blocks[i] = des.DecodeDES_One_Round(des.Blocks[i], key);

                                key = des.KeyToPrevRound(key);
                            }

                            key = des.KeyToNextRound(key);

                            string results = "";

                            for (int i = 0; i < des.Blocks.Length; i++)
                                results += des.Blocks[i];

                            writeFile("out2.txt", des.StringFromBinaryToNormalFormat(results));
                        }
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 2:
                        sWatch.Start();
                        tripleDES.Apply3DES();
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 3:
                        sWatch.Start();
                        Console.WriteLine("Зашифрованный текст:");
                        aes.ToAes256();
                        aes.FromAes256(aes.ToAes256());
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 4:
                        sWatch.Start();
                        byte[] mmkey = Encoding.Unicode.GetBytes(keyForTwoFish);
                        var mtwM = new TwoFish(mmkey);
                        readFile("in.txt");                        
                        decryptedText = Encoding.Unicode.GetBytes(s);
                        encryptedText = mtwM.Encrypt(decryptedText, 0, decryptedText.Count());
                        writeFile("out1.txt", Encoding.Unicode.GetString(encryptedText));
                        Console.WriteLine("Зашифрованный текст");
                       // Console.WriteLine(Encoding.Unicode.GetString(encryptedText));
                        s = "";
                        readFile("out1.txt");
                        encrypted = Encoding.Unicode.GetBytes(Encoding.Unicode.GetString(encryptedText));
                        decrypted = mtwM.Decrypt(encryptedText, 0, encryptedText.Count());
                        Console.WriteLine("Расшифрованный текст");
                        Console.WriteLine(Encoding.Unicode.GetString(decrypted));
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 5:
                        sWatch.Start();
                        s = "";
                        string keys = "KN@= S]NXGŹHŹNT += ĘźeXvV |$~c";
                        readFile("in.txt");
                        serpent.Encrypt("in.txt", Encoding.Unicode.GetBytes(keys), 32, Mode.Standard, EncryptionMode.ECB);
                        serpent.Decrypt("in.serpent", Encoding.Unicode.GetBytes(keys), 32, Mode.Standard, EncryptionMode.ECB);
                        sWatch.Stop();
                        Console.WriteLine("Время выполнения");
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    default:
                        Console.WriteLine("Введите корректное число");
                        break;
                }
            }
        }
    }
}
