using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;

namespace ExpressQuiz.Repos
{
    public class QuizRepo : IQuizRepo
    {
        private readonly QuizDbContext _ctx;
        private bool _disposed = false;
        public QuizRepo(QuizDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<Quiz> GetAll()
        {
            return _ctx.Quizzes.ToList();
        }

        public Quiz GetById(int id)
        {
            return _ctx.Quizzes.Find(id);
        }

        public void Insert(Quiz quiz)
        {
            _ctx.Quizzes.Add(quiz);
        }

        public void Delete(Quiz quiz)
        {
            _ctx.Quizzes.Remove(quiz);
        }

        public void SetModified(Quiz quiz)
        {
            _ctx.Entry(quiz).State = EntityState.Modified;
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