using Newtonsoft.Json;
using RedisCommon;
using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            List<Function> funAllList = SqlFilter.GetFunctionList();
            //sw.Stop();
            //TimeSpan ts2 = sw.Elapsed;
            //Console.WriteLine("Stopwatch总共花费{0}毫秒.", ts2.TotalMilliseconds);
            //Console.WriteLine("Stopwatch总共花费{0}秒.", ts2.TotalSeconds);
            //Console.ReadKey();




            string paras = "userinfo$username$pwd$描述";
            string sqls = "alter table {0} add {1}{2} ;EXECUTE   sp_addextendedproperty   N''MS_Description'',''{3}'',N''user'',N''dbo'',N''table'',N''{0}'',N''column'',N''{1}'' ";


            #region 查询
            //string paraStr1 = "select * from userinfo where VALNULL[userid={0}]";
            //string paraStr2 = "zhangyu$F6F6CC9096591F2C";
            //string paraStr3 = "$F6F6CC9096591F2C$zhangyu";
            //string sql1 = "select * from userinfo where userid={0}"; //当前SQL语句，包含两个函数
            //string sql2 = "select * from userinfo where VALNULL[userid=BASE64[DEPWD[ENPWD[{0}]]]] AND pwd=DEPWD[{1}]"; //当前SQL语句，包含两个函数
            //string sql3 = "GETLIST[GETDATA[select * from userinfo where VALNULL[userid={0}] AND zhangyu=DEPWD[{1}]   AND VALNULL[password={2}]]]"; //当前SQL语句，包含两个函数

            //List<string> para1 = paraStr1.Split('$').ToList();//当前传入的参数列表
            //List<string> para2 = paraStr2.Split('$').ToList();//当前传入的参数列表
            //List<string> para3 = paraStr3.Split('$').ToList();

            Task<string> ret = SqlFilter.GetFilterSql(sqls, paras.Split('$').ToList(), funAllList);
            Console.WriteLine(ret.Result);
            //Console.WriteLine("--------------------------------------------------------------------------------------");
            //Task<string> rettwo = SqlFilter.GetFilterSql(sql2, para2, funAllList);
            //Console.WriteLine(rettwo.Result);
            //Console.WriteLine("--------------------------------------------------------------------------------------");

            //Task<string> ret = SqlFilter.GetFilterSql(sql3, para3, funAllList.Where(x=>x.FunType=="前置").ToList());
            //Console.WriteLine(ret.Result);

            #endregion

            #region 查询之后的后置函数
            if (SqlFilter.HasFunction(ret.Result, funAllList.Where(x => x.FunType == "后置").ToList()))
            {
                string afterSql = SqlFilter.GetAfterSql(ret.Result, funAllList).Result;

                DataSet dstmp = new DataSet();
                if (dstmp.Tables.Contains("TAB_NM"))//已经存在该表的话，删除掉  
                    dstmp.Tables.Remove("TAB_NM");
                //创建虚拟数据表  
                DataTable table = new DataTable("TAB_NM");
                //获取列集合,添加列  z
                DataColumnCollection columns = table.Columns;
                columns.Add("col_id", typeof(String));
                columns.Add("opr_date", typeof(DateTime));
                //添加一行数据  
                for(int i=1;i<11;i++)
                {
                    DataRow row = table.NewRow();
                    row["col_id"] = "id" + i + i * 2 * 2 * 2;
                    row["opr_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    table.Rows.Add(row);
                }

                dstmp.Tables.Add(table); //把信息表放入DataSet中  


                List<Function> functions = SqlFilter.GetOrderFunction(ret.Result, funAllList);
                var obj = SqlFilter.GetAfterFun(dstmp, functions);

                Console.WriteLine("返回true," + afterSql + "");
            }
            else
            {
                Console.WriteLine("返回false,直接查询数据集");
            }
            #endregion


            #region 修改
            //string para1 = "方法名$CSIHelper.Helper.GetPopedoms({0})$true";
            //string sql1 = "insert into tableName (MethodName,MethodSql,IsSysMethod) values(VALNULL[{0}],{1},{2})";
            //Task<string> retone = SqlFilter.GetFilterSql(sql1, para1.Split('$').ToList(), funAllList);
            //Console.WriteLine(retone.Result);
            #endregion


            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.ReadKey();
        }
    }
}
