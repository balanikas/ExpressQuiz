using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;

namespace ExpressQuiz.Core.Services
{
    public interface IQuestionService : IService<Question>
    {
        void SaveOrder(List<Question> questions , string order);
    }
}
