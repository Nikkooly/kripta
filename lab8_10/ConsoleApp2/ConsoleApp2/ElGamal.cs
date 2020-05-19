using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class ElGamal
    {
        public static string crText = "";
        public void Crypt(int p, int g, int x, string strIn) //Шифрование
        {
            var y = Numbers.Power(g, x, p);
            Console.WriteLine( $"Открытый ключ (p,g,y) = ( {p}, {g}, {y})");
           Console.WriteLine($"Закрытый ключ x = {x}");

            var sb = new StringBuilder();

            foreach (var t in strIn)
            {
                var m = Convert.ToInt32(t);
                if (m <= 0) continue;

                var k = Numbers.Rand(1, p - 1);
                var a = Numbers.Power(g, k, p);
                var b = Numbers.Mul(Numbers.Power(y, k, p), m, p);

                sb.Append($"{a} {b} ");
            }
            crText = sb.ToString();
            StreamWriter sw = new StreamWriter("out1.txt");
            sw.WriteLine(sb.ToString());
            sw.Close();
            Process.Start("out1.txt");
            // Console.WriteLine(sb.ToString());
        }

        public void Decrypt(int p, int x)
        {
            if (crText.Length > 0)
            {
                var sb = new StringBuilder();
                var crypted = crText.Trim().Split(' ').Select(int.Parse).ToArray();

                for (var i = 0; i < crypted.Length - 1; i += 2)
                {
                    var a = crypted[i];
                    var b = crypted[i + 1];

                    if ((a == 0) || (b == 0)) continue;

                    var deM = Numbers.Mul(b, Numbers.Power(a, p - 1 - x, p), p); // m=b*(a^x)^(-1)mod p =b*a^(p-1-x)mod p - трудно было  найти нормальную формулу, в ней вся загвоздка
                    //Console.WriteLine(deM);
                    sb.Append(Convert.ToChar(deM));
                }

                StreamWriter sw = new StreamWriter("out2.txt");
                sw.WriteLine(sb.ToString());
                sw.Close();
                Process.Start("out2.txt");
                return;
            }
        }
    }
}
