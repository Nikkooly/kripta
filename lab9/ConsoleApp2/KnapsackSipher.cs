using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class KnapsackSipher
    {
        public static  Random RND = new Random();
        public static  char ONE = '1';
        public static  char ZERO = '0';
        public static  int RADIX = 2;

        private int[] d;
        private int[] e;
        private int z;
        private int a;
        private int n;
        private int aInv;

        public KnapsackSipher(int z)
        {
            this.z = z;
            d = new int[z];
            e = new int[z];
            init();
            printInfo();
            Array.Sort(d);
        }

        private void init()
        {

            int sum = 0;
            int di;
            for (int i = 0; i < z; i++)
            {
                do
                {
                    di = RND.Next(z) + sum; //nextInt(z) + sum;
                } while (di < sum);
                d[i] = ++di;
                sum += d[i];
            }

            a = RND.Next(sum);
            do
            {
                n = RND.Next(sum) + sum;
            } while (gcd(n, a) != 1);

            aInv = modInverse(a, n);

            for (int i = 0; i < z; i++)
            {
                e[i] = (d[i] * a) % n;
            }
        }


        public int[] encrypt(byte[] data)
        {
            int[] result = new int[data.Length];
            String binaryCode;
            int sum;
            for (int i = 0; i < result.Length; i++)
            {
                binaryCode = String.format("%8s",
                        Integer.toBinaryString(data[i])).replace(' ', '0');
                sum = 0;
                for (int j = 0; j < z; j++)
                {
                    if (binaryCode[j] == ONE)
                    {
                        sum += e[j];
                    }
                }
                result[i] = sum;
            }
            return result;
        }

        public byte[] decrypt(int[] data)
        {
            StringBuilder binaryCode;
            int sum;
            byte[] result = new byte[data.Length];
            for (int i = 0; i < result.Length; i++)
            {
                sum = (data[i] * aInv) % n;
                binaryCode = new StringBuilder();
                for (int j = z - 1; j >= 0; j--)
                {
                    if (d[j] <= sum)
                    {
                        sum -= d[j];
                        binaryCode.Append(ONE);
                    }
                    else
                    {
                        binaryCode.Append(ZERO);
                    }
                }
                binaryCode.reverse().replace(0, 1, String.valueOf(ZERO));
                result[i] = (byte)Integer.parseInt(binaryCode.toString(), RADIX);
            }
            return result;
        }

        private int gcd(int a, int b)
        {
            return BigInteger.valueOf(a).gcd(BigInteger.valueOf(b)).intValue();
        }

        private int modInverse(int a, int m)
        {
            return BigInteger.valueOf(a)
                    .modInverse(BigInteger.valueOf(m)).intValue();
        }

        void printInfo()
        {
            System.out.println("d: " + Arrays.toString(d));
            System.out.println("e: " + Arrays.toString(e));
            System.out.println("a = " + a);
            System.out.println("n = " + n);
            System.out.println("a^-1 = " + aInv);
        }
    }
}
