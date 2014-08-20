using System.Linq;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IQuizService : IService<Quiz>
    {
        int GetAverageLevel(Quiz quiz);

        int GetAverageRating(Quiz quiz);

        int GetAverageScore(Quiz quiz);

        int GetAverageTime(Quiz quiz);

        int GetAverageTimePercent(Quiz quiz);

        IQueryable<Quiz> GetPublicQuizzes();

        IQueryable<Quiz> GetBySearchTerm(string searchTerm, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetByCategory(int categoryId, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetBy(QuizFilter filter, IQueryable<Quiz> quizzes = null, bool? descending = null, int? count = null);

        IQueryable<Quiz> GetByRating(bool descending, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetByLevel(bool descending, IQueryable<Quiz> quizzes = null);

        IQueryable<Quiz> GetByCreationDate(bool descending, IQueryable<Quiz> quizzes = null);

        bool QuizExists(string name);
    }
}