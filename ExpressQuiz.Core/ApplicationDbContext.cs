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
          
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}