using RedisCommon;
using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisHelper redis = new RedisHelper();
            List<Function> funAllList = redis.HashKeys<Function>("FunCache");//所有函数列表

            string paraStr1 = "admin$zhangyu";
            string paraStr2 = "zhangyu$F6F6CC9096591F2C";
            string paraStr3 = "$F6F6CC9096591F2C$zhangyu";


            string sql1 = "select * from userinfo where VALNULL[userid={0}] AND password=ENPWD[{1}]"; //当前SQL语句，包含两个函数

            string sql2 = "select * from userinfo where VALNULL[userid=BASE64[DEPWD[ENPWD[{0}]]]] AND pwd=DEPWD[{1}]"; //当前SQL语句，包含两个函数


            //string sql3 = "GETDATA[select * from userinfo where VALNULL[userid={0}] AND zhangyu=DEPWD[{1}]   AND VALNULL[password={2}]]"; //当前SQL语句，包含两个函数

            List<string> para1 = paraStr1.Split('$').ToList();//当前传入的参数列表
            List<string> para2 = paraStr2.Split('$').ToList();//当前传入的参数列表
            List<string> para3 = paraStr3.Split('$').ToList();

            //SqlFilter.TestTwo(sql1, para1);
            //Console.WriteLine("--------------------------------------------------------------------------------------");
            //SqlFilter.TestTwo(sql2, para2);
            //Console.WriteLine("--------------------------------------------------------------------------------------");


            Stopwatch sw = new Stopwatch();
            sw.Start();

            SqlFilter.TestTwo(sql1, para1, funAllList);
            Console.WriteLine("--------------------------------------------------------------------------------------");
            SqlFilter.TestTwo(sql2, para2, funAllList);
            Console.WriteLine("--------------------------------------------------------------------------------------");


            //SqlFilter.TestTwo(sql3, para3, funAllList);
            Console.WriteLine("--------------------------------------------------------------------------------------");

            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}毫秒.", ts2.TotalMilliseconds);
            Console.WriteLine("Stopwatch总共花费{0}秒.", ts2.TotalSeconds);
            sw.Stop();
            Console.WriteLine("--------------------------------------------------------------------------------------");

            Console.ReadKey();
        }
    }
}
