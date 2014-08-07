using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Migrations;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Export()
        {
            var quizRepo = new Repo<Quiz>(new QuizDbContext());
            DataProvider.Export(quizRepo, @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\out.xml");
            return View("Index");
        }

        public ActionResult Import()
        {

            var ctx = new QuizDbContext();


            var quizzes = DataProvider.Import(ctx, @"C:\Users\grillo\Documents\GitHub\ExpressQuiz\ExpressQuiz\App_Data\seeddata.xml");

            ctx.Quizzes.AddOrUpdate(i => i.Name,
                       quizzes.ToArray()
                  );
            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
            
            return View("Index");
        }
    }
}