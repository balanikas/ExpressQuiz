using System;
using System.Collections.Generic;
using System.Linq;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    class QuizResultRepo : IQuizResultRepo
    {
        private readonly QuizDbContext _ctx;
        private bool _disposed = false;

        public QuizResultRepo(QuizDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<QuizResult> GetAll()
        {
            return _ctx.QuizResults.ToList();
        }

        public QuizResult GetById(int id)
        {
            return _ctx.QuizResults.Find(id);
        }

        public void Insert(QuizResult result)
        {
            _ctx.QuizResults.Add(result);
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