using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaMachine
{
    class Wheels
    {
        char[] alpha  = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
        char[] wheel1 = { 'E', 'K', 'M', 'F', 'L', 'G', 'D', 'Q', 'V', 'Z', 'N', 'T', 'O', 'W', 'Y', 'H', 'X', 'U', 'S', 'P', 'A', 'I', 'B', 'R', 'C', 'J'};
        char[] wheel2 = { 'A', 'J', 'D', 'K', 'S', 'I', 'R', 'U', 'X', 'B', 'L', 'H', 'W', 'T', 'M', 'C', 'Q', 'G', 'Z', 'N', 'P', 'Y', 'F', 'V', 'O', 'E'};
        char[] wheel3 = { 'B', 'D', 'F', 'H', 'J', 'L', 'C', 'P', 'R', 'T', 'X', 'V', 'Z', 'N', 'Y', 'E', 'I', 'W', 'G', 'A', 'K', 'M', 'U', 'S', 'Q', 'O'};
        char[] wheel4 = { 'E', 'S', 'O', 'V', 'P', 'Z', 'J', 'A', 'Y', 'Q', 'U', 'I', 'R', 'H', 'X', 'L', 'N', 'F', 'T', 'G', 'K', 'D', 'C', 'M', 'W', 'B'};
        char[] wheel5 = { 'V', 'Z', 'B', 'R', 'G', 'I', 'T', 'Y', 'U', 'P', 'S', 'D', 'N', 'H', 'L', 'X', 'A', 'W', 'M', 'J', 'Q', 'O', 'F', 'E', 'C', 'K'};
        char[] wheel6 = { 'J', 'P', 'G', 'V', 'O', 'U', 'M', 'F', 'Y', 'Q', 'B', 'E', 'N', 'H', 'Z', 'R', 'D', 'K', 'A', 'S', 'X', 'L', 'I', 'C', 'T', 'W'};
        char[] wheel7 = { 'N', 'Z', 'J', 'H', 'G', 'R', 'C', 'X', 'M', 'Y', 'S', 'W', 'B', 'O', 'U', 'F', 'A', 'I', 'V', 'L', 'P', 'E', 'K', 'Q', 'D', 'T'};
        char[] wheel8 = { 'F', 'K', 'Q', 'H', 'T', 'L', 'X', 'O', 'C', 'B', 'J', 'S', 'P', 'D', 'Z', 'R', 'A', 'M', 'E', 'W', 'N', 'I', 'U', 'Y', 'G', 'V'};

        private char[] rotatingWheel;    //char array of the rotating wheel
        private char[] staticWheel;      //char array of the static wheel
        private char notchChar1;         //indicate the notch character
        private char notchChar2;         //indicate the second notch character, if it exist
        private char offset;              //offset start of the roter wheel
        private char start;               //start position for the static wheel
        private int wheelSelection;      //user specified which wheel to use.

        public Wheels(char off, char str, int whlSelect)
        {
            this.offset = off;
            this.start = str;
            this.wheelSelection = whlSelect;
            initilize();
        }

        public void initilize()
        {
            //set the rotating wheel to the wheelSelection

            switch (wheelSelection)
            {
                case 1:
                    {
                        this.rotatingWheel = wheel1;
                        this.notchChar1 = 'R';
                        this.notchChar2 = 'R';
                        break;
                    }
                case 2:
                    {
                        this.rotatingWheel = wheel2;
                        this.notchChar1 = 'F';
                        this.notchChar2 = 'F';
                        break;
                    }
                case 3:
                    {
                        this.rotatingWheel = wheel3;
                        this.notchChar1 = 'W';
                        this.notchChar2 = 'W';
                        break;
                    }
                case 4:
                    {
                        this.rotatingWheel = wheel4;
                        this.notchChar1 = 'K';
                        this.notchChar2 = 'K';
                        break;
                    }
                case 5:
                    {
                        this.rotatingWheel = wheel5;
                        this.notchChar1 = 'A';
                        this.notchChar2 = 'A';
                        break;
                    }
                case 6:
                    {
                        this.rotatingWheel = wheel6;
                        this.notchChar1 = 'A';
                        this.notchChar2 = 'N';
                        break;
                    }
                case 7:
                    {
                        this.rotatingWheel = wheel7;
                        this.notchChar1 = 'A';
                        this.notchChar2 = 'N';
                        break;
                    }
                case 8:
                    {
                        this.rotatingWheel = wheel8;
                        this.notchChar1 = 'A';
                        this.notchChar2 = 'N';
                        break;
                    }
            }

            //set the starting position of the static wheel

            staticWheel = alpha;

            while (staticWheel[0] != start)
            {
                rotateStatic();
            }


            //set the starting position of the rotating wheel
            while (rotatingWheel[0] != offset)
            {
                rotate();
            }

        }

        public void rotate()//rotate the rotating wheel 1 position to the right in relation to the static wheel, so the mapping is different
        {
            char[] temp = new char[rotatingWheel.Length];
            for (int i = 0; i < rotatingWheel.Length; i++)
            {
                temp[(i+1) % temp.Length] = rotatingWheel[i];
            }

            rotatingWheel = temp;
        }

        public void rotateStatic()//rotate the wheel 1 position to the right, used when initilizing the enigma machine
        {
            char[] temp = new char[staticWheel.Length];
            for (int i = 0; i < staticWheel.Length; i++)
            {
                temp[(i + 1) % temp.Length] = staticWheel[i];
            }

            staticWheel = temp;
        }

        public bool checkNotch()//check and see if the current character is at the notch position, sinaling to rotate the next wheel
        {
            if (notchChar1 == rotatingWheel[0] || notchChar2 == rotatingWheel[0] )
                return true;
            else
                return false;
        }

        public bool checkNotchMiddle()//checks if the next position will trigger a doublestep
        {
            if (notchChar1 == rotatingWheel[25] || notchChar2 == rotatingWheel[25])
                return true;
            else
                return false;
        }

        public char mappingForward(char c) //find the index of the current character mapped in relation to the static wheel
        {
            for (int i = 0; i < staticWheel.Length; i++)
            {
                if (c == staticWheel[i])        //find the index of the character in the static wheel
                    return rotatingWheel[i];    //using that index, return the character in the corresponding index of the rotating wheel
            }
            return c;                           //unhandled exception yo
        }

        public char mappingBackwards(char c) //find the index of the current character mapped in relation to the rotating wheel
        {
            for (int i = 0; i < staticWheel.Length; i++)
            {
                if (c == rotatingWheel[i])          //find the index of the character in the rotating wheel
                    return staticWheel[i];          //using that index, return the character in the corresponding index of the static wheel
            }
            return c;                               //unhandled exception yo
        }

        public char[] getStaticWheel()//used for running the GUI
        {
            return staticWheel;
        }

        public char[] getRotatingWheel()//used for running the GUI
        {
            return rotatingWheel;
        }
    }
}
