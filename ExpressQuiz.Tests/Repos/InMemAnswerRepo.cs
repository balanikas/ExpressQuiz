using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Tests.Repos
{
    class InMemAnswerRepo : IRepo<Answer>
    {

        private readonly List<Answer> _items;

        public InMemAnswerRepo(List<Answer> items)
        {
            _items = items;
        }

        public Answer Get(int id)
        {
            return _items.First(x => x.Id == id);
        }

        public IQueryable<Answer> GetAll()
        {
            return _items.AsQueryable();
        }

        public Answer Insert(Answer o)
        {
            _items.Add(o);
            o.Id = new Random().Next(1, 10000);
            return o;
        }

        public void Update(Answer o)
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
