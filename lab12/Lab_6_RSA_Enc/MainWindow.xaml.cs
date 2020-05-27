using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Lab_6_RSA_Enc
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
            Regex regex = new Regex("[^a-zA-Z]+");
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
            if (RichText.GetText(RichTextOrig) != String.Empty && TextP.Text != String.Empty && TextQ.Text != String.Empty && TextE.Text != String.Empty)
            {
                int p = Int32.Parse(TextP.Text);
                int q = Int32.Parse(TextQ.Text);
                int E = Int32.Parse(TextE.Text);
                if (IsSimple(p) && IsSimple(q))
                {
                    int n = p * q;
                    int fn = (p - 1) * (q - 1);
                    if (E < n && NOD(E, fn) == 1)
                    {
                        int d = 0;
                        while (((d * E) % fn) != 1)
                        {
                            d++;
                        }
                        string text = RichText.GetText(RichTextOrig);
                        text = text.Substring(0, text.Length - 2);

                        var md5 = MD5.Create();
                        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                        text = Convert.ToBase64String(hash);

                        string encText = "";
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (alphabet.Contains(text[i]))
                            {
                                encText += ((int)(Math.Pow(LetterNumber(text[i]), E) % n)).ToString() + " ";
                            }
                        }

                        RichText.SetText(RichTextEnc, "S: " + encText + ", H: " + text);
                    }
                    else
                    {
                        MessageBox.Show("Коэффициент e не является взаимнопростым с f(n). Исправьте это");
                    }
                }
                else
                {
                    MessageBox.Show("Числа p и q не являются простыми. Исправьте это");
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
