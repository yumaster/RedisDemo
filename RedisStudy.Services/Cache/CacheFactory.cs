using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStudy.Services
{
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();
        public static ICache Cache
        {
            get
            {
                if (cache == null)
                {
                    lock (lockHelper)
                    {
                        if (cache == null)
                        {
                            cache = new RedisCacheImp();
                        }
                    }
                }
                return cache;
            }
        }
    }
}
