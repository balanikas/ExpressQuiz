using System;
using System.Collections.Generic;
using System.Linq;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Tests.Repos
{
    class InMemQuizRepo : IRepo<Quiz>
    {

        private readonly List<Quiz> _quizzes = new List<Quiz>(); 

        public Quiz Get(int id)
        {
            return _quizzes.First(x => x.Id == id);
        }

        public IQueryable<Quiz> GetAll()
        {
            return _quizzes.AsQueryable();
        }

        public Quiz Insert(Quiz o)
        {
            _quizzes.Add(o);
            o.Id = new Random().Next(1,10000);
            return o;
        }

        public void Update(Quiz o)
        {
            var existing = _quizzes.First(x => x.Id == o.Id);
            existing = o;
        }

        public void Save()
        {
            
        }

        public void Delete(int id)
        {
            _quizzes.Remove(_quizzes.First(x => x.Id == id));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
