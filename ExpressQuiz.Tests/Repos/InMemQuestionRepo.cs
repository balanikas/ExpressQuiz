using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Tests.Repos
{
    class InMemQuestionRepo : IRepo<Question>
    {
        private readonly List<Question> _items;

        public InMemQuestionRepo(List<Question> items)
        {
            _items = items;
        }

          public Question Get(int id)
        {
            return _items.First(x => x.Id == id);
        }

        public IQueryable<Question> GetAll()
        {
            return _items.AsQueryable();
        }

        public Question Insert(Question o)
        {
            _items.Add(o);
            o.Id = new Random().Next(1,10000);
            return o;
        }

        public void Update(Question o)
        {
            var existing = _items.First(x => x.Id == o.Id);
            existing = o;
        }

        public void Save()
        {
            
        }

        public void Delete(int id)
        {
            _items.Remove(_items.First(x => x.Id == id));
        }

        public void Dispose()
        {
           
        }
    }
}
