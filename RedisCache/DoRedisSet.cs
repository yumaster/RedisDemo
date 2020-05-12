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
        #region 其他
        /// <summary>
        /// 从fromKey集合中移除值为value的值，并把value添加到toKey集合中
        /// </summary>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        /// <param name="value"></param>
        public void MoveBetweenSets(string fromKey,string toKey,string value)
        {
            Core.MoveBetweenSets(fromKey, toKey, value);
        }
        /// <summary>
        /// 返回keys多个集合中的并集，返还hashset
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public HashSet<string>GetUnionFromSets(string[] keys)
        {
            return Core.GetUnionFromSets(keys);
        }
        /// <summary>
        /// keys多个集合中的并集,放入newKey集合中
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="keys"></param>
        public void StoreUnionFromSets(string newKey,string[] keys)
        {
            Core.StoreUnionFromSets(newKey, keys);
        }
        /// <summary>
        /// 把fromKey集合中的数据与keys集合中的数据对比，fromKey集合中不存在keys集合中，则把这些不存在的数据放入newkey集合中
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="fromKey"></param>
        /// <param name="keys"></param>
        public void StoreDifferencesFromSet(string newKey,string fromKey,string[] keys)
        {
            Core.StoreDifferencesFromSet(newKey, fromKey, keys);
        }
        #endregion
    }
}
