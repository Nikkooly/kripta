using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********************Task А********************");
            Console.ForegroundColor = ConsoleColor.White;
            EntropyPolish entropyEnglish = new EntropyPolish();
            entropyEnglish.EntropyPolishAlphabet();
            Console.ForegroundColor = ConsoleColor.Magenta;


            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********************Task Binary********************");
            Console.ForegroundColor = ConsoleColor.White;
            EntropyBinaryDigit entropyBinaryDigit = new EntropyBinaryDigit();
            entropyBinaryDigit.EntropyBinaryAlphabet();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********************Task В********************");
            Console.ForegroundColor = ConsoleColor.White;
            AmountOfInformation amountOfInformation = new AmountOfInformation();
            amountOfInformation.CountAmountOfInformation();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********************Task Г********************");
            Console.ForegroundColor = ConsoleColor.White;
            ErroneousTransmission erroneousTransmission = new ErroneousTransmission();
            erroneousTransmission.ErroneousTransmissionMethod();

            Console.ReadKey();
        }

        static double TheAlphabet = 0;
        static double BinaryAlphabet = 0;

        class EntropyPolish
        {
            public void EntropyPolishAlphabet()
            {
                int[] countLetter = new int[32];
                int countLettersInFile = 0;
                double[] probabilityLetters = new double[32];
                //double TheAlphabet = 0;
                string letters = "aąbcćdeęfghijklłmnńoóprsśtuwyzźż";
                using (StreamReader streamReader = new StreamReader(@"F:\Уник\3-2\берник\lab2\lab2pl.txt"))
                {
                    string file = streamReader.ReadToEnd().ToLower().Replace("\r", "").Replace("\n", "").Replace(" ", "");
                    file = Regex.Replace(file, @"\W+", "");
                    countLettersInFile = file.Count();

                    Console.WriteLine($"Количество символов в файле: {countLettersInFile}");
                    Console.WriteLine();
                    Console.WriteLine("Количество вхождений каждой буквы:");
                    for (int i = 0; i < file.Length; i++)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            countLetter[j] = file.Count(x => x == letters[j]);
                            Console.WriteLine($"{ letters[j]}: { countLetter[j]}");

                            probabilityLetters[j] = (double)countLetter[j] / countLettersInFile;
                            Console.WriteLine($"P({letters[j]}): {probabilityLetters[j]}");
                            Console.WriteLine();

                            TheAlphabet += probabilityLetters[j] * (Math.Log(probabilityLetters[j], 2)) * (-1);
                        }
                        Console.WriteLine("Энтропия алфавита по Шеннону:");
                        Console.WriteLine(TheAlphabet);
                        break;
                    }
                }
            }
        }

        class EntropyBinaryDigit
        {
            public void Read(String road)
            {
                var bytes = Encoding.ASCII.GetBytes(road);
                var h = string.Join(" ", bytes.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
                using (var streamWriter = new StreamWriter("bin.txt"))
                {
                    streamWriter.Write(h.ToCharArray());
                }
            }
            public void EntropyBinaryAlphabet()
            {
                int[] countLetter = new int[2];
                int countLettersInFile = 0;
                double[] probabilityLetters = new double[2];
                // double BinaryAlphabet = 0;
                string letters = "01";
                using (StreamReader streamReader = new StreamReader(@"F:\Уник\3-2\берник\lab2\lab2pl.txt"))
                {
                    string file1 = streamReader.ReadToEnd().Replace("\r", "").Replace("\n", "");
                    Read(file1);
                }
                using (StreamReader streamReader = new StreamReader(@"bin.txt"))
                {
                    string file = streamReader.ReadToEnd().Replace("\r", "").Replace("\n", "");
                    countLettersInFile = file.Count();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Binary Digit");
                    Console.WriteLine($"Количество символов в файле: {countLettersInFile}");
                    Console.WriteLine();
                    Console.WriteLine("Количество вхождений каждой цифры:");
                    for (int i = 0; i < file.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            countLetter[j] = file.Count(x => x == letters[j]);
                            Console.WriteLine($"{ letters[j]}: { countLetter[j]}");
                            probabilityLetters[j] = (double)countLetter[j] / countLettersInFile;
                            Console.WriteLine($"P({letters[j]}): {probabilityLetters[j]}");
                            Console.WriteLine();
                            BinaryAlphabet += (probabilityLetters[j] * (Math.Log(probabilityLetters[j], 2))) * (-1);
                        }
                        Console.WriteLine("Энтропия бинарного алфавита по Шеннону:");
                        Console.WriteLine(BinaryAlphabet);
                       break;
                    }
                }
            }
        }
   

        class AmountOfInformation
        {
            public void CountAmountOfInformation()
            {
                Console.WriteLine("Введите Ваше ФИО: ");
                string FIO = Console.ReadLine();
                double countEnglishInformation = TheAlphabet * FIO.Replace(" ", "").ToLower().Count();
                Console.WriteLine("Количество информации с использованием энтропии польского алфавита:");
                Console.WriteLine(countEnglishInformation);
                Console.WriteLine();
                double ascii = FIO.ToLower().Count() * 8;
                Console.WriteLine();
                Console.WriteLine("Количество информации с использованием ANSII для польского алфавита:");
                Console.WriteLine(ascii);
                Console.WriteLine();
            }
        }


        class ErroneousTransmission
        {
            public void ErroneousTransmissionMethod()
            {
                for(double i = 0.1; i < 1; i+=0.1)
                {
                    Console.WriteLine();
                    double p = i;
                    double q = 1 - p;
                    Console.WriteLine($"Веростяность ошибочной передачи = {i}", i) ;
                    double conditionalEntropy = -p * (Math.Log(p) / Math.Log(2)) - q * (Math.Log(q) / Math.Log(2));
                    Console.WriteLine($"Условная энтропия = {conditionalEntropy}");
                    double effectiveEntropy = 1 - conditionalEntropy;
                    Console.WriteLine($"Эффективная энтропия = {effectiveEntropy}");
                }
            }
            
        }
    }
}
