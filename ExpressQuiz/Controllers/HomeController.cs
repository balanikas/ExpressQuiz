using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressQuiz.Controllers
{
   
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult About()
        {
            

            return View("About");
        }

        public ActionResult Contact()
        {
            

            return View("Contact");
        }
        
        [Authorize]
        public ActionResult Admin()
        {


            return View("Index");
        }

        public ActionResult Error(string message)
        {
            HandleErrorInfo info = new HandleErrorInfo(new Exception("APPLICATION ERROR: " + message), "Home", "Error");
            return View("Error",info );
        }
    }
}