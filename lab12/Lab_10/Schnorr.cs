using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Lab_10.Utils;
using Lab_10.Extensions;

namespace Lab_10
{
    public struct DomainParameters
    {
        public BigInteger P { get; private set; }
        public BigInteger Q { get; private set; }
        public BigInteger G { get; private set; }
        public BigInteger H { get; private set; }

        private static RandomPrimeGenerator primes;

        public DomainParameters(BigInteger p, BigInteger q, BigInteger g)
            : this()
        {
            P = p;
            Q = q;
            G = g;
        }

        public DomainParameters(BigInteger p, BigInteger q, BigInteger g, BigInteger h)
            : this(p, q, g)
        {
            H = h;
        }

        public override string ToString()
        {
            return "P: " + P + " Q: " + Q + " G: " + G;
        }

        public static DomainParameters GenerateDomainParameters(int minQ = 10000, int maxQ = 99999)
        {
            if (primes == null)
                primes = new RandomPrimeGenerator();
            RandomGenerator numbers = new RandomNumberGenerator();

            BigInteger q, p, h, g;

            q = primes.Next(minQ, maxQ);

            int i = 1;
            int tries = 0;
            do
            {
                p = q * i + 1;
                i++;
                tries++;
                if (tries > 500) { i = 0; tries = 0; q = primes.Next(minQ, maxQ); }
            } while (!p.IsProbablePrime(1));
            h = numbers.Next(1, p - 1);
            g = BigInteger.ModPow(h, (p - 1) / q, p);
            //g = numbers.Next(n => BigInteger.Pow(n, (int)q) % p == 1);
            if (g.IsZero || g.IsOne) { throw new Exception("G = 0 or G = 1"); }
            return new DomainParameters(p, q, g, h);
        }
    }

    public class Schnorr
    {
        private const int T_MIN = 40;

        public DomainParameters Domain { get; set; }
        public BigInteger T { get; set; }
        private RandomGenerator numbers;

        public Schnorr(DomainParameters domain)
        {
            numbers = new RandomNumberGenerator();
            Domain = domain;
            T = GenerateT();
        }

        public BigInteger GenerateT()
        {
            BigInteger result = numbers.Next(T_MIN, BigInteger.Min(Domain.Q - 1, 100000));
            return result;
        }
    }

    public class SchorrProver
    {
        public BigInteger Secret { get; set; }
        public BigInteger PublicKey { get; set; }
        public DomainParameters Domain { get; private set; }
        public BigInteger R { get; private set; }
        public BigInteger X { get; private set; }
        public BigInteger Y { get; private set; }

        private RandomGenerator numbers;
        
        public SchorrProver(Schnorr schorrParameters)
        {
            Domain = schorrParameters.Domain;
            numbers = new RandomNumberGenerator();
        }

        public void GenerateKeys()
        {
            Secret = numbers.Next(1, Domain.Q - 1);
            BigInteger inverse = Domain.G.ModInverse(Domain.P);
            PublicKey = BigInteger.ModPow(inverse, Secret, Domain.P);
        }

        public BigInteger Call()
        {
            R = numbers.Next(1, Domain.Q - 1);
            X = BigInteger.ModPow(Domain.G, R, Domain.P);
            return X;
        }

        public BigInteger Response(BigInteger challenge)
        {
            Y = (Secret * challenge + R) % Domain.Q;
            return Y;
        }
    }

    public class SchorrVerifier
    {
        public DomainParameters Domain { get; private set; }
        public BigInteger T { get; set; }
        public BigInteger E { get; private set; }
        public BigInteger X { get; set; }
        public BigInteger Z { get; private set; }

        private RandomGenerator numbers;

        public SchorrVerifier(Schnorr schorrParameters)
        {
            Domain = schorrParameters.Domain;
            T = schorrParameters.T;
            numbers = new RandomNumberGenerator();
        }

        public BigInteger Challenge()
        {
            E = numbers.Next(1, BigInteger.Pow(2, (int)T));
            return E;
        }

        public bool VerifyResponse(BigInteger publicKey, BigInteger response)
        {
            Z = (BigInteger.ModPow(Domain.G, response, Domain.P) * BigInteger.ModPow(publicKey, E, Domain.P)) % Domain.P;
            return Z == X;
        }
    }
}
