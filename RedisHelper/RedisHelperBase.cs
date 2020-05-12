using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCommon
{
    public class RedisHelperBase
    {
        public ConnectionMultiplexer Multiplexer { get; }
        public string CustomRedisKey { get; }
        public int DBBase { get; }
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DBBase">数据库</param>
        /// <param name="redisCon">Redis连接字符串</param>
        /// <param name="redisKey">自定义KEY</param>
        public RedisHelperBase(int DBBase = -1, string redisCon = null, string redisKey = null)
        {
            Multiplexer = ConnectionMultiplexerHelp.GetMultiplexer(redisCon);
            CustomRedisKey = redisKey;
            this.DBBase = DBBase;
        }
        #endregion

        #region Key相应处理
        /// <summary>
        /// 删除单个KEY
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyDelete(key));
        }
        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">redis key</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(List<string>keys)
        {
            List<string> newKeys = keys.Select(AddSysCustomKey).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExists(key));
        }
        /// <summary>
        /// 重命名key
        /// </summary>
        /// <param name="key">旧的key</param>
        /// <param name="newKey">新的key</param>
        /// <returns></returns>
        public bool KeyRename(string key,string newKey)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyRename(key,newKey));
        }
        /// <summary>
        /// 设置key的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public bool KeyExpire(string key,TimeSpan?expire=default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExpire(key, expire));
        }

        #endregion

        #region 其他
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return Multiplexer.GetDatabase(DBBase);
        }

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }
        /// <summary>
        /// 根据IP获取对应的Redis服务
        /// </summary>
        /// <param name="hostAndPort"></param>
        /// <returns></returns>
        public IServer GetServer(string hostAndPort)
        {
            return Multiplexer.GetServer(hostAndPort);
        }
        /// <summary>
        /// 删除指定的库
        /// </summary>
        /// <param name="hostAndPort">IP:端口</param>
        /// <param name="database">数据库</param>
        public void FlushDataBase(string hostAndPort,int database=0)
        {
            GetServer(hostAndPort).FlushDatabase(database);
        }
        /// <summary>
        /// 删除该服务下所有数据库
        /// </summary>
        /// <param name="hostAndPort"></param>
        public void FlushAllDataBase(string hostAndPort)
        {
            GetServer(hostAndPort).FlushAllDatabases();
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 增加redis-key的前缀
        /// </summary>
        /// <param name="oldKey"></param>
        /// <returns></returns>
        public string AddSysCustomKey(string oldKey)
        {
            var prefixKey = CustomRedisKey ?? ConfigInfo.RedisKeyStr;
            return prefixKey + oldKey;
        }
        /// <summary>
        /// 方法执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Do<T>(Func<IDatabase, T> func)
        {
            var database = Multiplexer.GetDatabase(DBBase);
            return func(database);
        }
        /// <summary>
        /// 序列化处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }
        /// <summary>
        /// 反序列化处理（单条）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T ConvertObj<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 反序列化处理（集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public List<T> ConvertList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach(var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }
        /// <summary>
        /// RedisKey的类型转换
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public RedisKey[] ConvertRedisKeys(List<string>redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }
        /// <summary>
        /// RedisValue的类型转换
        /// </summary>
        /// <param name="redisValues"></param>
        /// <returns></returns>
        public RedisValue[] ConvertRedisValues(List<string>redisValues)
        {
            return redisValues.Select(redisValue => (RedisValue)redisValue).ToArray();
        }
        #endregion
    }
}
