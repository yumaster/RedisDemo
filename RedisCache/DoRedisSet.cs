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
        #region 获取
        /// <summary>
        /// 随机获取key集合中的一个值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetRandomItemFromSet(string key)
        {
            return Core.GetRandomItemFromSet(key);
        }
        /// <summary>
        /// 获取key集合的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetCount(string key)
        {
            return Core.GetSetCount(key);
        }
        /// <summary>
        /// 获取所有key集合的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HashSet<string>GetAllItemsFromSet(string key)
        {
            return Core.GetAllItemsFromSet(key);
        }
        #endregion
        #region 删除
        /// <summary>
        /// 随机删除key集合中的一个值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string PopItemFromSet(string key)
        {
            return Core.PopItemFromSet(key);
        }
        public void RemoveItemFromSet(string key,string value)
        {
            Core.RemoveItemFromSet(key, value);
        }
        #endregion

    }
}
