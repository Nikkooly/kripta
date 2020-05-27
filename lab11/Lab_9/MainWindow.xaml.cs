using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string AND(string a, string b)
        {
            string temp = "";
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] == '1' & b[i] == '1'))
                {
                    temp += "1";
                }
                else
                {
                    temp += "0";
                }
            }
            return temp;
        }

        private string OR(string a, string b)
        {
            string temp = "";
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] == '0' & b[i] == '0'))
                {
                    temp += "0";
                }
                else
                {
                    temp += "1";
                }
            }
            return temp;
        }

        private string NOT(string a)
        {
            string temp = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == '0')
                {
                    temp += "1";
                }
                else
                {
                    temp += "0";
                }
            }
            return temp;
        }

        private string XOR(string a, string b)
        {
            string temp = "";
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] == '1' & b[i] == '1') || (a[i] == '0' && b[i] == '0'))
                {
                    temp += "0";
                }
                else
                {
                    temp += "1";
                }
            }
            return temp;
        }

        private int BinToDec(string bin)
        {
            int dec = 0;
            for (int i = 0; i < bin.Length; i++)
            {
                dec += (int)(Math.Pow(2, bin.Length - i - 1) * Int32.Parse(bin[i].ToString()));
            }
            return dec;
        }

        private string BinToHex(string bin)
        {
            string hex = "";
            for (int i = 0; i < bin.Length / 4; i++)
            {
                if (bin.Substring(i * 4, 4) == "0000") hex += "0";
                else if (bin.Substring(i * 4, 4) == "0001") hex += "1";
                else if (bin.Substring(i * 4, 4) == "0010") hex += "2";
                else if (bin.Substring(i * 4, 4) == "0011") hex += "3";
                else if (bin.Substring(i * 4, 4) == "0100") hex += "4";
                else if (bin.Substring(i * 4, 4) == "0101") hex += "5";
                else if (bin.Substring(i * 4, 4) == "0110") hex += "6";
                else if (bin.Substring(i * 4, 4) == "0111") hex += "7";
                else if (bin.Substring(i * 4, 4) == "1000") hex += "8";
                else if (bin.Substring(i * 4, 4) == "1001") hex += "9";
                else if (bin.Substring(i * 4, 4) == "1010") hex += "A";
                else if (bin.Substring(i * 4, 4) == "1011") hex += "B";
                else if (bin.Substring(i * 4, 4) == "1100") hex += "C";
                else if (bin.Substring(i * 4, 4) == "1101") hex += "D";
                else if (bin.Substring(i * 4, 4) == "1110") hex += "E";
                else hex += "F";
            }
            return hex;
        }

        private void Hash(object sender, RoutedEventArgs e)
        {
            if (RichText.GetText(RichTextOrig) != String.Empty)
            {
                string hashtext = "";
                string text = RichText.GetText(RichTextOrig).Substring(0, RichText.GetText(RichTextOrig).Length - 2);
                int textLength = text.Length;
                string endBE = Convert.ToString(textLength, 2).PadLeft(64, '0');
                //string endBE = end.Substring(48, 16) + end.Substring(32, 16) + end.Substring(16, 16) + end.Substring(0, 16);
                string binText = "";
                var letters = Encoding.ASCII.GetBytes(text);
                foreach (int letter in letters)
                {
                    binText += Convert.ToString(letter, 2).PadLeft(8, '0');
                }
                int blockCount;
                string[] blocks;
                binText += "1";
                if (binText.Length % 512 <= 448)
                {
                    blockCount = binText.Length / 512 + 1;
                }
                else
                {
                    blockCount = binText.Length / 512 + 2;
                }
                blocks = new string[blockCount];
                binText = binText.PadRight(blockCount * 512 - 64, '0');
                binText += endBE;

                for (int i = 0; i < blockCount; i++)
                {
                    blocks[i] = binText.Substring(i * 512, 512);
                }

                string h0 = "01100111010001010010001100000001";
                string h1 = "11101111110011011010101110001001";
                string h2 = "10011000101110101101110011111110";
                string h3 = "00010000001100100101010001110110";
                string h4 = "11000011110100101110000111110000";

                for (int i = 0; i < blocks.Length; i++)
                {
                    string[] blocks32 = new string[16];
                    for (int j = 0; j < blocks[i].Length / 32; j++)
                    {
                        blocks32[j] = blocks[i].Substring(j * 32, 32);
                    }

                    string[] W = new string[80];
                    for (int t = 0; t < 80; t++)
                    {
                        if (t >= 0 && t <= 15)
                        {
                            W[t] = blocks32[t];
                        }
                        else
                        {
                            W[t] = XOR(XOR(XOR(W[t - 3], W[t - 8]), W[t - 14]), W[t - 16]);
                            W[t] = W[t].Substring(1, 31) + W[t].Substring(31, 1);
                        }
                    }

                    string A = h0;
                    string B = h1;
                    string C = h2;
                    string D = h3;
                    string E = h4;

                    for (int t = 0; t < 80; t++)
                    {
                        string F = "", K = "";
                        if (0 <= t && t <= 19)
                        {
                            F = OR((AND(B, C)), (AND((NOT(B)), D)));
                            K = "01011010100000100111100110011001";
                        }
                        else if (0 <= 20 && t <= 39)
                        {
                            F = XOR(XOR(B, C), D);
                            K = "01101110110110011110101110100001";
                        }
                        else if (0 <= 40 && t <= 59)
                        {
                            F = OR(OR(AND(B, C), AND(B, D)), AND(C, D));
                            K = "10001111000110111011110011011100";
                        }
                        else
                        {
                            F = XOR(XOR(B, C), D);
                            K = "11001010011000101100000111010110";
                        }
                        string Temp = Convert.ToString((BinToDec(A.Substring(5, 32 - 5) + A.Substring(0, 5)) + BinToDec(F) + BinToDec(E) + BinToDec(K) + BinToDec(W[t])), 2).PadLeft(32, '0');
                        E = D.PadLeft(32, '0');
                        D = C.PadLeft(32, '0');
                        C = B.Substring(30, 32 - 30) + A.Substring(0, 30).PadLeft(32, '0');
                        B = A.PadLeft(32, '0');
                        A = Temp.PadLeft(32, '0');

                        h0 = Convert.ToString((BinToDec(h0) + BinToDec(A)), 2).PadLeft(32, '0');
                        h1 = Convert.ToString((BinToDec(h1) + BinToDec(B)), 2).PadLeft(32, '0');
                        h2 = Convert.ToString((BinToDec(h2) + BinToDec(C)), 2).PadLeft(32, '0');
                        h3 = Convert.ToString((BinToDec(h3) + BinToDec(D)), 2).PadLeft(32, '0');
                        h4 = Convert.ToString((BinToDec(h4) + BinToDec(E)), 2).PadLeft(32, '0');
                    }
                    hashtext += BinToHex(h0) + " " + BinToHex(h1) + " " + BinToHex(h2) + " " + BinToHex(h3) + " " + BinToHex(h4) + " ";
                }
                RichText.SetText(RichTextHash, hashtext);
            }
        }
    }

    public static class RichText
    {
        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }
}
