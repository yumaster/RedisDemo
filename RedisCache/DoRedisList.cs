using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    /// <summary>
    /// .Net操作Redis数据类型List
    /// </summary>
    public class DoRedisList:DoRedisBase
    {
        #region 赋值
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void LPush(string key,string value)
        {
            Core.PushItemToList(key, value);
        }
        /// <summary>
        /// 从左侧向list中添加值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        public void LPush(string key,string value,DateTime dt)
        {
            Core.PushItemToList(key, value);
            Core.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 从左侧向list中添加值，设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sp"></param>
        public void LPush(string key,string value,TimeSpan sp)
        {
            Core.PushItemToList(key, value);
            Core.ExpireEntryIn(key, sp);
        }
        /// <summary>
        /// 从右侧向list中添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void RPush(string key,string value)
        {
            Core.PrependItemToList(key, value);
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        public void RPush(string key,string value,DateTime dt)
        {
            Core.PrependItemToList(key, value);
            Core.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sp"></param>
        public void RPush(string key,string value,TimeSpan sp)
        {
            Core.PrependItemToList(key, value);
            Core.ExpireEntryIn(key, sp);
        }
        /// <summary>
        /// 添加key/value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key,string value)
        {
            Core.AddItemToList(key, value);
        }
        /// <summary>
        /// 添加key/value，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        public void Add(string key,string value,DateTime dt)
        {
            Core.AddItemToList(key, value);
            Core.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 添加key/value，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sp"></param>
        public void Add(string key, string value, TimeSpan sp)
        {
            Core.AddItemToList(key, value);
            Core.ExpireEntryIn(key, sp);
        }
        /// <summary>
        /// 为key添加多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void Add(string key,List<string>values)
        {
            Core.AddRangeToList(key, values);
        }
        /// <summary>
        /// 为key添加多个值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="dt"></param>
        public void Add(string key,List<string>values,DateTime dt)
        {
            Core.AddRangeToList(key, values);
            Core.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 为key添加多个值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="sp"></param>
        public void Add(string key,List<string>values,TimeSpan sp)
        {
            Core.AddRangeToList(key, values);
            Core.ExpireEntryIn(key, sp);
        }
        #endregion

        #region 获取值
        /// <summary>
        /// 获取list中key包含的数据数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Count(string key)
        {
            return Core.GetListCount(key);
        }
        /// <summary>
        /// 获取key包含的所有数据集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string>Get(string key)
        {
            return Core.GetAllItemsFromList(key);
        }
        /// <summary>
        /// 获取key中下标为star到end的值集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<string>Get(string key,int star,int end)
        {
            return Core.GetRangeFromList(key, star, end);
        }
        #endregion

        #region 阻塞命令
        /// <summary>
        /// 阻塞命令：从list中key的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public string BlockingPopItemFromList(string key,TimeSpan? sp)
        {
            return Core.BlockingPopItemFromList(key, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public ItemRef BlockingPopItemFromLists(string[]keys,TimeSpan?sp)
        {
            return Core.BlockingPopItemFromLists(keys, sp);
        }
        /// <summary>
        ///  阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingDequeueItemFromList(string key, TimeSpan? sp)
        {
            return Core.BlockingDequeueItemFromList(key, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public ItemRef BlockingDequeueItemFromLists(string[] keys, TimeSpan? sp)
        {
            return Core.BlockingDequeueItemFromLists(keys, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中key的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingRemoveStartFromList(string keys, TimeSpan? sp)
        {
            return RedisBase.Core.BlockingRemoveStartFromList(keys, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中key的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public ItemRef BlockingRemoveStartFromLists(string[] keys, TimeSpan? sp)
        {
            return RedisBase.Core.BlockingRemoveStartFromLists(keys, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中一个fromkey的尾部移除一个值，添加到另外一个tokey的头部，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingPopAndPushItemBetweenLists(string fromkey, string tokey, TimeSpan? sp)
        {
            return RedisBase.Core.BlockingPopAndPushItemBetweenLists(fromkey, tokey, sp);
        }
        #endregion


    }
}
