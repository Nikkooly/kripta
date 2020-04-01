using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{
    class Program
    {
        private static string keyWords= "безопаcность";
        private static string textFromFile;
        private static string file;
        private static string ress,deress;
        private static string message,messages;
        
        private static string read = Environment.CurrentDirectory + @"\text.txt";
        private static string write= Environment.CurrentDirectory + @"\crypt.txt";
        private static string trisemus = Environment.CurrentDirectory + @"\trisemus.txt";
        class EntropyRussian
        {
            public async Task EntropyRussianAlphabetAsync(String path)
            {
                int[] countLetter = new int[33];
                int countLettersInFile = 0;
                double[] probabilityLetters = new double[33];
                //double TheAlphabet = 0;
                string letters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
                using (FileStream fstream = File.OpenRead($"{write}"))
                {
                    byte[] array = new byte[fstream.Length];
                    await fstream.ReadAsync(array, 0, array.Length);

                    file = System.Text.Encoding.Default.GetString(array).ToLower().Replace("\t", "");
                }
                    Console.WriteLine(file);
                    file = Regex.Replace(file, @"\W+", "");
                    countLettersInFile = file.Count();

                    Console.WriteLine($"Количество символов в файле: {countLettersInFile}");
                    Console.WriteLine();
                    Console.WriteLine("Количество вхождений каждой буквы:");
                    for (int i = 0; i < file.Length; i++)
                    {
                        for (int j = 0; j < 33; j++)
                        {
                            countLetter[j] = file.Count(x => x == letters[j]);
                            Console.WriteLine($"{ letters[j]}: { countLetter[j]}");

                            probabilityLetters[j] = (double)countLetter[j] / countLettersInFile;
                            Console.WriteLine($"P({letters[j]}): {probabilityLetters[j]}");
                            Console.WriteLine();
                        }
                        break;
                    }
            }
        }
        public static double ShannonEntropyString(string s)
        {
            var map = new Dictionary<char, int>();
            foreach (char c in s)
            {
                if (!map.ContainsKey(c))
                    map.Add(c, 1);
                else
                    map[c] += 1;
            }

            double result = 0.0;
            int len = s.Length;
            foreach (var item in map)
            {
                var frequency = (double)item.Value / len;
                result -= frequency * (Math.Log(frequency, 2));
            }

            return result;
        }
        public static void ShannonEnthropyFile(string path, string alphabet)
        {
            string s;
            int[] freq = new int[25600];
            for (int i = 0; i < 25600; i++)
            {
                freq[i] = 0;
            }           
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                s = Encoding.Default.GetString(array).ToLower();
            }

            foreach (var c in s)
            {
                freq[c]++;
            }

            int totalCount = s.Length;
            double result = 0.0;
            double frequency;
            foreach (var letter in alphabet)
            {
                Console.Write(letter);
                Console.Write(" => " + freq[letter]);
                frequency = (double)freq[letter] / totalCount;
                Console.WriteLine(" => " + frequency);
                if (frequency != 0)
                {
                    result -= frequency * (Math.Log(frequency, 2));
                }
            }
            Console.WriteLine("Total letters: " + totalCount);

        }
        static async Task Main(string[] args)
        {
            Stopwatch sWatch = new Stopwatch();
            for (; ; )
            {

                Console.Write($"Ключевое слово: {keyWords} ", keyWords);
                Console.WriteLine(" \n Выберите тип шифрования:");
                Console.WriteLine("1. Шифр Цезаря с ключевым словом");
                Console.WriteLine("2. Таблица Трисемуса");
                Console.WriteLine("3. Частота появления символов");
                int choose = Convert.ToInt32(Console.ReadLine());
                
                switch (choose)
                {
                    case 1:
                        Console.Write("Ключ: ");
                        int key = Convert.ToInt32(Console.ReadLine());
                        sWatch.Start();
                        CaesarWithKnife.createNewAlpha(keyWords, key);
                        Console.WriteLine();
                      Console.WriteLine("Шифрованный алфавит: " + CaesarWithKnife.getNewAlpha());
                        Console.WriteLine();
                        using (FileStream fstream = File.OpenRead($"{read}"))
                        {
                            byte[] array = new byte[fstream.Length];
                            await fstream.ReadAsync(array, 0, array.Length);
                            textFromFile = System.Text.Encoding.Default.GetString(array);
                            Console.WriteLine($"Текст из файла: {textFromFile}");

                        }                                             
                        string open = "", close = "";
                        Console.Write("--------------------------Текст зашифрован--------------------------------------");
                        open = textFromFile.ToLower();
                        close = CaesarWithKnife.encrypt(open);
                        Console.WriteLine();
                        Console.WriteLine("Шифрованное сообщение: " + close);
                        using (FileStream fstream = new FileStream($"{write}", FileMode.OpenOrCreate))
                        {
                            byte[] array = System.Text.Encoding.Default.GetBytes(close);
                            await fstream.WriteAsync(array, 0, array.Length);
                            Console.WriteLine("Текст записан в файл");
                        }
                        Console.Write("--------------------------Расшифрока текста--------------------------------------");
                        using (FileStream fstream = File.OpenRead($"{write}"))
                        {
                            byte[] array = new byte[fstream.Length];
                            await fstream.ReadAsync(array, 0, array.Length);

                            textFromFile = System.Text.Encoding.Default.GetString(array);
                        }
                        open = CaesarWithKnife.decrypt(textFromFile);
                        Console.WriteLine();
                        Console.WriteLine("Расшифрованное сообщение: " + open);
                        sWatch.Stop();
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString()+"мс");
                        break;
                    case 2:
                        sWatch.Start();
                        char[] alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ".ToCharArray();
                        int rows = 11, columns=3;
                        String ke = "безопасность";
                        char[] keyWord = ke.ToUpper().Distinct().ToArray();
                        var table = new char[rows, columns];

                        // Вписываем в нее ключевое слово
                        for (var i = 0; i < keyWord.Length; i++)
                        {
                            table[i / columns, i % columns] = keyWord[i];
                        }

                        // Исключаем уникальные символы ключевого слова из алфавита
                        alphabet = alphabet.Except(keyWord).ToArray();

                        // Вписываем алфавит
                        for (var i = 0; i < alphabet.Length; i++)
                        {
                            int position = i + keyWord.Length;
                            table[position / columns, position % columns] = alphabet[i];
                        }

                        // Получаем сообщение, которое необходимо зашифровать
                        using (FileStream fstream = File.OpenRead($"{read}"))
                        {
                            byte[] array = new byte[fstream.Length];
                            await fstream.ReadAsync(array, 0, array.Length);

                           message = System.Text.Encoding.Default.GetString(array).ToUpper();
                        }
                        StringBuilder sb = new StringBuilder();
                        for (int l = 0; l < message.Length - l; l += 255)
                        {
                            message.Substring(l, l + 255);
                            var result = new char[message.Length];
                            // Шифруем сообщение
                            for (var k = 0; k < message.Length; k++)
                            {
                                char symbol = message[k];
                                // Пытаемся найти символ в таблице
                                for (var i = 0; i < rows; i++)
                                {
                                    for (var j = 0; j < columns; j++)
                                    {
                                        if (symbol == table[i, j])
                                        {
                                            symbol = table[(i + 1) % rows, j]; // Смещаемся циклически на следующую строку таблицы и запоминаем новый символ
                                            i = rows; // Завершаем цикл по строкам
                                            break; // Завершаем цикл по колонкам
                                        }
                                    }
                                }
                                // Записываем найденный символ
                                result[k] = symbol;
                                
                            }
                            ress=sb.Append(result).ToString();                          
                            break;
                        }
                        using (FileStream fstream = new FileStream($"{trisemus}", FileMode.OpenOrCreate))
                        {
                            byte[] array = System.Text.Encoding.Default.GetBytes(ress);
                            await fstream.WriteAsync(array, 0, array.Length);
                            Console.WriteLine("Текст записан в файл");
                        }
                        Console.WriteLine("Зашифрованное сообщение: " + sb.ToString());
                        using (FileStream fstream = File.OpenRead($"{trisemus}"))
                        {
                            byte[] array = new byte[fstream.Length];
                            await fstream.ReadAsync(array, 0, array.Length);

                            messages = System.Text.Encoding.Default.GetString(array);
                        }
                        StringBuilder sbs = new StringBuilder();
                        for (int m = 0; m < messages.Length - m; m += 255)
                        {
                            messages.Substring(m, m + 255);                         
                            var results = new char[messages.Length];
                            // Шифруем сообщение
                            for (var k = 0; k < messages.Length; k++)
                            {
                                char symbol = messages[k];
                                // Пытаемся найти символ в таблице
                                for (var i = 0; i < rows; i++)
                                {
                                    for (var j = 0; j < columns; j++)
                                    {
                                        if (symbol == table[i, j])
                                        {
                                            symbol = table[(i + 10) % rows, j];
                                            i = rows; // Завершаем цикл по строкам
                                            break; // Завершаем цикл по колонкам
                                        }
                                    }
                                }
                                // Записываем найденный символ
                                results[k] = symbol;
                            }
                            deress = sbs.Append(results).ToString();
                            break;                          
                        }

                        Console.WriteLine("\n");
                        Console.WriteLine("Дешифрованное сообщение: " + deress);
                        sWatch.Stop();
                        Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                        break;
                    case 3:
                        string engAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
                        Console.WriteLine("Энтропия файла");
                        ShannonEnthropyFile($"{read}", engAlphabet);
                        Console.WriteLine("Энтропия файла по Цезарю с КС");
                        ShannonEnthropyFile($"{write}", engAlphabet);
                        Console.WriteLine("Энтропия файла таблицей Трисемуса с КС");
                        ShannonEnthropyFile($"{trisemus}", engAlphabet);
                        //Console.WriteLine("Энтропия исходного сообщения");
                        //EntropyRussian entropyRussian = new EntropyRussian();
                        //await entropyRussian.EntropyRussianAlphabetAsync($"{read}");
                       // Console.WriteLine("Энтропия зашифрованного сообщения по Цезарю с КС");
                        //EntropyRussian entropy = new EntropyRussian();
                        //await entropy.EntropyRussianAlphabetAsync(@"F:\Уник\3-2\берник\lab4\ConsoleApp2\ConsoleApp2\bin\Debug\text.txt");
                        //Console.WriteLine("Энтропия зашифрованного сообщения таблицей Трисемуса с КС");
                        //await entropyRussian.EntropyRussianAlphabetAsync($"{trisemus}");
                        break;
                    default:
                        break;
                }
                
            }
            
        }
    }
}
