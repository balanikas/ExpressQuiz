using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Utils;

namespace ExpressQuiz.Core
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext()
            : base("QuizDbContext")
        {
            Database.CreateIfNotExists();
            //Database.Log = new Logger().Info;
        }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuizCategory> QuizCategories { get; set; }
        public DbSet<QuizRating> QuizRatings { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<UserActivity> UserActivity { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                new Logger().Error(sb.ToString());

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); 
            }
        }
    }
}