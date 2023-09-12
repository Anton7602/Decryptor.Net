using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Decryptor.Models;

namespace Decryptor.Controllers
{
    public class UploadController : Controller
    {
        public static bool CorrecrFileUploaded { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            Processor.NewProcessor();
            return View();
        }
        [HttpGet]
        public ActionResult Encrypt()
        {
            Processor.CurrentOperation = Operation.Encrypt;
            return Redirect("/Upload/Index");
        }
        [HttpGet]
        public ActionResult Decrypt()
        {
            Processor.CurrentOperation = Operation.Decrypt;
            return Redirect("/Upload/Index");
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                Processor.NameOfInputFile = Path.GetFileName(file.FileName);
                Processor.PathToInputFile = Path.Combine(Server.MapPath("~/App_Data"), Processor.NameOfInputFile);
                if (Processor.NameOfInputFile==""||Processor.NameOfInputFile==null)
                {
                    ViewBag.Message = $"Выберите файл чтобы продолжить";
                    return View("~/Views/Upload/Index.cshtml");
                }
                if (Processor.NameOfInputFile.EndsWith(".txt") || Processor.NameOfInputFile.EndsWith(".docx") || Processor.NameOfInputFile.EndsWith(".doc"))
                {
                    if (file.ContentLength > 0)
                    {
                        file.SaveAs(Processor.PathToInputFile);
                    }
                }
                else
                {
                    ViewBag.Message = $"Файл имеет неподдерживаемый формат {Processor.NameOfInputFile.Substring(Processor.NameOfInputFile.LastIndexOf("."))}";
                    return View("~/Views/Upload/Index.cshtml");
                }
                if (Processor.NameOfInputFile.EndsWith(".txt"))
                {
                    Processor.InputText = Processor.ReadTXTFile(Processor.CheckFileEncoding());
                }
                if (Processor.NameOfInputFile.EndsWith(".doc") || Processor.NameOfInputFile.EndsWith(".docx"))
                {
                    Processor.InputText = Processor.ReadDOCXFile();
                }
                return Redirect("/Alphabet/Index");
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Загрузка файла не удалась:  " + e.Message;
                return View("~/Views/Upload/Index.cshtml");
            }
        }
    }
}