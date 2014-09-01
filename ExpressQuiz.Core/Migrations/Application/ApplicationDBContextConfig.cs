using System.Data.Entity.Migrations;
using System.Linq;
using ExpressQuiz.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpressQuiz.Core.Migrations.Application
{
    internal sealed class ApplicationDbContextConfig : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public ApplicationDbContextConfig()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
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

            AddAdditionalUsers(context);

        }

        private void AddAdditionalUsers(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            ApplicationUser user;

            user = new ApplicationUser { UserName = "user1", Email = "user1@domain.com" };
            manager.Create(user, "_User1Pwd");
            user = new ApplicationUser { UserName = "user2", Email = "user2@domain.com" };
            manager.Create(user, "_User2Pwd");
            
        }
    }
}
