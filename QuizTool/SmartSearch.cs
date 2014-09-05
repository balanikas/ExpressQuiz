using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuizTool
{
    class SmartSearch
    {
       
      
        public List<Uri> Find(Uri uri)
        {
            var allLinks = new List<Uri>();
           

            var doc = DownloadAsXml(uri);

            var links = FindLinks(doc, uri);


            Parallel.ForEach(links, link =>
            {
                doc = DownloadAsXml(link);
                if (doc == null)
                {
                    return;
                }

                foreach (var link1 in FindLinks(doc, link))
                {
                    if (DownloadAsXml(link1) == null)
                    {
                        continue;
                    }
                    allLinks.Add(link1);
                }
            });
        

            return allLinks;
        }

        private static IEnumerable<Uri> FindLinks(XElement doc, Uri uri)
        {
            var absPath = uri.AbsolutePath;
            var host = uri.Host;

            var links =
               doc.Descendants()
                   .Where(x => x.Attribute("href") != null)
                   .Where(x => ((string)x.Attribute("href")).StartsWith(absPath))
                   .Select(x => x.Attribute("href").Value)
                   .Distinct()
                   .Where(x => x != absPath)
                   .Select(x => new Uri("http://" + host + x));

            return links;
        }

        private XElement DownloadAsXml(Uri uri)
        {
            string data;


            var filePath = Path.Combine(Environment.CurrentDirectory, "documents",
                uri.AbsolutePath.Replace("/", "") + ".html");
            if (!File.Exists(filePath))
            {
                 using (var client = new WebClient())
                {
                    client.DownloadFile(uri.AbsoluteUri, filePath);
                    data = client.DownloadString(uri);

                }
            }
            else
            {
                data = Helpers.ReadDocument(filePath);
            }

           

            data = WebUtility.HtmlDecode(data);


            XElement doc;
            try
            {
                doc = Helpers.HtmlToXElement(data);
            }
            catch (Exception)
            {

                return null;
            }

            return doc;
        }

      
    }
}
