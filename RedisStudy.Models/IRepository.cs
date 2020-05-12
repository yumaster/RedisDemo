using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStudy.DAL.Abstraction
{
    public interface IRepository<T>
    {
        IDbContext Context { get; }

        void Add(T entity);
        void Remove(T entity);
        T GetById(string id);

        void Update(T entity);
    }
}
