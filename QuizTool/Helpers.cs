using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Xml.Linq;
using ExpressQuiz.Core.Models;
using HtmlAgilityPack;
using System.Diagnostics;

namespace QuizTool
{
    public class Helpers
    {
        public static XElement HtmlToXElement(string html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            HtmlDocument doc = new HtmlDocument();
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(html);
            using (StringWriter writer = new StringWriter())
            {
                doc.Save(writer);
                using (StringReader reader = new StringReader(writer.ToString()))
                {
                    return XElement.Load(reader);
                }
            }
        }

        public static void SaveLinks(string filePath, List<Uri> links )
        {
            var file = new StreamWriter(filePath);
            foreach (var link in links)
            {
                file.WriteLine(link.AbsoluteUri);
            }

            file.Close();
        }

        public static void SaveDocuments(string path, List<Uri> uris)
        {
            foreach (var uri in uris)
            {
                SaveDocument(path,uri);
            }          
        }

        public static void SaveDocument(string path, Uri uri)
        {
      
            using (var client = new WebClient())
            {
                client.DownloadFile(uri.AbsoluteUri, Path.Combine(path, uri.AbsolutePath + ".html"));

            }
            
        }

        public static string ReadDocument(string path)
        {
            return File.ReadAllText(path);
        }

        public static bool ValidateObject(object quiz)
        {

            var context = new ValidationContext(quiz, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quiz, context, results);

            

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    Trace.WriteLine(validationResult.ErrorMessage);
                }

                
            }

            return isValid;
        }

        public static bool ValidateQuiz(Quiz quiz)
        {
            if (ValidateObject(quiz))
            {
                foreach (var question in quiz.Questions)
                {
                  
                    if ( ValidateObject(question))
                    {
                        foreach (var answer in question.Answers)
                        {
                            

                            if (!ValidateObject(answer))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }
    }
}
