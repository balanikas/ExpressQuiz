using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ExpressQuiz.Models
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext()
            : base("QuizDbContext")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            // Database.SetInitializer<QuizDbContext>(new DropCreateDatabaseAlways<QuizDbContext>());
        }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuizCategory> QuizCategories { get; set; }
        public DbSet<QuizRating> QuizRatings { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
       
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}