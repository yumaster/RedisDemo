using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using StackExchange.Redis;

namespace RedisCache
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfig RedisConfig = RedisConfig.GetConfig();
        private static PooledRedisClientManager prcm;
        /// <summary>
        /// 静态构造方法，初始化连接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }
        public static void CreateManager()
        {
            string[] WriteServerConStr = SplitString(RedisConfig.WriteServerConStr, ",");
            string[] ReadServerConStr = SplitString(RedisConfig.ReadServerConStr, ",");
            prcm = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = RedisConfig.MaxWritePoolSize,
                                 MaxReadPoolSize = RedisConfig.MaxReadPoolSize,
                                 AutoStart = RedisConfig.AutoStart,
                             });
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }
    }
}
