using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    public class DoRedisString:DoRedisBase
    {
        #region 赋值
        /// <summary>
        /// 设置key的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(string key,string value)
        {
            return RedisBase.Core.Set<string>(key, value);
        }
        #endregion
    }
}
