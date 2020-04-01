using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    static class Transposition
    {
        
        public static char[,] MultipleMethod(List<char> baseAlphabet, string surname, string name)
        {
            char[] keyOne = surname.ToCharArray();
            char[] keyTwo = name.ToCharArray();
            if (Math.Sqrt(baseAlphabet.Count) % 1 != 0)
            {
                while (baseAlphabet.Count < keyOne.Length * keyTwo.Length)
                {
                    baseAlphabet.Add(' ');
                }
            }
            double row = Math.Sqrt(baseAlphabet.Count);
            char[,] alphabet = new char[(int)row, (int)row];
            int count = 0;

            Console.Write("   |");
            for (int i = 0; i < keyOne.Length; i++)
            {
                Console.Write(keyOne[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------");
            char[] oneKey = new char[keyOne.Length];
            char[] twoKey = new char[keyTwo.Length];
            for (int i = 0; i < keyOne.Length; i++)
            {
                oneKey[i] = keyOne[i];
            }

            for (int i = 0; i < keyTwo.Length; i++)
            {
                twoKey[i] = keyTwo[i];
            }

            for (int i = 0; i < alphabet.GetLength(1); i++)
            {
                Console.Write(keyTwo[i] + "  |");
                for (int j = 0; j < alphabet.GetLength(0); j++)
                {
                    alphabet[i, j] = baseAlphabet[count];
                    Console.Write(alphabet[i, j] + " ");
                    count++;
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine();
            Array.Sort(keyOne);
            Array.Sort(keyTwo);
            char[,] alphabetSecond = new char[(int)row, (int)row];
            char[,] alphabetFinish = new char[(int)row, (int)row];

            for (int i = 0; i < alphabetSecond.GetLength(0); i++)
            {
                for (int j = 0; j < alphabetSecond.GetLength(1); j++)
                {
                    alphabetSecond[i, j] = alphabet[i, Array.IndexOf(oneKey, keyOne[j])];
                }
            }

            for (int i = 0; i < alphabetFinish.GetLength(0); i++)
            {
                for (int j = 0; j < alphabetFinish.GetLength(1); j++)
                {
                    alphabetFinish[i, j] = alphabetSecond[Array.IndexOf(twoKey, keyTwo[i]), j];
                }
            }

            Console.Write("   |");
            for (int i = 0; i < keyOne.Length; i++)
            {
                Console.Write(keyOne[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------");

            List<char> encryptMessage = new List<char>();
            for (int i = 0; i < alphabetFinish.GetLength(0); i++)
            {
                Console.Write(keyTwo[i] + "  |");
                for (int j = 0; j < alphabetFinish.GetLength(1); j++)
                {
                    encryptMessage.Add(alphabetFinish[i, j]);
                    Console.Write(alphabetFinish[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.Write("Зашифрованное сообщение: ");
            foreach (char elem in encryptMessage)
            {
                Console.Write(elem);
            }
            Console.WriteLine();
            Console.WriteLine();
            return alphabetFinish;
        }
        public static void DecryptMultiple(char[,] encryptMessage, string surname,string name)
        {
            char[] keyOne = surname.ToCharArray();
            char[] keyTwo = name.ToCharArray();
            char[] oneKey = new char[keyOne.Length];
            char[] twoKey = new char[keyTwo.Length];
            for (int i = 0; i < keyOne.Length; i++)
            {
                oneKey[i] = keyOne[i];
            }

            for (int i = 0; i < keyTwo.Length; i++)
            {
                twoKey[i] = keyTwo[i];
            }
            Array.Sort(keyOne);
            Array.Sort(keyTwo);
            char[,] alphabetSecond = new char[encryptMessage.GetLength(1), encryptMessage.GetLength(1)];
            char[,] alphabetFinish = new char[encryptMessage.GetLength(1), encryptMessage.GetLength(1)];

            for (int i = 0; i < alphabetSecond.GetLength(0); i++)
            {
                for (int j = 0; j < alphabetSecond.GetLength(1); j++)
                {
                    alphabetSecond[i, j] = encryptMessage[i, Array.IndexOf(keyOne, oneKey[j])];
                }
            }

            for (int i = 0; i < alphabetFinish.GetLength(0); i++)
            {
                for (int j = 0; j < alphabetFinish.GetLength(1); j++)
                {
                    alphabetFinish[i, j] = alphabetSecond[Array.IndexOf(keyTwo, twoKey[i]), j];
                }
            }

            Console.Write("   |");
            for (int i = 0; i < oneKey.Length; i++)
            {
                Console.Write(oneKey[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------");
            List<char> decryptMessage = new List<char>();
            for (int i = 0; i < alphabetFinish.GetLength(0); i++)
            {
                Console.Write(twoKey[i] + "  |");
                for (int j = 0; j < alphabetFinish.GetLength(1); j++)
                {
                    decryptMessage.Add(alphabetFinish[i, j]);
                    Console.Write(alphabetFinish[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.Write("Расшифрованное сообщение: ");
            foreach (char elem in decryptMessage)
            {
                Console.Write(elem);
            }
        }
    }
}
