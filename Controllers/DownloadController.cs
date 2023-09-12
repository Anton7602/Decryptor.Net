using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Decryptor.Models;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace Decryptor.Controllers
{
    public class DownloadController : Controller
    {
        public ActionResult Index()
        {
            string path = Server.MapPath("~/App_Data/");
            Processor.DeleteAllFiles(path);
            CompileTXTFile();
            CompileDOCXFile();
            DirectoryInfo filesFolder = new DirectoryInfo(path);
            FileInfo[] files = filesFolder.GetFiles("*.*");
            List<string> listOfFiles = new List<string>(files.Length);
            foreach (var file in files)
            {
                listOfFiles.Add(file.Name);
            }
            return View(listOfFiles);
        }
        public ActionResult DownloadFile(string filename)
        {
            if (Path.GetExtension(filename) == ".txt" || Path.GetExtension(filename) == ".docx")
            {
                string filePath = Path.Combine(Server.MapPath("~/App_Data/"), filename);
                return File(filePath, "App_Data/", filename);
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
        public void CompileTXTFile()
        {
            string path = Server.MapPath("~/App_Data/");
            string outText = Processor.OutputText;
            if (Processor.NameOfInputFile == "" || Processor.NameOfInputFile == null)
            {
                Processor.NameOfInputFile = "Result.txt";
            }
            string outputFileName = Processor.NameOfInputFile.Substring(0, Processor.NameOfInputFile.LastIndexOf("."));
            System.IO.File.WriteAllText($"{path}{outputFileName}.txt", outText);
        }
        public void CompileDOCXFile()
        {
            Word.Application application = new Word.Application();
            Word.Document document = new Word.Document();
            document.Content.SetRange(0, 0);
            document.Content.Text = Processor.OutputText;
            string path = Server.MapPath("~/App_Data/");
            if (Processor.NameOfInputFile == "" || Processor.NameOfInputFile == null)
            {
                Processor.NameOfInputFile = "Result.docx";
            }
            string outputFileName = Processor.NameOfInputFile.Substring(0, Processor.NameOfInputFile.LastIndexOf("."));
            document.SaveAs2($@"{path}{outputFileName}.docx");
            document.Close();
            application.Quit();
        }
    }
}