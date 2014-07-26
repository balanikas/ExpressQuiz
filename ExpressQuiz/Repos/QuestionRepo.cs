using System;
using System.Collections.Generic;
using System.Linq;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    class QuestionRepo : IQuestionRepo
    {
        private readonly QuizDbContext _ctx;
        private bool _disposed = false;

        public QuestionRepo(QuizDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Question> GetAll()
        {
            return _ctx.Questions.ToList();
        }

        public Question GetById(int id)
        {
            return _ctx.Questions.Find(id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            this._disposed = true;
        }
    }
}