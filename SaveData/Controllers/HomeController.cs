using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SaveData.Models;

namespace SaveData.Controllers
{
    public class HomeController : Controller
    {

        SentenceContext db = new SentenceContext();

        static string fileName = null;
        static string s = " ";

        public ActionResult Index()
        {
            IEnumerable<Sentence> sentences = db.Sentences;
            ViewBag.Sentences = sentences;

            int strLen = s.Length;
            if (strLen > 500)
            {
                string result = s.Substring(0, 500);
                result = result + " ...";
                ViewBag.Message = result;
            }
            else
            {
                ViewBag.Message = s;
            }

            return View();
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string word)
        {

            if (upload != null)
            {

                fileName = Path.GetFileName(upload.FileName);
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));

                string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Files");
                string absolutePathToFile = Path.Combine(appDataPath, fileName);
                StreamReader sr = new StreamReader(absolutePathToFile);
                
                
                string s_test = System.IO.File.ReadAllText(absolutePathToFile).Replace("\n", " ");

 
                if (s_test.Any(wordByte => wordByte > 127))
                {
                    s = System.IO.File.ReadAllText(absolutePathToFile,Encoding.GetEncoding(1251)).Replace("\n", " ");
                }
                else
                {
                    s = System.IO.File.ReadAllText(absolutePathToFile).Replace("\n", " ");
                }

                string[] bufs = s.Split('.');


                foreach (string s in bufs)
                {
                    Sentence copy = null;

                    bool res = Regex.IsMatch(s, "\\b" + word + "\\b");
                    int amount = new Regex(word).Matches(s).Count;

                    if (res is true)
                    {
                        string Strif = new string(s.ToCharArray().Reverse().ToArray());
                        copy = db.Sentences.FirstOrDefault(u => u.SentenceData == Strif);
                        if (copy == null)
                        {
                            string output = new string(s.ToCharArray().Reverse().ToArray());
 
                            db.Sentences.Add(new Sentence { SentenceData = output, Amount = amount });
                        }
                        else
                        {
                            string output = new string(s.ToCharArray().Reverse().ToArray());
                            output += "(copy)";
                            db.Sentences.Add(new Sentence { SentenceData = output, Amount = amount });
                        }

                        res = false;
                    }

                }

                sr.Close();
                db.SaveChanges();

            }

            return RedirectToAction("index");
        }

    }
}