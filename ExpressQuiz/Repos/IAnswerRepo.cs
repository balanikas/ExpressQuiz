using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    public interface IAnswerRepo : IDisposable
    {
        IEnumerable<Answer> GetAll();
        Answer GetById(int id);
       
        void Save();
    }
}
