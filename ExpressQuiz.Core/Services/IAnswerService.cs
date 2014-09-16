using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Services
{
    public interface IAnswerService : IService<Answer>
    {
        void SaveOrder(Question question, string order);
    }
}