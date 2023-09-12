using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Decryptor.Models;

namespace Decryptor.Controllers
{
    public class ProcessController : Controller
    {
        public static string ProcessSystemMessage { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Encrypt()
        {
            Processor.NewProcessor();
            Processor.CurrentOperation = Operation.Encrypt;
            return Redirect("/Alphabet/Index");
        }
        public ActionResult Decrypt()
        {
            Processor.NewProcessor();
            Processor.CurrentOperation = Operation.Decrypt;
            return Redirect("/Alphabet/Index");
        }
        public void GenerateRandomKey()
        {
            Processor.GenerateRandomKey();
            //return EmptyResult;
        }
        public ActionResult DoOperation(string key, string inputText, string process)
        {
            if (process== "Сгенерировать ключ")
            {
                Processor.GenerateRandomKey();
                Processor.InputText = inputText;
                return Redirect("/Process/Index");
            }
            if (key=="" || key==null)
            {
                ProcessSystemMessage = "С пустым ключом текст останется незашифрованным. Оно вам надо?";
                return Redirect("/Process/Index");
            }
            if (inputText=="" || inputText== null)
            {
                ProcessSystemMessage = "Введите текст для шифрования";
                return Redirect("/Process/Index");
            }
            Processor.InputText = inputText;
            foreach(char symbol in key)
            {
                if (!Processor.EncryptionAlphabet.Contains(symbol.ToString().ToLower()))
                {
                    ProcessSystemMessage = "Ключ должен состоять только из символов алфавита Шифрования";
                    return Redirect("/Process/Index");
                }

            }
            Processor.KeyWord = key;
            Processor.OutputText = "";
            Processor.DoOperation();
            return Redirect("/Download/Index");
        }
    }
}