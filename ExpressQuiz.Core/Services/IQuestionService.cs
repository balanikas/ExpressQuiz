using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IQuestionService : IService<Question>
    {
        void SaveOrder(Quiz quiz, string order);
    }
}