using Newtonsoft.Json;
using RedisCommon;
using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            //RedisHelper redis = new RedisHelper();
            //List<Function> funAllList = redis.HashKeys<Function>("FunCache");//所有函数列表

            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Function> funAllList = SqlFilter.GetFunctionList();
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}毫秒.", ts2.TotalMilliseconds);
            Console.WriteLine("Stopwatch总共花费{0}秒.", ts2.TotalSeconds);
            Console.ReadKey();


            string paraStr1 = "admin$zhangyu";
            string paraStr2 = "zhangyu$F6F6CC9096591F2C";
            string paraStr3 = "$F6F6CC9096591F2C$zhangyu";


            string sql1 = "select * from userinfo where VALNULL[userid={0}] AND password=ENPWD[{1}]"; //当前SQL语句，包含两个函数

            string sql2 = "select * from userinfo where VALNULL[userid=BASE64[DEPWD[ENPWD[{0}]]]] AND pwd=DEPWD[{1}]"; //当前SQL语句，包含两个函数


            string sql3 = "GETDATA[select * from userinfo where VALNULL[userid={0}] AND zhangyu=DEPWD[{1}]   AND VALNULL[password={2}]]"; //当前SQL语句，包含两个函数

            List<string> para1 = paraStr1.Split('$').ToList();//当前传入的参数列表
            List<string> para2 = paraStr2.Split('$').ToList();//当前传入的参数列表
            List<string> para3 = paraStr3.Split('$').ToList();


            //Task<string> retone = SqlFilter.TestTwo(sql1, para1, funAllList);
            //Console.WriteLine(retone.Result);
            //Console.WriteLine("--------------------------------------------------------------------------------------");
            //Task<string> rettwo = SqlFilter.TestTwo(sql2, para2, funAllList);
            //Console.WriteLine(rettwo.Result);
            //Console.WriteLine("--------------------------------------------------------------------------------------");



            //Console.ReadKey();


            Stopwatch swone = new Stopwatch();
            swone.Start();

            Task<string> retconsole = SqlFilter.GetFilterSql(sql1, para1, funAllList);
            Console.WriteLine(retconsole.Result);
            retconsole = SqlFilter.GetFilterSql(sql2, para2, funAllList);
            Console.WriteLine(retconsole.Result);

            for (int i = 0; i < 1; i++)
            {
                Task<string> ret = SqlFilter.GetFilterSql(sql3, para3, funAllList);
                Console.WriteLine(ret.Result);
            }


            TimeSpan ts3 = swone.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}毫秒.", ts3.TotalMilliseconds);
            Console.WriteLine("Stopwatch总共花费{0}秒.", ts3.TotalSeconds);
            swone.Stop();
            Console.WriteLine("--------------------------------------------------------------------------------------");

            Console.ReadKey();
        }
    }
}
