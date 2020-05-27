using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections;
using Lab_10.Extensions;

namespace Lab_10.Utils
{
    public class RandomPrimeGenerator : RandomGenerator
    {
        private List<BigInteger> primes;
        private Random rand;

        public RandomPrimeGenerator(int max = 9999999)
        {
            this.primes = GeneratePrimes(max);
            this.rand = new Random();
        }

        private List<BigInteger> GeneratePrimes(int max)
        {
            List<BigInteger> result = new List<BigInteger>();
            BitArray sieve = new BitArray(max, true);
            for (int i = 2; i < Math.Sqrt(max) + 1; i++)
            {
                if (!sieve[i])
                {
                    continue;
                }

                for (int j = i * i; j < max; j += i)
                {
                    sieve[j] = false;
                }
            }
            for (int i = 2; i < max; i++)
            {
                if (sieve[i])
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private const int MIN_NUM = 2;
        private const int MAX_NUM = 99999;

        public List<BigInteger> Numbers()
        {
            return this.primes;
        }

        public BigInteger Next()
        {
            return Next(MIN_NUM, MAX_NUM);
        }

        public BigInteger Next(Predicate<BigInteger> p)
        {
            return Next(MIN_NUM, MAX_NUM, p);
        }

        public BigInteger Next(BigInteger min, BigInteger max)
        {
            return Next(min, max, x => true);
        }

        public BigInteger Next(BigInteger min, BigInteger max, Predicate<BigInteger> p)
        {
            int minIdx = primes.FindIndex(n => n.CompareTo(min) >= 0);
            int maxIdx = primes.FindLastIndex(n => n <= max);
            int idx;
            do { idx = rand.Next(minIdx, maxIdx); } while (!p(primes[idx]));
            return primes[idx];
        }
    }
}
