using RssManager.Interfaces.BO;
using System.Collections.Generic;

namespace RssManager.Interfaces.Repository
{
    public interface IRepository<T> where T: IEntity
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        int Delete(long id);
        void Update(T entity);
        void Save(T entity);
        T Get(long id);
    }
}
