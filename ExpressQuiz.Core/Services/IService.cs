using System.Linq;

namespace ExpressQuiz.Core.Services
{
    public interface IService<T>
    {
        T Get(int id);
        IQueryable<T> GetAll();
        T Insert(T o);
        void Update(T o);
        void Delete(int id);
    }
}