using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_10.Extensions
{
    public static class StringExtensions
    {
        private class FNV
        {
            int shift;
            uint mask;

            // hash without xor-folding
            public FNV()
            {
                shift = 0;
                mask = 0xFFFFFFFF;
            }

            // hash with xor-folding
            public FNV(int bits)
            {
                shift = 32 - bits;
                mask = (1U << shift) - 1U;
            }

            public uint ComputeHash(byte[] data)
            {
                const uint p = 16777619;
                uint hash = 2166136261;
                foreach (byte b in data)
                    hash = (hash ^ b) * p;
                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }

        public static uint GetDigest(this string s)
        {
            Encoding e = new UTF8Encoding();
            return new FNV().ComputeHash(e.GetBytes(s));
        }
    }
}
