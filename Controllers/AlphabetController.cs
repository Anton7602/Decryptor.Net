using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Decryptor.Models;

namespace Decryptor.Controllers
{
    public class AlphabetController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FormAlphabet(bool cyrillic, bool latin, bool numeric, bool punctuation, bool spaces, bool additional, string additionalSymbols )
        {
            Processor.EncryptionAlphabet = "";
            if (cyrillic)
            {
                Processor.EncryptionAlphabet += "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }
            if (latin)
            {
                Processor.EncryptionAlphabet += "abcdefghijklmnopqrstuvwxyz";
            }
            if (numeric)
            {
                Processor.EncryptionAlphabet += "0123456789";
            }
            if (punctuation)
            {
                Processor.EncryptionAlphabet += $@"!#$%&'()*+,-./:;<=>?[\^_`|~";
            }
            if (spaces)
            {
                Processor.EncryptionAlphabet += " ";
            }
            if (additional)
            {
                try
                {
                    foreach (char symbol in additionalSymbols)
                    {
                        if (!Processor.EncryptionAlphabet.Contains(symbol))
                        {
                            Processor.EncryptionAlphabet += symbol;
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Добавлены недопустимые символы" + $"{e.Message}";
                }
            }
            ProcessController.ProcessSystemMessage = "";
            return Redirect("/Process/Index");
        }
    }
}