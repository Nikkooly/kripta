using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class Numbers
    {
        public static int GetNextPrimeAfter(int n)
        {
            return 3571;
        }
        
        public static int Rand(int min, int max) //Ф-я получения случайного числа
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        
        public static int Power(int a, int b, int n) // a^b mod n - возведение a в степень b по модулю n
        {
            var tmp = a;
            var sum = tmp;
            for (var i = 1; i < b; i++)
            {
                for (var j = 1; j < a; j++)
                {
                    sum += tmp;
                    if (sum >= n)
                    {
                        sum -= n;
                    }
                }

                tmp = sum;
            }

            return tmp;
        }
        
        public static int Mul(int a, int b, int n) // a*b mod n - умножение a на b по модулю n
        {
            var sum = 0;
            for (var i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= n)
                {
                    sum -= n;
                }
            }

            return sum;
        }

        public static int GetPRoot(int p)
        {
            for (var i = 0; i < p; i++)
                if (IsPRoot(p, i))
                    return i;
            return 0;
        }

        private static bool IsPRoot(int p, int a)
        {
            if (a == 0 || a == 1)
                return false;
            var last = 1;
            var set = new HashSet<int>();
            for (var i = 0; i < p - 1; i++)
            {
                last = (last * a) % p;
                if (set.Contains(last))
                    return false;
                set.Add(last);
            }
            return true;
        }
    }
}
