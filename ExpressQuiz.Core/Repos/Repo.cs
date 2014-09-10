using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Utils;

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
            var result = _ctx.Set<T>().Add(o);
            if (result != null)
            {
                new Logger().Info("Creating entity of type " + typeof(T).Name);
            }
          
            return result;
        }

        public void Update(T o)
        {
            _ctx.Entry(o).State = EntityState.Modified;
            new Logger().Info("Updating entity of type " + typeof(T).Name + " with id " + o.Id);
        }

        public void Save()
        {
            List<Object> modifiedOrAddedEntities = _ctx.ChangeTracker.Entries()
                 .Where(x => x.State == EntityState.Modified
                        || x.State == EntityState.Added
                        || x.State == EntityState.Deleted)
                 .Select(x => x.Entity).ToList();

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _ctx.Set<T>().Find(id);
            if (existing != null)
            {
                _ctx.Set<T>().Remove(existing);
                new Logger().Info("Deleting entity of type " + typeof(T).Name + " with id " + id);
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