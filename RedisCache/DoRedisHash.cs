using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    /// <summary>
    /// .Net操作Redis数据类型哈希Hash
    /// </summary>
    public class DoRedisHash:DoRedisBase
    {
        #region 添加
        /// <summary>
        /// 向hashid集合中添加key/value
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetEntryInHash(string hashid,string key,string value)
        {
            return Core.SetEntryInHash(hashid, key, value);
        }
        /// <summary>
        /// 如果hashid集合中存在key/value则不添加返回false，如果不存在则添加key/value,返回true
        /// </summary>
        public bool SetEntryInHashIfNotExists(string hashid, string key, string value)
        {
            return Core.SetEntryInHashIfNotExists(hashid, key, value);
        }
        /// <summary>
        /// 存储对象T t到hash集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void StoreAsHash<T>(T t)
        {
            Core.StoreAsHash<T>(t);
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取对象T中ID为id的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetFromHash<T>(object id)
        {
            return Core.GetFromHash<T>(id);
        }
        /// <summary>
        /// 获取所有hashid数据集的key/value数据集合
        /// </summary>
        public Dictionary<string, string> GetAllEntriesFromHash(string hashid)
        {
            return RedisBase.Core.GetAllEntriesFromHash(hashid);
        }
        /// <summary>
        /// 获取hashid数据集中的数据总数
        /// </summary>
        public long GetHashCount(string hashid)
        {
            return RedisBase.Core.GetHashCount(hashid);
        }
        /// <summary>
        /// 获取hashid数据集中所有key的集合
        /// </summary>
        public List<string> GetHashKeys(string hashid)
        {
            return RedisBase.Core.GetHashKeys(hashid);
        }
        /// <summary>
        /// 获取hashid数据集中的所有value集合
        /// </summary>
        public List<string> GetHashValues(string hashid)
        {
            return RedisBase.Core.GetHashValues(hashid);
        }
        /// <summary>
        /// 获取hashid数据集中，key的value数据
        /// </summary>
        public string GetValueFromHash(string hashid, string key)
        {
            return RedisBase.Core.GetValueFromHash(hashid, key);
        }
        /// <summary>
        /// 获取hashid数据集中，多个keys的value集合
        /// </summary>
        public List<string> GetValuesFromHash(string hashid, string[] keys)
        {
            return RedisBase.Core.GetValuesFromHash(hashid, keys);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除hashid数据集合中的key数据
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveEntryFromHash(string hashid,string key)
        {
            return Core.RemoveEntryFromHash(hashid, key);
        }
        #endregion

        #region 其他
        /// <summary>
        /// 判断hashid数据集中是否存在key的数据
        /// </summary>
        public bool HashContainsEntry(string hashid, string key)
        {
            return RedisBase.Core.HashContainsEntry(hashid, key);
        }
        /// <summary>
        /// 给hashid数据集key的value加countby，返回相加后的数据
        /// </summary>
        public double IncrementValueInHash(string hashid, string key, double countBy)
        {
            return RedisBase.Core.IncrementValueInHash(hashid, key, countBy);
        }
        #endregion

        #region 扩展
        /// <summary>
        /// 根据key值，获取hash转换为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetHashToListCache<T>(string key)
        {
            List<T> list = new List<T>();
            var hashFields = GetHashValues(key);
            foreach (var field in hashFields)
            {
                list.Add(JsonConvert.DeserializeObject<T>(field));
            }
            return list;
        }

        #endregion

        //static void Main(string[] args)
        //{
        //    //清空数据库
        //    DoRedisBase.Core.FlushAll();
        //    //声明事务
        //    using (var tran = RedisManager.GetClient().CreateTransaction())
        //    {
        //        try
        //        {
        //            tran.QueueCommand(p =>
        //            {
        //                //操作redis数据命令
        //                Core.Set<int>("name", 30);
        //                long i = Core.IncrementValueBy("name", 1);
        //            });
        //            //提交事务
        //            tran.Commit();
        //        }
        //        catch
        //        {
        //            //回滚事务
        //            tran.Rollback();
        //        }
        //        ////操作redis数据命令
        //        //RedisManager.GetClient().Set<int>("zy", 30);
        //        ////声明锁，网页程序可获得锁效果
        //        //using (RedisManager.GetClient().AcquireLock("zy"))
        //        //{
        //        //    RedisManager.GetClient().Set<int>("zy", 31);
        //        //    Thread.Sleep(10000);
        //        //}
        //    }
        //    Console.ReadKey();
        //}

    }
}
