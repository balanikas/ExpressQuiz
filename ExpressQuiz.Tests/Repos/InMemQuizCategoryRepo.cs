using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Tests.Repos
{
    class InMemQuizCategoryRepo : IRepo<QuizCategory>
    {


        private readonly List<QuizCategory> _items;

        public InMemQuizCategoryRepo(List<QuizCategory> items)
        {
            _items = items;
        }

        public void Dispose()
        {
          
        }

        public QuizCategory Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<QuizCategory> GetAll()
        {
            throw new NotImplementedException();
        }

        public QuizCategory Insert(QuizCategory o)
        {
            throw new NotImplementedException();
        }

        public void Update(QuizCategory o)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
