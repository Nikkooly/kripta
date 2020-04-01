using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{

    class Program
    {
        public static int n = 397;
        public static int m = 354;
        public static int isSimpleNumber;
        private static bool isSimple(int N)
        {
            for (int i = 2; i <= (int)(N / 2); i++)
            {
                if (N % i == 0)
                    return false;
            }
            return true;
        }
        private static void nodWork(int a, int b)
        {
            for (int i = a; i > 0; i--)
            {
                if (a % i == 0)
                {
                    if (b % i == 0)
                    {
                        Console.WriteLine($"Нод равен {i}", i);
                        isSimpleNumber = i;
                        Console.ReadLine();
                        break;
                    }
                }
            }
        }
        private static void Evklid(int a, int m)
        {

            a = a % m;

            for (int x = 1; x < m; x++)

                if ((a * x) % m == 1)

                    Console.WriteLine(x);

        }
        private static void simpleNumbers(int a)
        {
            int b = 2;
            string rez = string.Empty;
            while (a > 1)
            {
                if (a % b == 0)
                {
                    rez += b + "*";
                    a /= b;
                    b = 2;
                }
                else
                    b++;
            }
            Console.WriteLine(rez.Remove(rez.Length - 1, 1));
        }
        private static void nodForThreeNumber(int a, int b, int c)
        {
            for (int i = a; i > 0; i--)
            {
                if (a % i == 0)
                {
                    if (b % i == 0)
                    {
                        if (c % i == 0)
                        {
                            Console.WriteLine($"Нод равен {i}", i);
                            break;
                        }

                    }
                }
            }
        }
        static void Main(string[] args)
        {
            for (; ; )
            {
                Console.WriteLine("Выберите операцию:");
                Console.WriteLine($"1. Вычисление НОД для m={m} n={n}", m, n);
                Console.WriteLine($"2. Числа m={m} и n={n} в виде произведения простых множителей", m, n);
                Console.WriteLine("3. Проверка, является ли число, состоящее из конкатенации цифр m и n, простым.");
                Console.WriteLine($"4. Количество простых чисел между m={m} n={n}", m, n);
                Console.WriteLine($"5. Количество простых чисел между m=2 n={n}", n);
                Console.WriteLine("6. Вычисление НОД для двух чисел");
                Console.WriteLine("7. Вычисление НОД для трех чисел");
                Console.WriteLine("8. Поиск простых чисел в заданном интервале");
                Console.WriteLine("9. Нахождение обратного по модулю");
                int a = int.Parse(Console.ReadLine());

                switch (a)
                {
                    case 1:
                        nodWork(n, m);
                        break;
                    case 2:
                        simpleNumbers(n);
                        simpleNumbers(m);
                        break;
                    case 3:
                        String number = String.Concat(m.ToString(), n.ToString());
                        int result = Convert.ToInt32(number);
                        if (isSimple(result))
                        {
                            Console.WriteLine($"Число {result} простое", result);
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Число {result} не простое", result);
                            Console.ReadLine();
                        }
                        break;
                    case 4:
                        int countCaseFour = 0;
                        for (int i = m; i <= n; i++)
                        {
                            if (isSimple(i))
                            {
                                countCaseFour++;
                            }
                        }
                        Console.WriteLine($"Количество {countCaseFour} ", countCaseFour);
                        break;
                    case 5:
                        int countCaseFive = 0;
                        for (int i = 2; i <= n; i++)
                        {
                            if (isSimple(i))
                            {
                                countCaseFive++;
                            }
                        }
                        Console.WriteLine($"Количество {countCaseFive} ", countCaseFive);
                        break;
                    case 6:
                        int firstNumber = int.Parse(Console.ReadLine());
                        int secondNumber = int.Parse(Console.ReadLine());
                        nodWork(firstNumber, secondNumber);
                        break;
                    case 7:
                        int first = int.Parse(Console.ReadLine());
                        int second = int.Parse(Console.ReadLine());
                        int third = int.Parse(Console.ReadLine());
                        nodForThreeNumber(first, second, third);
                        break;
                    case 8:
                        int startNumber = int.Parse(Console.ReadLine());
                        int maxNumber = int.Parse(Console.ReadLine());
                        for (int i = startNumber; i <= maxNumber; i++)
                        {
                            if (isSimple(i))
                            {
                                Console.Write(i.ToString() + " ");
                            }
                        }
                        break;
                    case 9:
                        Console.WriteLine("Введите число к которому хотите найти обратный");
                        int firstNum = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите модуль");
                        int secNum = int.Parse(Console.ReadLine());
                        nodWork(firstNum, secNum);
                        if (isSimpleNumber.Equals(1))
                        {
                            Evklid(firstNum, secNum);
                        }
                        else
                        {
                            Console.WriteLine("Не взаимнопростые");
                        }
                        break;
                        
                    default:
                        Console.WriteLine("Вы овощ");
                        break;
                }

            }
        }
    }
}
