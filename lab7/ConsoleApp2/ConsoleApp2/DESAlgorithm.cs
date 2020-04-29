using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{

    class DESAlgorithm
    {
        public const int sizeOfBlock = 128; //в DES размер блока 64 бит, но поскольку в unicode символ в два раза длинее, то увеличим блок тоже в два раза
        public const int sizeOfChar = 16; //размер одного символа (in Unicode 16 bit)
        public const int shiftKey = 2; //сдвиг ключа 
        public int quantityOfRounds = 16; //количество раундов
        public string[] Blocks; //сами блоки в двоичном формате
        /*
         * Метод, доводящий строку до такого размера, чтобы она делилась на sizeOfBlock.
         * Размер увеличивается с помощью добавления к исходной строке символа “решетка”.
         */
        public string StringToRightLength(string input)
        {
            while (((input.Length * sizeOfChar) % sizeOfBlock) != 0)
                input += "#";
            return input;
        }
        //Метод, разбивающий строку в обычном (символьном) формате на блоки.
        public void CutStringIntoBlocks(string input)
        {
            Blocks = new string[(input.Length * sizeOfChar) / sizeOfBlock];

            int lengthOfBlock = input.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
                Blocks[i] = StringToBinaryFormat(Blocks[i]);
            }
        }
        //Метод, разбивающий строку в двоичном формате на блоки.
        public void CutBinaryStringIntoBlocks(string input)
        {
            Blocks = new string[input.Length / sizeOfBlock];

            int lengthOfBlock = input.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
        }
        //Метод, переводящий строку в двоичный формат.
        public string StringToBinaryFormat(string input)
        {
            string output = "";

            for (int i = 0; i < input.Length; i++)
            {
                string char_binary = Convert.ToString(input[i], 2);

                while (char_binary.Length < sizeOfChar)
                    char_binary = "0" + char_binary;

                output += char_binary;
            }

            return output;
        }
        //Метод, доводящий длину ключа до нужной длины.
        public string CorrectKeyWord(string input, int lengthKey)
        {
            if (input.Length > lengthKey)
                input = input.Substring(0, lengthKey);
            else
                while (input.Length < lengthKey)
                    input = "0" + input;

            return input;
        }
        //Один раунд шифрования алгоритмом DES.
        public string EncodeDES_One_Round(string input, string key)
        {
            string L = input.Substring(0, input.Length / 2);
            string R = input.Substring(input.Length / 2, input.Length / 2);

            return (R + XOR(L, f(R, key)));
        }
        //Один раунд расшифровки алгоритмом DES.
        public string DecodeDES_One_Round(string input, string key)
        {
            string L = input.Substring(0, input.Length / 2);
            string R = input.Substring(input.Length / 2, input.Length / 2);

            return (XOR(f(L, key), R) + L);
        }
        //XOR двух строк с двоичными данными.
        public string XOR(string s1, string s2)
        {
            string result = "";

            for (int i = 0; i < s1.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

                if (a ^ b)
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }
        //Шифрующая функция f. Изпользуем в качестве нее также логическую операцию XOR.
        public string f(string s1, string s2)
        {
            return XOR(s1, s2);
        }
        //Вычисление ключа для следующего раунда шифрования DES. Циклический сдвиг >> shiftKey.
        public string KeyToNextRound(string key)
        {
            for (int i = 0; i < shiftKey; i++)
            {
                key = key[key.Length - 1] + key;
                key = key.Remove(key.Length - 1);
            }

            return key;
        }
        //Вычисление ключа для следующего раунда расшифровки DES. циклический сдвиг << shiftKey.
        public string KeyToPrevRound(string key)
        {
            for (int i = 0; i < shiftKey; i++)
            {
                key = key + key[0];
                key = key.Remove(0, 1);
            }

            return key;
        }
        //
        public string StringFromBinaryToNormalFormat(string input)
        {
            string output = "";

            while (input.Length > 0)
            {
                string char_binary = input.Substring(0, sizeOfChar);
                input = input.Remove(0, sizeOfChar);

                int a = 0;
                int degree = char_binary.Length - 1;

                foreach (char c in char_binary)
                    a += Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--);

                output += ((char)a).ToString();
            }

            return output;
        }
    }
}
