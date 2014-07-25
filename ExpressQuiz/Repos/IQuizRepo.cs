using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    interface IQuizRepo: IDisposable
    {
        IEnumerable<Quiz> GetAll();
        Quiz GetById(int id);
        void Insert(Quiz quiz);
        void Delete(Quiz quiz);

        void SetModified(Quiz quiz);
        void Save();
    }
}
