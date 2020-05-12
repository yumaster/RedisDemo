using RedisStudy.DAL.Abstraction;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace RedisStudy.DAL.EF
{
    public class EFRepository<T> : IEFRepository<T> where T : class
    {
        public EFRepository(IDbContext context)
        {
            Context = context;
        }

        public IDbContext Context { get; }

        public IQueryable<T> Table => GetDbSet();

        private DbSet<T> GetDbSet()
        {
            if (this.Context is DbContext context)
            {
                //DbContext context = Context as DbContext;
                return context.Set<T>();
            }
            throw new InvalidCastException();
        }

        private DbContext GetContext()
        {
            if (this.Context is DbContext context)
            {
                //DbContext context = Context as DbContext;
                return context;
            }
            throw new InvalidCastException();
        }



        public void Add(T entity) => GetDbSet().Add(entity);

        public T GetById(string id) => GetDbSet().Find(id);

        public void Remove(T entity) => GetDbSet().Remove(entity);

        public void Update(T entity)
        {
            GetDbSet().Attach(entity);
            GetContext().Entry(entity).State= EntityState.Modified;
        }
    }
}
