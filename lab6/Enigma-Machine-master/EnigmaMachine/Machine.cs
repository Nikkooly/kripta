using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EnigmaMachine
{
    class Machine
    {
        //settings for each wheel
        private char rightWheelOffset;
        private char rightWheelStart;
        private char[] rightStatic;
        private char[] rightMoving;
        private int rightWheelSelection;

        private char middleWheelOffset;
        private char middleWheelStart;
        private char[] middleStatic;
        private char[] middleMoving;
        private int middleWheelSelection;

        private char leftWheelOffset;
        private char leftWheelStart;
        private char[] leftStatic;
        private char[] leftMoving;
        private int leftWheelSelection;

        //settings for the reflector
        private int reflectorSelection;
        private char[] reflector1;
        private char[] reflector2;

        private Wheels right;
        private Wheels middle;
        private Wheels left;

        private Reflector reflect;

        public Machine(char rOff, char rStart, int rSelect, char mOff, char mStart, int mSelect, char lOff, char lStart, int lSelect, int refSelect)
        {
            this.rightWheelOffset = rOff;
            this.rightWheelStart = rStart;
            this.rightWheelSelection = rSelect;

            this.middleWheelOffset = mOff;
            this.middleWheelStart = mStart;
            this.middleWheelSelection = mSelect;

            this.leftWheelOffset = lOff;
            this.leftWheelStart = lStart;
            this.leftWheelSelection = lSelect;

            this.reflectorSelection = refSelect;

            this.right = new Wheels(rightWheelOffset, rightWheelStart, rightWheelSelection);
            this.middle = new Wheels(middleWheelOffset, middleWheelStart, middleWheelSelection);
            this.left = new Wheels(leftWheelOffset, leftWheelStart, leftWheelSelection);

            this.reflect = new Reflector(reflectorSelection);

            reflector1 = reflect.getReflector1();
            reflector2 = reflect.getReflector2();

            rightStatic = right.getStaticWheel();
            rightMoving = right.getRotatingWheel();

            middleStatic = middle.getStaticWheel();
            middleMoving = middle.getRotatingWheel();

            leftStatic = left.getStaticWheel();
            leftMoving = left.getRotatingWheel();
        }

        public char run(char c)
        {
            right.rotate();                         //always rotate the right wheel before running the character through the machine
            rightMoving = right.getRotatingWheel();
            if (middle.checkNotchMiddle() == true)  //check if the machine is in a double step situation
            {
                middle.rotate();                    //if so, rotate the middle and right wheels
                middleMoving = middle.getRotatingWheel();
                left.rotate();
                leftMoving = left.getRotatingWheel();
            }
            if (right.checkNotch() == true)         //if the right wheel hits the notch, rotate the middle wheel
            {
                middle.rotate();
                middleMoving = middle.getRotatingWheel();
            }
            

            c = right.mappingForward(c);    //right     ->
            c = middle.mappingForward(c);   //middle    ->
            c = left.mappingForward(c);     //left      ->
            c = reflect.reflect(c);         //reflect   ->
            c = left.mappingBackwards(c);   //left      ->
            c = middle.mappingBackwards(c); //middle    ->
            c = right.mappingBackwards(c);  //right     ->
            
            return c;
        }

        public char[] getRightStatic()//used for running the GUI
        {
            return rightStatic;
        }

        public char[] getRightRotating()//used for running the GUI
        {
            return rightMoving;
        }

        public char[] getMiddleStatic()//used for running the GUI
        {
            return middleStatic;
        }

        public char[] getMiddleRotating()//used for running the GUI
        {
            return middleMoving;
        }

        public char[] getLeftStatic()//used for running the GUI
        {
            return leftStatic;
        }

        public char[] getLeftRotating()//used for running the GUI
        {
            return leftMoving;
        }

        public char[] getReflect1()
        {
            return reflector1;
        }

        public char[] getReflect2()
        {
            return reflector2;
        }

    }
}
