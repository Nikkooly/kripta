using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaMachine
{
    class Reflector
    {
        //(AY) (BR) (CU) (DH) (EQ) (FS) (GL) (IP) (JX) (KN) (MO) (TZ) (VW)
        char[] B1 = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'I', 'J', 'K', 'M', 'T', 'V' };
        char[] B2 = { 'Y', 'R', 'U', 'H', 'Q', 'S', 'L', 'P', 'X', 'N', 'O', 'Z', 'W' };
        //(AF) (BV) (CP) (DJ) (EI) (GO) (HY) (KR) (LZ) (MX) (NW) (TQ) (SU)
        char[] C1 = { 'A', 'B', 'C', 'D', 'E', 'G', 'H', 'K', 'L', 'M', 'N', 'T', 'S' };
        char[] C2 = { 'F', 'V', 'P', 'J', 'I', 'O', 'Y', 'R', 'Z', 'X', 'W', 'Q', 'U' };

        private int selection;
        private char[] reflect1;
        private char[] reflect2;

        public Reflector(int select)
        {
            this.selection = select;
            if (selection == 1)
            {
                this.reflect1 = B1;
                this.reflect2 = B2;
            }

            if (selection == 2)
            {
                this.reflect1 = C1;
                this.reflect2 = C2;
            }
        }

        public char reflect(char c)
        {
            //search both arrays for c, and find the index and the cooresponding reflection
            for (int i = 0; i < reflect1.Length; i++)
            {
                if (reflect1[i] == c)
                    return reflect2[i];
                if (reflect2[i] == c)
                    return reflect1[i];
            }
            return c;               //another unhandled exception
        }

        public char[] getReflector1()
        {
            return reflect1;
        }
        public char[] getReflector2()
        {
            return reflect2;
        }
    }
}
