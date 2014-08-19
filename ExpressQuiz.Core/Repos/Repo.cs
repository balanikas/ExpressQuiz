using System;
using System.Data.Entity;
using System.Linq;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.Core.Repos
{
    public class Repo<T>: IRepo<T> where T: Entity,new()
    {
        private readonly QuizDbContext _ctx;
        private bool _disposed = false;

        public Repo(QuizDbContext ctx)
        {
            _ctx = ctx;
        }
        public T Get(int id)
        {
            return _ctx.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _ctx.Set<T>();
        }

        public T Insert(T o)
        {
            return _ctx.Set<T>().Add(o);
        }

        public void Update(T o)
        {
            _ctx.Entry(o).State = EntityState.Modified;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _ctx.Set<T>().Find(id);
            if (existing != null)
            {
                _ctx.Set<T>().Remove(existing);
            }
            
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