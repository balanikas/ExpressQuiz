using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpressQuiz.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ExpressQuiz.Core.Models.QuizDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(QuizDbContext context)
        {
            context.UserAnswers.RemoveRange(context.UserAnswers.AsEnumerable());
            context.QuizResults.RemoveRange(context.QuizResults.AsEnumerable());
            context.QuizCategories.RemoveRange(context.QuizCategories.AsEnumerable());

            context.SaveChanges();



            SeedUsersAndRoles(new ApplicationDbContext());
           

            var uri = DataProvider.MapPath("~/bin/App_Data/seeddata.xml");
            var extraUri = DataProvider.MapPath("~/App_Data/imports/out.xml");
            var quizzes = DataProvider.Import(uri).ToList();
            quizzes.AddRange(DataProvider.Import(extraUri).ToList());

            context.QuizCategories.AddOrUpdate(i => i.Name,
               quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
               );



            context.SaveChanges();

            foreach (var quiz in quizzes)
            {
                quiz.Category = context.QuizCategories.First(x => x.Name == quiz.Category.Name);
            }

            context.Quizzes.AddOrUpdate(i => i.Name,
                      quizzes.ToArray()
                 );

            context.SaveChanges();
        }

        private void SeedUsersAndRoles(ApplicationDbContext context)
        {
            
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "info@coderentals.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "info@coderentals.com", Email = "info@coderentals.com" };
               
                var result = manager.Create(user, "Admin123");
               
                manager.AddToRole(user.Id, "Admin");
            }
        }

        private void RunAdditionalSeeds(QuizDbContext context)
        {
            var uri = DataProvider.MapPath("~/App_Data/imports/out.xml");

            var quizzes = DataProvider.Import(uri);

            context.QuizCategories.AddOrUpdate(i => i.Name,
               quizzes.Select(x => x.Category).DistinctBy(x => x.Name).ToArray()
               );

            context.SaveChanges();

            foreach (var quiz in quizzes)
            {
                quiz.Category = context.QuizCategories.First(x => x.Name == quiz.Category.Name);
            }


            context.Quizzes.AddOrUpdate(i => i.Name,
                        quizzes.ToArray()
                   );
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var error in e.EntityValidationErrors)
                {
                    foreach (var error2 in error.ValidationErrors)
                    {
                        Debug.WriteLine(error2.ErrorMessage);
                    }
                }
                throw;
            }

        }

        
    }
}
