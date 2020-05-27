using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Lab_6_EG_Enc
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

        static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        private static bool IsSimple(int n)
        {
            for (int i = 2; i <= (int)(n / 2); i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void LetterValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private int NOD(int first, int second)
        {
            int a, b, q, r = 1;
            if (first >= second)
            {
                a = first;
                b = second;
            }
            else
            {
                a = second;
                b = first;
            }
            while (r != 0)
            {
                q = (int)(a / b);
                r = (a % b);
                a = b;
                b = r;
            }
            return a;
        }

        private static int LetterNumber(char letter)
        {
            int number = 0;
            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i] == letter)
                {
                    number = i;
                }
            }
            return number;
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            RichTextEnc.Document.Blocks.Clear();
            if (RichText.GetText(RichTextOrig) != String.Empty && TextP.Text != String.Empty && TextG.Text != String.Empty && TextC.Text != String.Empty)
            {

                Int32 FirstPartOfThePublicKey = Int32.Parse(TextP.Text);
                Int32 SecondPartOfThePublicKey = Int32.Parse(TextG.Text);
                Int32 SecretKey = Int32.Parse(TextC.Text);
                BigInteger ThirdPartOfThePublicKey = BigInteger.Pow(SecondPartOfThePublicKey, SecretKey) % FirstPartOfThePublicKey;

                if (IsSimple(FirstPartOfThePublicKey) && SecondPartOfThePublicKey < (FirstPartOfThePublicKey - 1) && SecretKey < (FirstPartOfThePublicKey - 1))
                {
                    string text = RichText.GetText(RichTextOrig).Substring(0, RichText.GetText(RichTextOrig).Length - 2);

                    var md5 = MD5.Create();
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                    text = Convert.ToBase64String(hash);

                    Random tempRandom = new Random();
                    Int32 SessionKey = (42 + tempRandom.Next()) % (Math.Abs(Convert.ToInt32(FirstPartOfThePublicKey - SecondPartOfThePublicKey))) + 6;

                    string encText = "";
                    BigInteger FirstPartOfTheCipgertext = BigInteger.Pow(SecondPartOfThePublicKey, SessionKey) % FirstPartOfThePublicKey;
                    for (int i = 0; i < text.Length; i++)
                    {
                        BigInteger Message = (BigInteger)(LetterNumber(text[i]));
                        BigInteger SecondPartOfTheCipgertext = (BigInteger.Pow(ThirdPartOfThePublicKey, SessionKey) * Message) % FirstPartOfThePublicKey;
                        encText += "(" + FirstPartOfTheCipgertext.ToString() + "," + SecondPartOfTheCipgertext.ToString() + ") ";
                    }
                    RichText.SetText(RichTextEnc, "S: " + encText + ", H: " + text);
                }
                else
                {
                    MessageBox.Show("Число p не является простым или коэффициенты ошибочны. Исправьте это");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
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
