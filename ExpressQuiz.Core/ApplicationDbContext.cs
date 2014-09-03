using ExpressQuiz.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpressQuiz.Core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
            Database.CreateIfNotExists();
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, ApplicationDbContextConfig>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}