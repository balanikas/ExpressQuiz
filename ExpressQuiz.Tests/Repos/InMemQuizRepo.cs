﻿using System;
using System.Collections.Generic;
using System.Linq;
using ExpressQuiz.Controllers;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Tests.Repos
{
    class InMemQuizRepo : IRepo<Quiz>
    {

        private readonly List<Quiz> _items;

        public InMemQuizRepo(List<Quiz> items)
        {
            _items = items;
        }

        public Quiz Get(int id)
        {
            return _items.First(x => x.Id == id);
        }

        public IQueryable<Quiz> GetAll()
        {
            return _items.AsQueryable();
        }

        public Quiz Insert(Quiz o)
        {
            _items.Add(o);
            o.Id = new Random().Next(1,10000);
            return o;
        }

        public void Update(Quiz o)
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
