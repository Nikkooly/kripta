using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace EnigmaMachine
{
    public partial class Enigma : Form
    {
        public Enigma()
        {
            InitializeComponent();
        }

        private Machine EnigmaMachine;
        private char[] rightMoving;
        private char[] rightStatic;

        private char[] middleMoving;
        private char[] middleStatic;

        private char[] leftMoving;
        private char[] leftStatic;

        private char[] reflect1;
        private char[] reflect2;

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text.ToUpper();
            char encrypted;
            foreach (char c in message)
            {
                if (c == ' ')
                    textBox2.AppendText(c.ToString());
                else
                {

                    encrypted = EnigmaMachine.run(c);
                    Application.DoEvents();
                    updateWheels();
                    textBox2.AppendText(encrypted.ToString());
                }

                Thread.Sleep(1000);
            }
        }

        public void updateMachine()
        {
            //create Enigma Machine using the parameters specified.
            char rightWheelOffset = comboBox3.SelectedItem.ToString().ToCharArray()[0];
            char rightWheelStart = StartRightWheel.SelectedItem.ToString().ToCharArray()[0];
            int rightWheelSelection = comboBoxWheel1.SelectedIndex + 1;

            char middleWheelOffset = comboBox1.SelectedItem.ToString().ToCharArray()[0];
            char middleWheelStart = StartMiddleWheel.SelectedItem.ToString().ToCharArray()[0];
            int middleWheelSelection = comboBoxWheel2.SelectedIndex + 1;

            char leftWheelOffset = comboBox4.SelectedItem.ToString().ToCharArray()[0];
            char leftWheelStart = StartLeftWheel.SelectedItem.ToString().ToCharArray()[0];
            int leftWheelSelection = comboBoxWheel3.SelectedIndex + 1;

            //settings for the reflector
            int reflectorSelection = ReflectorComboBox.SelectedIndex + 1;

            this.EnigmaMachine = new Machine(rightWheelOffset, rightWheelStart, rightWheelSelection,
                                             middleWheelOffset, middleWheelStart, middleWheelSelection,
                                             leftWheelOffset, leftWheelStart, leftWheelSelection, reflectorSelection);
            reflect1 = EnigmaMachine.getReflect1();
            reflect2 = EnigmaMachine.getReflect2();

            updateWheels();

            updateReflector();

            updateRightStatic();
            updateLeftStatic();
            updateMiddleStatic();

            updateRightMoving();
            updateMiddleMoving();
            updateLeftMoving();
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            string message = textBox2.Text.ToUpper();
            char encrypted;
            foreach (char c in message)
            {
                if( c == ' ' )
                    textBox1.AppendText(c.ToString());
                else
                {
                    encrypted = EnigmaMachine.run(c);
                    Application.DoEvents();
                    updateWheels();
                    textBox1.AppendText(encrypted.ToString());
                }
                
                Thread.Sleep(1000);
            }

        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox1.Clear();
            updateMachine();
        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            updateMachine();
        }

       private void updateWheels()
        {
            rightMoving = EnigmaMachine.getRightRotating();
            rightStatic = EnigmaMachine.getRightStatic();
            middleMoving = EnigmaMachine.getMiddleRotating();
            middleStatic = EnigmaMachine.getMiddleStatic();
            leftMoving = EnigmaMachine.getLeftRotating();
            leftStatic = EnigmaMachine.getLeftStatic();

            updateRightMoving();
            updateMiddleMoving();
            updateLeftMoving();
        }

        private void updateRightStatic()
        {
            SR1.Text = rightStatic[0].ToString();
            SR2.Text = rightStatic[1].ToString();
            SR3.Text = rightStatic[2].ToString();
            SR4.Text = rightStatic[3].ToString();
            SR5.Text = rightStatic[4].ToString();
            SR6.Text = rightStatic[5].ToString();
            SR7.Text = rightStatic[6].ToString();
            SR8.Text = rightStatic[7].ToString();
            SR9.Text = rightStatic[8].ToString();
            SR10.Text = rightStatic[9].ToString();
            SR11.Text = rightStatic[10].ToString();
            SR12.Text = rightStatic[11].ToString();
            SR13.Text = rightStatic[12].ToString();
            SR14.Text = rightStatic[13].ToString();
            SR15.Text = rightStatic[14].ToString();
            SR16.Text = rightStatic[15].ToString();
            SR17.Text = rightStatic[16].ToString();
            SR18.Text = rightStatic[17].ToString();
            SR19.Text = rightStatic[18].ToString();
            SR20.Text = rightStatic[19].ToString();
            SR21.Text = rightStatic[20].ToString();
            SR22.Text = rightStatic[21].ToString();
            SR23.Text = rightStatic[22].ToString();
            SR24.Text = rightStatic[23].ToString();
            SR25.Text = rightStatic[24].ToString();
            SR26.Text = rightStatic[25].ToString();
        }

        private void updateRightMoving()
        {
            MR1.Text = rightMoving[0].ToString();
            MR2.Text = rightMoving[1].ToString();
            MR3.Text = rightMoving[2].ToString();
            MR4.Text = rightMoving[3].ToString();
            MR5.Text = rightMoving[4].ToString();
            MR6.Text = rightMoving[5].ToString();
            MR7.Text = rightMoving[6].ToString();
            MR8.Text = rightMoving[7].ToString();
            MR9.Text = rightMoving[8].ToString();
            MR10.Text = rightMoving[9].ToString();
            MR11.Text = rightMoving[10].ToString();
            MR12.Text = rightMoving[11].ToString();
            MR13.Text = rightMoving[12].ToString();
            MR14.Text = rightMoving[13].ToString();
            MR15.Text = rightMoving[14].ToString();
            MR16.Text = rightMoving[15].ToString();
            MR17.Text = rightMoving[16].ToString();
            MR18.Text = rightMoving[17].ToString();
            MR19.Text = rightMoving[18].ToString();
            MR20.Text = rightMoving[19].ToString();
            MR21.Text = rightMoving[20].ToString();
            MR22.Text = rightMoving[21].ToString();
            MR23.Text = rightMoving[22].ToString();
            MR24.Text = rightMoving[23].ToString();
            MR25.Text = rightMoving[24].ToString();
            MR26.Text = rightMoving[25].ToString();
        }

        private void updateMiddleStatic()
        {
            SM1.Text = middleStatic[0].ToString();
            SM2.Text = middleStatic[1].ToString();
            SM3.Text = middleStatic[2].ToString();
            SM4.Text = middleStatic[3].ToString();
            SM5.Text = middleStatic[4].ToString();
            SM6.Text = middleStatic[5].ToString();
            SM7.Text = middleStatic[6].ToString();
            SM8.Text = middleStatic[7].ToString();
            SM9.Text = middleStatic[8].ToString();
            SM10.Text = middleStatic[9].ToString();
            SM11.Text = middleStatic[10].ToString();
            SM12.Text = middleStatic[11].ToString();
            SM13.Text = middleStatic[12].ToString();
            SM14.Text = middleStatic[13].ToString();
            SM15.Text = middleStatic[14].ToString();
            SM16.Text = middleStatic[15].ToString();
            SM17.Text = middleStatic[16].ToString();
            SM18.Text = middleStatic[17].ToString();
            SM19.Text = middleStatic[18].ToString();
            SM20.Text = middleStatic[19].ToString();
            SM21.Text = middleStatic[20].ToString();
            SM22.Text = middleStatic[21].ToString();
            SM23.Text = middleStatic[22].ToString();
            SM24.Text = middleStatic[23].ToString();
            SM25.Text = middleStatic[24].ToString();
            SM26.Text = middleStatic[25].ToString();

        }

        private void updateMiddleMoving()
        {
            MM1.Text = middleMoving[0].ToString();
            MM2.Text = middleMoving[1].ToString();
            MM3.Text = middleMoving[2].ToString();
            MM4.Text = middleMoving[3].ToString();
            MM5.Text = middleMoving[4].ToString();
            MM6.Text = middleMoving[5].ToString();
            MM7.Text = middleMoving[6].ToString();
            MM8.Text = middleMoving[7].ToString();
            MM9.Text = middleMoving[8].ToString();
            MM10.Text = middleMoving[9].ToString();
            MM11.Text = middleMoving[10].ToString();
            MM12.Text = middleMoving[11].ToString();
            MM13.Text = middleMoving[12].ToString();
            MM14.Text = middleMoving[13].ToString();
            MM15.Text = middleMoving[14].ToString();
            MM16.Text = middleMoving[15].ToString();
            MM17.Text = middleMoving[16].ToString();
            MM18.Text = middleMoving[17].ToString();
            MM19.Text = middleMoving[18].ToString();
            MM20.Text = middleMoving[19].ToString();
            MM21.Text = middleMoving[20].ToString();
            MM22.Text = middleMoving[21].ToString();
            MM23.Text = middleMoving[22].ToString();
            MM24.Text = middleMoving[23].ToString();
            MM25.Text = middleMoving[24].ToString();
            MM26.Text = middleMoving[25].ToString();

        }

        private void updateLeftStatic()
        {
            SL1.Text = leftStatic[0].ToString();
            SL2.Text = leftStatic[1].ToString();
            SL3.Text = leftStatic[2].ToString();
            SL4.Text = leftStatic[3].ToString();
            SL5.Text = leftStatic[4].ToString();
            SL6.Text = leftStatic[5].ToString();
            SL7.Text = leftStatic[6].ToString();
            SL8.Text = leftStatic[7].ToString();
            SL9.Text = leftStatic[8].ToString();
            SL10.Text = leftStatic[9].ToString();
            SL11.Text = leftStatic[10].ToString();
            SL12.Text = leftStatic[11].ToString();
            SL13.Text = leftStatic[12].ToString();
            SL14.Text = leftStatic[13].ToString();
            SL15.Text = leftStatic[14].ToString();
            SL16.Text = leftStatic[15].ToString();
            SL17.Text = leftStatic[16].ToString();
            SL18.Text = leftStatic[17].ToString();
            SL19.Text = leftStatic[18].ToString();
            SL20.Text = leftStatic[19].ToString();
            SL21.Text = leftStatic[20].ToString();
            SL22.Text = leftStatic[21].ToString();
            SL23.Text = leftStatic[22].ToString();
            SL24.Text = leftStatic[23].ToString();
            SL25.Text = leftStatic[24].ToString();
            SL26.Text = leftStatic[25].ToString();
        }

        private void updateLeftMoving()
        {
            ML1.Text = leftMoving[0].ToString();
            ML2.Text = leftMoving[1].ToString();
            ML3.Text = leftMoving[2].ToString();
            ML4.Text = leftMoving[3].ToString();
            ML5.Text = leftMoving[4].ToString();
            ML6.Text = leftMoving[5].ToString();
            ML7.Text = leftMoving[6].ToString();
            ML8.Text = leftMoving[7].ToString();
            ML9.Text = leftMoving[8].ToString();
            ML10.Text = leftMoving[9].ToString();
            ML11.Text = leftMoving[10].ToString();
            ML12.Text = leftMoving[11].ToString();
            ML13.Text = leftMoving[12].ToString();
            ML14.Text = leftMoving[13].ToString();
            ML15.Text = leftMoving[14].ToString();
            ML16.Text = leftMoving[15].ToString();
            ML17.Text = leftMoving[16].ToString();
            ML18.Text = leftMoving[17].ToString();
            ML19.Text = leftMoving[18].ToString();
            ML20.Text = leftMoving[19].ToString();
            ML21.Text = leftMoving[20].ToString();
            ML22.Text = leftMoving[21].ToString();
            ML23.Text = leftMoving[22].ToString();
            ML24.Text = leftMoving[23].ToString();
            ML25.Text = leftMoving[24].ToString();
            ML26.Text = leftMoving[25].ToString();
        }

        private void updateReflector()
        {
            RI1.Text = reflect1[0].ToString();
            RI2.Text = reflect1[1].ToString();
            RI3.Text = reflect1[2].ToString();
            RI4.Text = reflect1[3].ToString();
            RI5.Text = reflect1[4].ToString();
            RI6.Text = reflect1[5].ToString();
            RI7.Text = reflect1[6].ToString();
            RI8.Text = reflect1[7].ToString();
            RI9.Text = reflect1[8].ToString();
            RI10.Text = reflect1[9].ToString();
            RI11.Text = reflect1[10].ToString();
            RI12.Text = reflect1[11].ToString();
            RI13.Text = reflect1[12].ToString();

            RO1.Text = reflect2[0].ToString();
            RO2.Text = reflect2[1].ToString();
            RO3.Text = reflect2[2].ToString();
            RO4.Text = reflect2[3].ToString();
            RO5.Text = reflect2[4].ToString();
            RO6.Text = reflect2[5].ToString();
            RO7.Text = reflect2[6].ToString();
            RO8.Text = reflect2[7].ToString();
            RO9.Text = reflect2[8].ToString();
            RO10.Text = reflect2[9].ToString();
            RO11.Text = reflect2[10].ToString();
            RO12.Text = reflect2[11].ToString();
            RO13.Text = reflect2[12].ToString();
        }


    }
}
