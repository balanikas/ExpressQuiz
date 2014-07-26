using System;
using System.Collections.Generic;
using System.Linq;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    class AnswerRepo : IAnswerRepo
    {
        private readonly QuizDbContext _ctx;
        private bool _disposed = false;

        public AnswerRepo(QuizDbContext ctx)
        {
            _ctx = ctx;
        }
      

        public IEnumerable<Answer> GetAll()
        {
            return _ctx.Answers.ToList();
        }

        public Answer GetById(int id)
        {
            return _ctx.Answers.Find(id);
        }

        public void Save()
        {
            _ctx.SaveChanges();
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