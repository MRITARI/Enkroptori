using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Enkröptori
{
    internal class Program
    {
        static string history = "";
        [STAThread] //joku ätrribyytti entiiö ei pysty kopioimaa muute :DD
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            
            
            
            Console.Title = "Enkröptori";
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "-h":
                    case "--help":
                        PrintHelp();
                        return;

                    case "-v":
                    case "--version":
                        Console.WriteLine("Software Version: 1.0.0");
                        return;

                    case "-e":
                    case "--encrypt":
                        Encrypting();

                        return;

                    case "-d":
                    case "--decrypt":
                        Decrypting();

                        return;

                    case "-c":
                    case "--clearhistory":
                        ClearFile();
                        Console.WriteLine("Encryption history cleared.");
                        return;

                    case "-hs":
                    case "--history":
                        string readText = File.ReadAllText("encrypthistory.txt");
                        Console.WriteLine(readText);
                        return;

                    default:
                        Console.WriteLine($"Unknown argument: {args[0]}");
                        return;
                
                }
            }

            start();

            /*
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua
            */

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"[Enkröptori]> ");
                Console.ForegroundColor = ConsoleColor.White;
                string input = Console.ReadLine();
                try
                {
                    if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                    {
                        continue;
                    }
                    else if (input == "startscreen")
                    {
                        start();
                    }
                    else if (input == "history")
                    {
                        string readText = File.ReadAllText("encrypthistory.txt");
                        Console.WriteLine(readText);
                    }
                    else if (input == "clearhistory")
                    {
                        ClearFile();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("History cleared successfully.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (input == "help")
                    {
                        Console.WriteLine(@"
=========Available commands=========
startscreen
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
clearhistory
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
history
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
decrypt
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
encrypt
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
clear
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
exit
====================================");

                    }
                    else if (input == "encrypt")
                    {
                        Encrypting();
                    }
                    else if (input == "decrypt")
                    {
                        Decrypting();
                    }
                    else if (input == "clear")
                    {
                        Console.Clear();
                    }
                    else if (input == "exit")
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Exiting in 3 seconds...");
                        System.Threading.Thread.Sleep(3000);
                        Console.ForegroundColor = ConsoleColor.White;
                        Environment.Exit(0);
                        
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"'{input}' Invalid command. Type 'help' to see available commands.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        static void ClearFile()
        {
            if (!File.Exists("encrypthistory.txt"))
                File.Create("encrypthistory.txt");

            TextWriter tw = new StreamWriter("encrypthistory.txt", false);
            tw.Write(string.Empty);
            tw.Close();
        }
        static void start()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
                 _____       _        _   _       _             _     ____  
                | ____|_ __ | | ___ _(_)_(_)_ __ | |_ ___  _ __(_)  _|  _ \ 
                |  _| | '_ \| |/ / '__/ _ \| '_ \| __/ _ \| '__| | (_) | | |
                | |___| | | |   <| | | (_) | |_) | || (_) | |  | |  _| |_| |
                |_____|_| |_|_|\_\_|  \___/| .__/ \__\___/|_|  |_| (_)____/ 
                www.asciiart.eu            |_|                              
                                           V.1.0  Mico Ritari  24.3.2025");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tEnkröptori is a file encryption program that allows you to encrypt and decrypt files.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t\t\tType 'help' to see available commands.");
            Console.WriteLine("\t\t\t\tPress any key to continue...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey(true);
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
            return;
        }
        /*==============================enkryptaaminen ja dekryptaaminen==================================*/
        private static byte[] GenerateRandomKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        private static byte[] GenerateRandomIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }

        private static byte[] GetUserKey()
        {
            Console.Write("Enter your AES key (Base64, 32 bytes): ");
            string keyInput = Console.ReadLine();
            return Convert.FromBase64String(keyInput);
        }

        private static byte[] GetUserIV()
        {
            Console.Write("Enter your IV (Base64, 16 bytes): ");
            string ivInput = Console.ReadLine();
            return Convert.FromBase64String(ivInput);
        }

        private static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fileStream = new FileStream(outputFile, FileMode.Create))
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                {
                    inputStream.CopyTo(cryptoStream);
                }
            }
        }
        private static void Encrypting()
        {
            try
            {
                Console.Write("Enter the file path to encrypt: ");
                string inputFile = Console.ReadLine();
                string directory = Path.GetDirectoryName(inputFile);
                string encryptedFile = directory + @"\" + Path.GetFileNameWithoutExtension(inputFile) + ".aes";//vittu mitä sähläystä
                byte[] key = GenerateRandomKey();//avaimet
                byte[] iv = GenerateRandomIV();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n=== SAVE THESE CREDENTIALS ===");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(encryptedFile);
                Console.WriteLine("AES Key (Base64): " + Convert.ToBase64String(key));//muunnetaan base64 että on helpompi käyttää
                Console.WriteLine("AES IV  (Base64): " + Convert.ToBase64String(iv));
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("================================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("WARNING: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("If you lose this key and IV you will NOT be able to decrypt the file!\n");
                Console.ForegroundColor = ConsoleColor.Green;
                string clipboard = ($"=== SAVE THESE CREDENTIALS ===\n{encryptedFile}\nAES Key (Base64): {Convert.ToBase64String(key)}\nAES IV  (Base64): {Convert.ToBase64String(iv)}\n================================\nWARNING: If you lose this key and IV you will NOT be able to decrypt the file!");
                Clipboard.SetText(clipboard);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Credentials copied to clipboard.");
                Console.ForegroundColor = ConsoleColor.Green;
                EncryptFile(inputFile, encryptedFile, key, iv);
                Console.WriteLine($"File encrypted successfully: {encryptedFile}");
                Console.ForegroundColor = ConsoleColor.White;
                System.IO.File.Delete(inputFile);
                string encrypteddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                history = ($@"
Encrypted successfully

FILE = {encryptedFile}  

DATE & TIME = {encrypteddate}  
----------------------------------");
                File.AppendAllText("encrypthistory.txt", history);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
                return;


            }
        }
        private static void Decrypting()
        {
            try
            {
                Console.Write("Enter the encrypted file path: ");
                string encryptedFile = Console.ReadLine();
                Console.Write("Enter filetype (example: .txt/.pdf): ");
                string fileextension = Console.ReadLine();
                string decryptedFile = encryptedFile.Replace(".aes", fileextension);

                byte[] key = GetUserKey();
                byte[] iv = GetUserIV();
                Console.ForegroundColor = ConsoleColor.Green;
                DecryptFile(encryptedFile, decryptedFile, key, iv);
                Console.WriteLine($"File decrypted successfully: {decryptedFile}");
                Console.ForegroundColor = ConsoleColor.White;
                string decrypteddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                System.IO.File.Delete(encryptedFile);
                history = ($@"
Decrypted successfully

FILE = {decryptedFile}  

DATE & TIME = {decrypteddate}  
----------------------------------");
                File.AppendAllText("encrypthistory.txt", history);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
        }

        private static void DecryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fileStream = new FileStream(outputFile, FileMode.Create))
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                {
                    inputStream.CopyTo(cryptoStream);
                }
            }
        }
        /*==============================ARGS LOL==================================*/
        static void PrintHelp()
        {
            Console.WriteLine("Usage: software.exe [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help          |  Show this help message");
            Console.WriteLine("  -v, --version       |  Show the software version");
            Console.WriteLine("  -e, --encrypt       |  Encrypt a file");
            Console.WriteLine("  -d, --decrypt       |  Decrypt a file");
            Console.WriteLine("  -c, --clearhistory  |  Clear the encryption history");
            Console.WriteLine("  -hs, --history      |  Show the encryption history");

        }
    }
}
/*
 SONNET 1

From fairest creatures we desire increase,
That thereby beauty's rose might never die,
But as the riper should by time decease,
His tender heir might bear his memory:
But thou, contracted to thine own bright eyes,
Feed'st thy light'st flame with self-substantial fuel,
Making a famine where abundance lies,
Thyself thy foe, to thy sweet self too cruel.
Thou that art now the world's fresh ornament
And only herald to the gaudy spring,
Within thine own bud buriest thy content
And, tender churl, makest waste in niggarding.
Pity the world, or else this glutton be,
To eat the world's due, by the grave and thee.


https://www.opensourceshakespeare.org
*/

