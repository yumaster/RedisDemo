using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCommon
{
    public class ConfigInfo
    {
        private static string _RedisConnStr;
        private static string _RedisKeyStr;
        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public static string RedisConnStr
        {
            get { 
                if(string.IsNullOrEmpty(_RedisConnStr))
                {
                    _RedisConnStr = ConfigurationManager.ConnectionStrings["RedisExchangeHosts"].ConnectionString;
                }
                return _RedisConnStr;
            }
        }

        public static string RedisKeyStr
        {
            get
            {
                if(string.IsNullOrEmpty(_RedisConnStr))
                {
                    _RedisKeyStr = ConfigurationManager.AppSettings["redisKey"] ?? "";
                }
                return _RedisKeyStr;
            }
        }
    }
}
