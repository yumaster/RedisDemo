using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCommon
{
    public class ConnectionMultiplexerHelp
    {
        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _Multiplexer;//多路复用器
        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        static ConnectionMultiplexerHelp()
        {
            if(_Multiplexer==null)
            {
                lock(Locker)
                {
                    if(_Multiplexer==null||!_Multiplexer.IsConnected)
                    {
                        _Multiplexer = GetManager(ConfigInfo.RedisConnStr);
                    }
                }
            }
        }

        private static ConnectionMultiplexer GetManager(string redisCon=null)
        {
            ConnectionMultiplexer connect = ConnectionMultiplexer.Connect(redisCon);
            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;
            return connect;
        }

        /// <summary>
        /// 获取Redis
        /// </summary>
        /// <param name="redisCon"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetMultiplexer(string redisCon)
        {
            if(string.IsNullOrEmpty(redisCon))
            {
                redisCon = ConfigInfo.RedisConnStr;
            }
            if(!ConnectionCache.ContainsKey(redisCon))
            {
                ConnectionCache[redisCon] = GetManager(redisCon);
            }
            return ConnectionCache[redisCon];
        }

        #region 事件
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender,EndPointEventArgs e)
        {
            Console.WriteLine("Configuration changed: " + e.EndPoint);
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender,RedisErrorEventArgs e)
        {
            Console.WriteLine("ErrorMessage: " + e.Message);
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine("InternalError:Message" + e.Exception.Message);
        }
        #endregion
    }
}
