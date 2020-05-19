using RedisCommon;
using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string paraStr1 = "admin$******";
            string paraStr2 = "$******";


            string sql1 = "select * from userinfo where VALNULL[userid={0}] AND password=DEPWD[{1}]"; //当前SQL语句，包含两个函数
            string sql2 = "select * from userinfo where VALNULL[userid={0}] AND password=DEPWD[{1}]"; //当前SQL语句，包含两个函数


            List<string> para1 = paraStr1.Split('$').ToList();//当前传入的参数列表
            List<string> para2 = paraStr2.Split('$').ToList();//当前传入的参数列表
            SqlFilter.test(sql1, para1);
            Console.WriteLine("--------------------------------------------------------------------------------------");
            SqlFilter.test(sql2, para2);




            //RedisHelper redis = new RedisHelper();
            //List<Function> funDbList = redis.HashKeys<Function>("FunCache");

            Console.ReadKey();
        }
    }
}
