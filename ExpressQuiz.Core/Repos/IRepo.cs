using System;
using System.Linq;

namespace ExpressQuiz.Core.Repos
{
    public interface IRepo<T> : IDisposable
    {
        T Get(int id);
        IQueryable<T> GetAll();
        T Insert(T o);
        void Update(T o);
        void Save();
        void Delete(int id);
    }
}