using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class CharNum
    {
        private char _ch;
        private int _numberInWord;
        public char Ch
        {
            get { return _ch; }
            set
            {
                if (_ch == value)
                    return;
                _ch = value;
            }
        }
        public int NumberInWord
        {
            get { return _numberInWord; }
            set
            {
                if (_numberInWord == value)
                    return;
                _numberInWord = value;
            }
        }
    }
    class Program
    {
        private static string read = Environment.CurrentDirectory + @"\text.txt";
        private static string write = Environment.CurrentDirectory + @"\crypt.txt";
        private static int len = 25;
        private static string surname = "ярмолик";
        private static string name = "николай";
        private static string textFromFile;

        static async Task Main(string[] args)
        {
            string a = "Памагите пожалуйста очень нужно";
            using (FileStream fstream = File.OpenRead($"{read}"))
            {
                byte[] array = new byte[fstream.Length];
                await fstream.ReadAsync(array, 0, array.Length);
                textFromFile = System.Text.Encoding.Default.GetString(array);
            }
            Stopwatch sWatch = new Stopwatch();
            for (; ; )
            {
                Console.WriteLine("\nВыберите метод шифрования");
                Console.WriteLine("При использовании ноотбука к началу комбинации клавиш добавить Fn+");
                Console.WriteLine("Ctrl+F3 : Маршрутная перестановка по спирали");
                Console.WriteLine("Shift+F3 : Множественная перестановка");
                var key = Console.ReadKey();
                if (key.Modifiers.HasFlag(ConsoleModifiers.Control) && key.Key == ConsoleKey.F3)
                {
                    sWatch.Start();
                    DecryptRoute(RouteMethod(a.ToList<char>()));
                    sWatch.Stop();
                    Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                }
                else if (key.Modifiers.HasFlag(ConsoleModifiers.Shift) && key.Key == ConsoleKey.F3)
                {
                    sWatch.Start();
                    Transposition.DecryptMultiple(Transposition.MultipleMethod(a.ToList<char>(), surname, name),surname,name);
                    sWatch.Stop();
                    Console.WriteLine(sWatch.ElapsedMilliseconds.ToString() + "мс");
                }
            }
        }
        public static List<char> RouteMethod(List<char> alphabet)
        {
            Console.WriteLine("Размер алфавита: " + alphabet.Count);
            Console.WriteLine();

            if (Math.Sqrt(alphabet.Count) % 1 != 0)
            {
                while (Math.Sqrt(alphabet.Count) % 1 != 0)
                {
                    alphabet.Add(' ');
                }
            }
            double row = Math.Sqrt(alphabet.Count);

            for (int i = 0; i < alphabet.Count; i++)
            {
                Console.Write(alphabet[i] + "\0");
                if ((i + 1) % row == 0 && i > 1)
                {
                    Console.WriteLine();
                }
            }
            List<char> newAlphabet = new List<char>();
            int count = 0;

            while (newAlphabet.Count < alphabet.Count)
            {
                for (int i = count + (int)row * count; i < (alphabet.Count - row + 1) - row * count + count; i += (int)row)
                {
                    newAlphabet.Add(alphabet[i]);
                }

                for (int i = (alphabet.Count - (int)row + 1) - (int)row * count + count; i < alphabet.Count - row * count - count; i++)
                {
                    newAlphabet.Add(alphabet[i]);
                }

                for (int i = alphabet.Count - 1 - (int)row - (int)row * count - count; i > row * count; i = i - (int)row)
                {
                    newAlphabet.Add(alphabet[i]);
                }

                if (newAlphabet.Count < alphabet.Count)
                {
                    for (int i = (int)row * count + (int)row - 1 - 1 - count; i > row * count + count; i--)
                    {
                        newAlphabet.Add(alphabet[i]);
                    }
                }
                count++;
            }
            Console.Write("Зашифрованное сообщение: ");
            foreach (char elem in newAlphabet)
            {
                Console.Write(elem);
            }
            Console.WriteLine("\n---------------------------------");
            return newAlphabet;
        }

        public static void DecryptRoute(List<char> alphabet)
        {
            List<char> baseAlphabet = new List<char>();
            for (int i = 0; i < alphabet.Count; i++)
            {
                baseAlphabet.Add('-');
            }

            double row = Math.Sqrt(alphabet.Count);
            int count = 0;
            int baseCount = 0;

            while (count < alphabet.Count)
            {
                for (int i = baseCount + (int)row * baseCount; i < (baseAlphabet.Count - row + 1) - row * baseCount + baseCount; i += (int)row)
                {
                    baseAlphabet[i] = alphabet[count];
                    count++;
                }

                for (int i = (baseAlphabet.Count - (int)row + 1) - (int)row * baseCount + baseCount; i < baseAlphabet.Count - row * baseCount - baseCount; i++)
                {
                    baseAlphabet[i] = alphabet[count];
                    count++;
                }

                for (int i = baseAlphabet.Count - 1 - (int)row - (int)row * baseCount - baseCount; i > row * baseCount; i = i - (int)row)
                {
                    baseAlphabet[i] = alphabet[count];
                    count++;
                }

                for (int i = (int)row * baseCount + (int)row - 1 - 1 - baseCount; i > row * baseCount + baseCount; i--)
                {
                    baseAlphabet[i] = alphabet[count];
                    count++;
                }
                baseCount++;
            }

            for (int i = 0; i < baseAlphabet.Count; i++)
            {
                Console.Write(baseAlphabet[i] + "\0");
                if ((i + 1) % row == 0 && i > 1)
                {
                    Console.WriteLine();
                }
            }
            Console.Write("Расшифрованное сообщение: ");
            foreach (char elem in baseAlphabet)
            {
                Console.Write(elem);
            }
        }
    }
}
