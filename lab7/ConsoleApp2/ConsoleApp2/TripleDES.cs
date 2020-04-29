using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class TripleDES
    {
        public String s = "";
        public void Apply3DES()
        {
            try
            {
               
                // Создание 3DES который генерирует новй ключ и инициализирует вектор IV.  
                // Этот ключ будет использоваться для расшифрования и зашифрования 
                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    byte[] encrypted = Encrypt(tdes.Key, tdes.IV);
                    Console.WriteLine($"Зашифрованный текст: {System.Text.Encoding.UTF8.GetString(encrypted)}");
                    // Decrypt the bytes to a string. 
                    string decrypted = Decrypt(encrypted,tdes.Key, tdes.IV);
                    Console.WriteLine($"Расшифрованный текст: {decrypted}"); 
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
       public byte[] Encrypt(byte[] Key, byte[] IV)
        {
            s = "";
             StreamReader sr = new StreamReader("in.txt");
                while (!sr.EndOfStream)
                {
                    s += sr.ReadLine();
                }
                sr.Close();
            byte[] encrypted;
            // Создаем новый TripleDESCryptoServiceProvider.  
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                // Создаем encryptor 
                ICryptoTransform encryptor = tdes.CreateEncryptor(Key, IV);
                // Создаем MemoryStream  
                using (MemoryStream ms = new MemoryStream())
                {
                    // Создаем crypto stream использующий the CryptoStream class. Этот класс является ключом шифрования  
                    // и используется для шифрования и расшифрования данных полученных с потока. 
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream  
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(s);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public string Decrypt(byte[] encrypted, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create TripleDESCryptoServiceProvider  
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                // Create a decryptor  
                ICryptoTransform decryptor = tdes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.  
                using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    // Create crypto stream  
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream  
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}
