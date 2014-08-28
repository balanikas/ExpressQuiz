using System.Data.Entity.Migrations;
using System.Linq;
using ExpressQuiz.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpressQuiz.Core.Migrations.Application
{
    internal sealed class ApplicationDbContextConfig : DbMigrationsConfiguration<ExpressQuiz.Core.Models.ApplicationDbContext>
    {
        public ApplicationDbContextConfig()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ExpressQuiz.Core.Models.ApplicationDbContext context)
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
    }
}
