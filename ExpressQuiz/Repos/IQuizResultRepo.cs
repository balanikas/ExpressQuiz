using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    interface IQuizResultRepo : IDisposable
    {
        IEnumerable<QuizResult> GetAll();
        QuizResult GetById(int id);

        void Insert(QuizResult result);
        void Save();
    }
}