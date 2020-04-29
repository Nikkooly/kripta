using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace ConsoleApp2
{
    class AESAlgorithm
    {
        private static readonly byte[] Salt =new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };
        private static byte[] CreateKey( int keyBytes = 32)
        {
            String password = "Type Of day must be pleasant now";
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, Salt, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }
        public byte[] ToAes256()
        {
            String s = "";
            StreamReader sr = new StreamReader("in.txt");
            while (!sr.EndOfStream)
            {
                s += sr.ReadLine();
            }
            sr.Close();
            //Объявляем объект класса AES
            Aes aes = Aes.Create();
            //Генерируем соль
            aes.GenerateIV();
            //Присваиваем ключ. aeskey - переменная (массив байт), сгенерированная методом GenerateKey() класса AES
            aes.Key = CreateKey();
            byte[] encrypted;
            ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(s);
                    }
                }
                //Записываем в переменную encrypted зашиврованный поток байтов
                encrypted = ms.ToArray();
            }
            Console.WriteLine(Encoding.Default.GetString(encrypted.Concat(aes.IV).ToArray()));
            //Возвращаем поток байт + крепим соль
            return encrypted.Concat(aes.IV).ToArray();
            
        }
        public string FromAes256(byte[] shifr)
        {
            byte[] bytesIv = new byte[16];
            byte[] mess = new byte[shifr.Length - 16];

            //Списываем соль
            for (int i = shifr.Length - 16, j = 0; i < shifr.Length; i++, j++)
                bytesIv[j] = shifr[i];

            //Списываем оставшуюся часть сообщения
            for (int i = 0; i < shifr.Length - 16; i++)
                mess[i] = shifr[i];

            //Объект класса Aes
            Aes aes = Aes.Create();
            //Задаем тот же ключ, что и для шифрования
            aes.Key = CreateKey();
            //Задаем соль
            aes.IV = bytesIv;
            //Строковая переменная для результата
            string text = "";
            byte[] data = mess;
            ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        //Результат записываем в переменную text в вие исходной строки
                        text = sr.ReadToEnd();
                    }
                }
            }
            Console.WriteLine($"Расшифрованный текст: {text}");
            return text;
        }

    }
}
