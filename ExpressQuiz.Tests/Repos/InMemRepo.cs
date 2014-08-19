using System;
using System.Collections.Generic;
using System.Linq;

using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;


namespace ExpressQuiz.Tests.Repos
{
    class InMemRepo<T> : IRepo<T> where T : Entity
    {
        private readonly List<T> _items;

        public InMemRepo(List<T> items)
        {
            _items = items;
        }

        public void Dispose()
        {
           
        }

        public T Get(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return _items.AsQueryable();
        }

        public T Insert(T o)
        {
            _items.Add(o);
            o.Id = new Random().Next(1, 10000);
            return o;
        }

        public void Update(T o)
        {
            var existing = _items.FirstOrDefault(x => x.Id == o.Id);
            if (existing != null)
            {
                existing = o;
            }
           
        }

        public void Save()
        {
           
        }

        public void Delete(int id)
        {
            var existing = _items.FirstOrDefault(x => x.Id == id);

            if (existing != null)
            {
                _items.Remove(existing);
            }
           
        }
    }
}
