using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Office.Interop.Word;

namespace Decryptor.Models
{
    public enum Operation {Encrypt, Decrypt}
    public abstract class Processor
    {
        public static string EncryptionAlphabet { get; set; }
        public static string PathToInputFile { get; set; }
        public static string NameOfInputFile { get; set; }
        public static string KeyWord { get; set; }
        public static string InputText { get; set; }
        public static string OutputText { get; set; }
        public static Operation CurrentOperation { get; set; }

        public static void NewProcessor()
        {
            EncryptionAlphabet = "";
            PathToInputFile = "";
            NameOfInputFile = "";
            KeyWord = "";
            InputText = "";
            OutputText = "";
        }
        public static string ReadTXTFile(string inputFileEncoding)
        {
            StreamReader txtReader;
            switch (inputFileEncoding)
            {
                case "ANSI":
                    txtReader = new StreamReader(PathToInputFile, Encoding.Default);
                    break;
                case "UTF8":
                    txtReader = new StreamReader(PathToInputFile, Encoding.UTF8);
                    break;
                default:
                    txtReader = new StreamReader(PathToInputFile, Encoding.Unicode);
                    break;
            }
            string textFromFile = txtReader.ReadToEnd();
            txtReader.Close();
            File.Delete(PathToInputFile);
            return textFromFile;
        }
        public static string ReadDOCXFile()
        {
            Application application = new Application();
            Document document = application.Documents.Open($@"{PathToInputFile}");
            int count = document.Words.Count;
            string textFromFile = "";
            for (int i = 1; i <= count; i++)
            {
                textFromFile += document.Words[i].Text;
            }
            application.Quit();
            return textFromFile;
        }
        public static void GenerateRandomKey()
        {
            string generatedKey = "";
            Random keyLenght = new Random();
            int lenght = keyLenght.Next(10, 40);
            Random keyLetter = new Random();
            for (int i = 0; i < lenght; i++)
            {
                generatedKey += EncryptionAlphabet[keyLetter.Next(0, EncryptionAlphabet.Length)];
            }
            KeyWord = generatedKey;
        }

        public static void DoOperation()
        {
            Processor.OutputText = "";
            string modifiedKey = KeyWord.ToLower();
            List<char> inputLine = new List<char>();
            foreach (char symbol in InputText.ToLower())
            {
                if (EncryptionAlphabet.Contains(symbol))
                {
                    inputLine.Add(symbol);
                }
            }
            while (modifiedKey.Length<inputLine.Count)
            {
                modifiedKey += modifiedKey;
            }
            string DecodedSymbols = "";
            int newIndex = 0;
            if (CurrentOperation == Operation.Decrypt)
            {
                for (int i = 0; i < inputLine.Count; i++)
                {
                    if (EncryptionAlphabet.Contains(inputLine[i]))
                    {
                        newIndex = (EncryptionAlphabet.IndexOf(inputLine[i]) - EncryptionAlphabet.IndexOf(modifiedKey[i]) + EncryptionAlphabet.Length) % EncryptionAlphabet.Length;
                        DecodedSymbols += EncryptionAlphabet[newIndex];
                    }
                }
            }
            else  //CurrentOperation == Operation.Eecrypt
            {
                for (int i = 0; i < inputLine.Count; i++)
                {
                    if (EncryptionAlphabet.Contains(inputLine[i]))
                    {
                        newIndex = (EncryptionAlphabet.IndexOf(inputLine[i]) + EncryptionAlphabet.IndexOf(modifiedKey[i])) % EncryptionAlphabet.Length;
                        DecodedSymbols += EncryptionAlphabet[newIndex];
                    }
                }
            }
            int pointer = 0;
            foreach (char symbol in InputText)
            {
                if (EncryptionAlphabet.Contains(symbol.ToString().ToLower()))
                {
                    if (char.IsUpper(symbol))
                    {
                        OutputText += DecodedSymbols[pointer].ToString().ToUpper();
                        pointer++;
                    }
                    else
                    {
                        OutputText += DecodedSymbols[pointer].ToString();
                        pointer++;
                    }
                }
                else
                {
                    OutputText += symbol;
                }
            }
        }

        public static void DeleteAllFiles(string path)
        {
            string[] listOfFilesInAppData = Directory.GetFiles(path);
            try
            {
                foreach (string file in listOfFilesInAppData)
                {
                    string deleteFile = file.Substring(file.LastIndexOf($@"\")+1);
                    File.Delete($@"{path}{deleteFile}");
                }
            }
            catch { }
        }

        public static string CheckFileEncoding()
        {
            FileStream fs = System.IO.File.OpenRead(PathToInputFile);
            BinaryReader instr = new BinaryReader(fs);
            byte[] data = instr.ReadBytes((int)instr.BaseStream.Length);
            instr.Close();
            if (data.Length > 2 && data[0] == 0xef && data[1] == 0xbb && data[2] == 0xbf)
            {
                if (data.Length != 3) return "UTF8";
                else return "";
            }
            int i = 0;
            while (i < data.Length - 1)
            {
                if (data[i] > 0x7f)
                {
                    if ((data[i] >> 5) == 6)
                    {
                        if ((i > data.Length - 2) || ((data[i + 1] >> 6) != 2))
                            return "ANSI";
                        i++;
                    }
                    else if ((data[i] >> 4) == 14)
                    {
                        if ((i > data.Length - 3) || ((data[i + 1] >> 6) != 2) || ((data[i + 2] >> 6) != 2))
                            return "ANSI"; ;
                        i += 2;
                    }
                    else if ((data[i] >> 3) == 30)
                    {
                        if ((i > data.Length - 4) || ((data[i + 1] >> 6) != 2) || ((data[i + 2] >> 6) != 2) || ((data[i + 3] >> 6) != 2))
                            return "ANSI"; ;
                        i += 3;
                    }
                    else
                    {
                        return "ANSI"; ;
                    }
                }
                i++;
            }
            return "UTF8";
        }
    }
}