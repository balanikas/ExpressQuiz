using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IQuizCategoryService : IService<QuizCategory>
    {
        QuizCategory InsertByName(string name);

        bool Exists(string name);
    }
}