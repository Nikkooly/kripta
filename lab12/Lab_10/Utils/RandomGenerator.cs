using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Lab_10.Utils
{
    interface RandomGenerator
    {
        BigInteger Next();
        BigInteger Next(Predicate<BigInteger> p);
        BigInteger Next(BigInteger min, BigInteger max);
        BigInteger Next(BigInteger min, BigInteger max, Predicate<BigInteger> p);
    }
}
