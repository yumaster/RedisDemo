using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStudy.Services
{
    public abstract class BaseBusinessCache<T>
    {
        public abstract string CacheKey { get; }
        public virtual bool Remove()
        {
            return CacheFactory.Cache.RemoveCache(CacheKey);
        }
        public virtual List<T> GetList()
        {
            throw new Exception("请在子类实现");
        }
    }
}
