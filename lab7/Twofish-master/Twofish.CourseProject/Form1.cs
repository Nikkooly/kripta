using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwofishManaged;

namespace Twofish.CourseProject
{
	public partial class Form1 : Form
	{
        byte[] decryptedText;
        byte[] encryptedText;

        string encryptPath;
        string decryptPath;
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            try
            {
                byte[] mmkey = Encoding.Unicode.GetBytes(textBox3.Text);
                var mtwM = new MyTwofishManagedTransform(mmkey);

                if (checkBox2.Checked)
                {
                    string extension = Path.GetExtension(decryptPath);
                    if (extension == ".txt")
                    {
                        string str = File.ReadAllText(decryptPath);
                        decryptedText = Encoding.Unicode.GetBytes(str);
                    }
                    else
                    {
                        Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document document = application.Documents.Open(decryptPath);
                        decryptedText = Encoding.Unicode.GetBytes(document.Content.Text);

                        application.Quit();
                    }

                }
                else
                {
                    decryptedText = Encoding.Unicode.GetBytes(textBox1.Text);
                }

                encryptedText = mtwM.Encrypt(decryptedText, 0, decryptedText.Count());
                if (checkBox1.Checked)
                {
                    string extension = Path.GetExtension(encryptPath);
                    if (extension == ".txt")
                    {
                        File.WriteAllText(encryptPath, Encoding.Unicode.GetString(encryptedText));
                    }
                    else
                    {
                        Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(encryptPath);
                        doc.Content.Text = Encoding.Unicode.GetString(encryptedText);
                        doc.Save();
                        app.Quit();

                    }

                }
                else
                {
                    textBox2.Text = Encoding.Unicode.GetString(encryptedText);
                }
                MessageBox.Show("Encrypted text is writed!");
            }
            catch (ArgumentOutOfRangeException eq)
            {
                MessageBox.Show("Invalid input text");
            }
            catch (Exception eq) {
                MessageBox.Show("Error! =(");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            byte[] mmkey = Encoding.Unicode.GetBytes(textBox4.Text);
            var mtwM = new MyTwofishManagedTransform(mmkey);


            try
            {
                if (checkBox1.Checked)
                {
                    string extension = Path.GetExtension(encryptPath);
                    if (extension == ".txt")
                    {
                        string text = File.ReadAllText(encryptPath);
                        encryptedText = Encoding.Unicode.GetBytes(text);
                    }
                    else
                    {
                        Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document document = application.Documents.Open(encryptPath);
                        string t = document.Content.Text;
                        t = t.Remove(t.Count() - 1, 1);
                        encryptedText = Encoding.Unicode.GetBytes(t);

                        application.Quit();
                    }
                }
                else
                {
                    encryptedText = Encoding.Unicode.GetBytes(textBox2.Text);
                }

                decryptedText = mtwM.Decrypt(encryptedText, 0, encryptedText.Count());
                if (checkBox2.Checked)
                {
                    string extension = Path.GetExtension(decryptPath);
                    if (extension == ".txt")
                    {
                        File.WriteAllText(decryptPath, Encoding.Unicode.GetString(decryptedText));
                    }
                    else
                    {
                        Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(decryptPath);
                        doc.Content.Text = Encoding.Unicode.GetString(decryptedText);
                        doc.Save();
                        app.Quit();

                    }
                }
                else
                {
                    textBox1.Text = Encoding.Unicode.GetString(decryptedText);
                }

                MessageBox.Show("Decrypted text is writed!");
            }
            catch (ArgumentOutOfRangeException eq)
            {
                MessageBox.Show("Invalid input text");
            }
            catch (Exception eq) {
                MessageBox.Show("Error! =(");
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Text files | *.txt| Document Word | *.doc; *.docx";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                decryptPath = dialog.FileName;

            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (button3.Enabled)
            {
                button3.Enabled = false;
                textBox2.Enabled = true;
            }
            else
            {
                button3.Enabled = true;
                textBox2.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (button4.Enabled)
            {
                button4.Enabled = false;
                textBox1.Enabled = true;
            }
            else
            {
                button4.Enabled = true;
                textBox1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Text files | *.txt| Document Word | *.doc; *.docx";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                encryptPath = dialog.FileName;
                string extension = Path.GetExtension(encryptPath); 
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
	
}
