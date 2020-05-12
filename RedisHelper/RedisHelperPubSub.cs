using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCommon
{
    /// <summary>
    /// 发布订阅
    /// </summary>
    public partial class RedisHelper
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel,Action<string,string>handler=null)
        {
            ISubscriber sub = Multiplexer.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + "订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel,T msg)
        {
            ISubscriber sub = Multiplexer.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = Multiplexer.GetSubscriber();
            sub.Unsubscribe(channel);
        }
        /// <summary>
        /// 取消全部订阅
        /// </summary>
        public void UnSubscribeAll()
        {
            ISubscriber sub = Multiplexer.GetSubscriber();
            sub.UnsubscribeAll();
        }
    }
}
