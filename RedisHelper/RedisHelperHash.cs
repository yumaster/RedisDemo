using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCommon
{
    /// <summary>
    /// Hash 哈希类型
    /// </summary>
    public partial class RedisHelper
    {
        #region 同步方法
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key">哈希key  hashid</param>
        /// <param name="dataKey">字段  field</param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return Do(x => x.HashExists(key, dataKey));
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">hash id</param>
        /// <param name="dataKey">field</param>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public bool HashSet<T>(string key,string dataKey,T t)
        {
            key = AddSysCustomKey(key);
            return Do(x =>
            {
                string json = ConvertJson(t);
                return x.HashSet(key, dataKey, json);
            });
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key">hash id</param>
        /// <param name="dataKey">field</param>
        /// <returns></returns>
        public bool HashDelete(string key,string dataKey)
        {
            key = AddSysCustomKey(key);
            return Do(x => x.HashDelete(key, dataKey));
        }
        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key">hash id</param>
        /// <param name="dataKeys">fields</param>
        /// <returns></returns>
        public long HashDelete(string key,List<string>dataKeys)
        {
            key = AddSysCustomKey(key);
            return Do(x => x.HashDelete(key, ConvertRedisValues(dataKeys)));
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">hash id</param>
        /// <param name="dataKey">field</param>
        /// <returns></returns>
        public T HashGet<T>(string key,string dataKey)
        {
            key = AddSysCustomKey(key);
            return Do(x =>
            {
                string value = x.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            });
        }
        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">hash id</param>
        /// <param name="dataKey">field</param>
        /// <param name="val">可以为负</param>
        /// <returns>返回增长后的值</returns>
        public double HashIncrement(string key,string dataKey,double val=1)
        {
            key = AddSysCustomKey(key);
            return Do(x => x.HashIncrement(key, dataKey, val));
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">hash id</param>
        /// <param name="dataKey">field</param>
        /// <param name="val">可以为负</param>
        /// <returns>返回减少后的值</returns>
        public double HashDecrement(string key,string dataKey,double val=1)
        {
            key = AddSysCustomKey(key);
            return Do(x => x.HashDecrement(key, dataKey, val));
        }
        /// <summary>
        /// 获取hashkey 所有redis value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T>HashKeys<T>(string key)
        {
            key = AddSysCustomKey(key);
            return Do(x =>
            {
                //RedisValue[] values = x.HashKeys(key);
                RedisValue[] values = x.HashValues(key);
                return ConvertList<T>(values);
            });
        }
        #endregion

        #region 异步方法
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool>HashExistsAsync(string key,string dataKey)
        {
            key = AddSysCustomKey(key);
            return await Do(x => x.HashExistsAsync(key, dataKey));
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool>HashSetAsync<T>(string key,string dataKey,T t)
        {
            key = AddSysCustomKey(key);
            return await Do(x =>
            {
                string json = ConvertJson(t);
                return x.HashSetAsync(key, dataKey, json);
            });
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys)
        {
            key = AddSysCustomKey(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray()));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGetAsync<T>(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashKeysAsync<T>(string key)
        {
            key = AddSysCustomKey(key);
            //RedisValue[] values = await Do(db => db.HashKeysAsync(key));
            RedisValue[] values = await Do(db => db.HashValuesAsync(key));
            return ConvertList<T>(values);
        }
        #endregion
    }
}
