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
using System.Numerics;
using Lab_10.Utils;
using Lab_10.Extensions;

namespace Lab_10
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private DomainParameters domain;
        private Schnorr schnorr;
        private SchorrProver prover;
        private SchorrVerifier verifier;

        private BigInteger call;
        private BigInteger challenge;

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            domain = DomainParameters.GenerateDomainParameters(1, 999999);
            schnorr = new Schnorr(domain);

            P.Text = domain.P.ToString();
            Q.Text = domain.Q.ToString();
            H.Text = domain.H.ToString();
            G.Text = domain.G.ToString();
            T.Text = schnorr.T.ToString();

            ResetForm();
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            prover.GenerateKeys();
            S.Text = prover.Secret.ToString();
            A.Text = prover.PublicKey.ToString();
        }

        private void Button3Click(object sender, RoutedEventArgs e)
        {
            prover.Call();
            R.Text = prover.R.ToString();
            X.Text = prover.X.ToString();
        }

        private void Button4Click(object sender, RoutedEventArgs e)
        {
            challenge = verifier.Challenge();
            E.Text = challenge.ToString();
        }

        private void Button5Click(object sender, RoutedEventArgs e)
        {
            BigInteger secret = BigInteger.Parse(S.Text);
            prover.Secret = secret;
            BigInteger ch = BigInteger.Parse(E.Text);
            prover.Response(ch);
            Y.Text = prover.Y.ToString();
        }

        private void Button6Click(object sender, RoutedEventArgs e)
        {
            BigInteger x = BigInteger.Parse(X.Text);
            BigInteger res = BigInteger.Parse(Y.Text);
            BigInteger pub = BigInteger.Parse(A.Text);
            verifier.X = x;
            bool result = verifier.VerifyResponse(pub, res);

            Z.Text = verifier.Z.ToString();
            if (result)
            {
                MessageBox.Show("Доказано");
            }
            else
            {
                MessageBox.Show("НЕ доказано");
            }
        }

        private void ResetForm()
        {
            schnorr.T = BigInteger.Parse(T.Text);

            prover = new SchorrProver(schnorr);
            verifier = new SchorrVerifier(schnorr);

            S.Text = "";
            A.Text = "";
            R.Text = "";
            X.Text = "";
            E.Text = "";
            Y.Text = "";
            Z.Text = "";
        }
    }
}
