using System;
using System.Collections.Generic;
using System.Linq;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    class QuizCategoryRepo : IQuizCategoryRepo
    {
        private readonly QuizDbContext _ctx;
        private bool _disposed = false;

        public QuizCategoryRepo(QuizDbContext ctx)
        {
            _ctx = ctx;
        }
      

        public IEnumerable<QuizCategory> GetAll()
        {
            return _ctx.QuizCategories.ToList();
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