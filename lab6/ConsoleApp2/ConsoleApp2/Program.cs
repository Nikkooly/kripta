using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            for (; ; )
            {
                EnigmaMachine machine = new EnigmaMachine();
                EnigmaSettings eSettings = new EnigmaSettings();

                querySettings(eSettings);

                string message = "";
                Console.Write("Enter message to encrypt: ");
                message = Console.ReadLine();
                while (!Regex.IsMatch(message, @"^[a-zA-Z ]+$"))
                {
                    Console.Write("Only letters A-Z is allowed, try again: ");
                    message = Console.ReadLine();
                }
                message = message.Replace(" ", "").ToUpper();

                // Задаем настройки машины
                machine.setSettings(eSettings.rings, eSettings.grund, eSettings.order, eSettings.reflector);

                // The plugboard settings
                foreach (string plug in eSettings.plugs)
                {
                    char[] p = plug.ToCharArray();
                    machine.addPlug(p[0], p[1]);
                }
                Console.WriteLine();
                Console.WriteLine("Plain text:\t" + message);
                string enc = machine.runEnigma(message);
                Console.WriteLine("Encrypted:\t" + enc);

                // Сбрасываем настройки перед дешифрованием
                machine.setSettings(eSettings.rings, eSettings.grund, eSettings.order, eSettings.reflector);
                string dec = machine.runEnigma(enc);
                Console.WriteLine("Decrypted:\t" + dec);
                Console.WriteLine();

                Console.ReadLine();
            }
        }

        private static void querySettings(EnigmaSettings e)
        {
            string r;
            Console.WriteLine("Enigma Machine Emulator\n");
                e.setDefault();
            Console.WriteLine();
        }
        private class EnigmaSettings
        {
            public char[] rings { get; set; }
            public char[] grund { get; set; }
            public string order { get; set; }
            public char reflector { get; set; }
            public List<string> plugs = new List<string>();

            public EnigmaSettings()
            {
                setDefault();
            }

            public void setDefault()
            {
                rings = new char[] { 'A', 'A', 'A' };
                grund = new char[] { 'A', 'A', 'A' };
                order = "III-Gamma-V";
                reflector = 'B';
                plugs.Clear();
            }
        }
    }
}
