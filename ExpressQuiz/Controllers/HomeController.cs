using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.ViewModels;

namespace ExpressQuiz.Controllers
{
   
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly IRepo<ContactInfo> _contactInfoRepo;

        public HomeController(IRepo<ContactInfo> contactInfoRepo )
        {
            _contactInfoRepo = contactInfoRepo;
        }

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

            var vm = new ContactViewModel();
            vm.ContactInfo = new ContactInfo();
            return View("Contact", vm);
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.ContactInfo.Created = DateTime.Now;
                _contactInfoRepo.Insert(vm.ContactInfo);
                _contactInfoRepo.Save();

                vm = new ContactViewModel();
                vm.ContactInfo = new ContactInfo();
                vm.WasSent = true;
                ModelState.Clear();
            }
           
            return View("Contact", vm);
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