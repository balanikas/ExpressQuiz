using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Utils;
using HtmlAgilityPack;
using System.Diagnostics;

namespace QuizTool
{
    class Program
    {
        static void Main(string[] args)
        {

            var docPath = Path.Combine(Environment.CurrentDirectory, "documents");


            if (!Directory.Exists(docPath))
            {
                Directory.CreateDirectory(docPath);
            }


            if (true)
            {
                var search = new SmartSearch();
                var links = search.Find(new Uri("http://www.educationquizzes.com/specialist/"));
            }
            

          
            
            //save found links to file
           // Helpers.SaveLinks(Path.Combine(Environment.CurrentDirectory, "links.txt"),links);


            //download and save docs 
           // Helpers.SaveDocuments(Path.Combine(Environment.CurrentDirectory, "documents"), links);


            //start importing

            var outPath = Path.Combine(Environment.CurrentDirectory, "out.xml");


            var importer = new Importer();
            var quizzes = importer.Import(docPath, outPath);
        
            


            DataProvider.Export(quizzes, outPath);

        
        }



        
    }
}
