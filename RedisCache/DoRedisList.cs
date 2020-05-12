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


    }
}
