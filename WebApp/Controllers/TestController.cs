using RedisCommon;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            RedisHelper redis = new RedisHelper();

            redis.FlushDataBase("127.0.0.1:6379");
            //RedisString(redis);
            //RedisList(redis);
            //RedisHash(redis);
            RedisSortedSet(redis);
            //RedisPubSub(redis);
            //RedisTransaction(redis);

            //RedisLock(redis);
            return View();
        }

        static void RedisString(RedisHelper redis)
        {
            #region String

            string str = "123";
            Demo demo = new Demo()
            {
                Id = 1,
                Name = "123"
            };
            var resukt = redis.StringSet("redis_string_test", str);
            string str1 = redis.StringGet("redis_string_test");
            redis.StringSet("redis_string_model", demo);
            var model = redis.StringGet<Demo>("redis_string_model");

            for (int i = 0; i < 10; i++)
            {
                redis.StringIncrement("StringIncrement", 2);
            }
            for (int i = 0; i < 10; i++)
            {
                redis.StringDecrement("StringIncrement");
            }
            redis.StringSet("redis_string_model1", demo, TimeSpan.FromSeconds(10));

            #endregion String
        }

        static void RedisList(RedisHelper redis)
        {
            #region List

            for (int i = 0; i < 10; i++)
            {
                redis.ListRightPush("list", i);
            }

            for (int i = 10; i < 20; i++)
            {
                redis.ListLeftPush("list", i);
            }
            var length = redis.ListLength("list");

            var leftpop = redis.ListLeftPop<string>("list");
            var rightPop = redis.ListRightPop<string>("list");

            var list = redis.ListRange<int>("list");

            #endregion List
        }

        static void RedisHash(RedisHelper redis)
        {
            #region Hash
            //var dddd = redis.HashGet<string>("UserCache", "UserCacheda20d268-08e3-4aef-a9f3-4a8a2129d3a0");
            redis.HashSet("user", "u1", "123");
            redis.HashSet("user", "u2", "1234");
            redis.HashSet("user", "u3", "1235");
            var news = redis.HashGet<string>("user", "u2");
            #endregion Hash
        }

        static void RedisSortedSet(RedisHelper redis)
        {
            for(int i=0;i<10;i++)
            {
                Thread.Sleep(500);
                Demo demo = new Demo
                {
                    Id = new Random().Next(1,1000),
                    Name = "zhangyu" + i
                };
                redis.SortedSetAdd("demo", demo, demo.Id);
            }


            var demos = redis.SortedSetRangeByRank<Demo>("demo");
            var demotwo = redis.SortedSetRangeByRank<Demo>("demo", 0, 20, Order.Descending);
        }


        static void RedisPubSub(RedisHelper redis)
        {
            #region 发布订阅

            Action<string, string> handler = Con;
            //redis.Subscribe("Channel1",handler);
            redis.Subscribe("Channel1");
            for (int i = 0; i < 10; i++)
            {
                redis.Publish("Channel1", "msg" + i);
                if (i == 2)
                {
                    redis.Unsubscribe("Channel1");
                }
            }

            #endregion 发布订阅
        }

        static void Con(string one,string two)
        {
            Console.WriteLine(one + "," + two);

            System.Diagnostics.Debug.WriteLine(one + "订阅收到消息zhangyu：" + two);
        }


        static void RedisTransaction(RedisHelper redis)
        {
            #region 事务

            var tran = redis.CreateTransaction();

            tran.StringSetAsync("tran_string", "test1");
            tran.StringSetAsync("tran_string1", "test2");
            bool committed = tran.Execute();

            #endregion 事务
        }

        static void RedisLock(RedisHelper redis)
        {
            #region Lock

            var db = redis.GetDatabase();
            string token = Environment.MachineName;
            if (db.LockTake("lock_test", token, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    //TODO:开始做你需要的事情
                    Thread.Sleep(5000);

                    Debug.WriteLine("开始");
                }
                finally
                {
                    db.LockRelease("lock_test", token);
                }
            }

            #endregion Lock
        }
    }
    public class Demo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}