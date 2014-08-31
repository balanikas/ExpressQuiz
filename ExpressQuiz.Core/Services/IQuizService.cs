using System.Linq;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IQuizService : IService<Quiz>
    {
       
        IQueryable<Quiz> GetPublicQuizzes();

        IQueryable<Quiz> GetBySearchTerm(string searchTerm, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetByCategory(int categoryId, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetBy(QuizFilter filter, IQueryable<Quiz> quizzes = null, bool? descending = null, int? count = null);

        IQueryable<Quiz> GetByRating(bool descending, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetByLevel(bool descending, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetByCreationDate(bool descending, IQueryable<Quiz> quizzes = null);

        bool Exists(string name);
    }
}