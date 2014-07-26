using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    public interface IQuestionRepo : IDisposable
    {
        IEnumerable<Question> GetAll();
        Question GetById(int id);
    }
}
