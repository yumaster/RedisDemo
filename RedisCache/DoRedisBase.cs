using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    /// <summary>
    /// DoRedisBase
    /// </summary>
    public class DoRedisBase : RedisBase
    {
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="datetime">datetime</param>
        public void SetExpire(string key, DateTime datetime)
        {
            Core.ExpireEntryAt(key, datetime);
        }

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="tp">tp</param>
        public void SetExpire(string key, TimeSpan tp)
        {
            Core.ExpireEntryIn(key, tp);
        }

        /// <summary>
        /// 移除整个key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>result</returns>
        public bool Remove(string key)
        {
            return Core.Remove(key);
        }

        /// <summary>
        /// 清空数据库
        /// </summary>
        public void PushAll()
        {
            Core.FlushAll();
        }

        /// <summary>
        /// key重命名
        /// </summary>
        /// <param name="fromName">fromName</param>
        /// <param name="toName">toName</param>
        public void RenameKey(string fromName, string toName)
        {
            Core.RenameKey(fromName, toName);
        }
    }
}
