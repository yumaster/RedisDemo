using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    /// <summary>
    /// .Net操作Redis数据类型Set
    /// </summary>
    public class DoRedisSet:DoRedisBase
    {
        #region 添加
        /// <summary>
        /// key集合中添加value值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key,string value)
        {
            Core.AddItemToSet(key, value);
        }
        /// <summary>
        /// key集合添加list集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        public void Add(string key,List<string>list)
        {
            Core.AddRangeToSet(key, list);
        }
        #endregion
    }
}
