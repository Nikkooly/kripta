using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class RSA
    {
         char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                '8', '9', '0' };
        long n, d;
        public  void Encrypt(long p, long q)
        {
            if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
            {
                string s = "";

                StreamReader sr = new StreamReader("in.txt");

                while (!sr.EndOfStream)
                {
                    s += sr.ReadLine();
                }

                sr.Close();

                s = s.ToUpper();

                n = p * q;
                long m = (p - 1) * (q - 1);
                d = Calculate_d(m);
                long e_ = Calculate_e(d, m);

                List<string> result = RSA_Endoce(s, e_, n);

                StreamWriter sw = new StreamWriter("out1.txt");
                foreach (string item in result)
                    sw.WriteLine(item);
                sw.Close();
                Console.WriteLine($"Числа для расшифровки \n d={d.ToString()} \n n={n.ToString()} ");

                Process.Start("out1.txt");
            }
            else
                Console.WriteLine("p или q - не простые числа!");
        }
        public  void Decrypt()
        {
            List<string> input = new List<string>();

            StreamReader sr = new StreamReader("out1.txt");

            while (!sr.EndOfStream)
            {
                input.Add(sr.ReadLine());
            }

            sr.Close();

            string result = RSA_Dedoce(input, d, n);

            StreamWriter sw = new StreamWriter("out2.txt");
            sw.WriteLine(result);
            sw.Close();

            Process.Start("out2.txt");
        }
        public bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
        //Метод, выполняющий шифрование строки алгоритмом RSA:
        List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }
        /*При возведении числа в степень в данном случае получаются очень большие числа,
         * которые не помещаются ни в один из стандартных типов. Поэтому для их хранения используется экземпляр
         * класса BigInteger. Этот класс позволяет хранить целые числа произвольной (любой) длины и выполнять
         * математические операции с ними.Метод, выполняющий расшифровку строки алгоритмом RSA
         */
        public  string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }
        //Вычисление параметра d (d должно быть взаимно простым с m).
        public  long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }
        //Метод, вычисляющий значение параметра e
        public long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

    }
}
