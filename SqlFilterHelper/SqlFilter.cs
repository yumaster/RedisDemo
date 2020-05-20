using RedisCommon;
using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    public class SqlFilter
    {
        public static void TestTwo(string sqlStr, List<string> paraTest)
        {
            RedisHelper redis = new RedisHelper();
            List<Function> funAllList = redis.HashKeys<Function>("FunCache");//所有函数列表

            if (HasFunction(sqlStr, funAllList))
            {
                #region 先处理前置条件
                sqlStr = DoBefore(sqlStr, paraTest, funAllList.Where(x=>x.FunType=="前置").ToList());
                #endregion

                #region 再处理后置条件
                sqlStr = DoAfter(sqlStr, paraTest, funAllList.Where(x => x.FunType == "后置").ToList());
                #endregion
                Console.WriteLine("最终结果：" + sqlStr);
            }
            else
            {
                Console.WriteLine("最终结果：" + sqlStr);
            }
        }

        #region 核心方法
        /// <summary>
        /// 判断SQL中，是否还有未处理的函数
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="funAllList"></param>
        /// <returns></returns>
        public static bool HasFunction(string sqlStr, List<Function> funAllList)
        {
            bool ret = false;
            foreach (var item in funAllList)
            {
                if (sqlStr.Contains(item.FunName))
                {
                    ret = true;
                }
                else
                {
                    continue;
                }
            }
            return ret;
        }

        /// <summary>
        /// 递归，处理SQL
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paraAllList"></param>
        /// <param name="funAllList"></param>
        /// <returns></returns>
        public static string DoBefore(string sqlStr, List<string> paraAllList, List<Function> funAllList)
        {
            List<Function> funList = new List<Function>();//筛选到的SQL语句包含的函数
            foreach (var item in funAllList)
            {
                if (sqlStr.Contains(item.FunName))
                {
                    funList.Add(item);
                }
                else
                {
                    continue;
                }
            }
            if (funList.Count > 0)
            {
                foreach (var item in funList)
                {
                    string retFun = SearchString(sqlStr, item.FunName, "]");//VALNULL[userid={0}]  DEPWD[{1}]
                    //执行函数
                    Type type = Type.GetType("SqlFilterHelper.SqlFilter");//通过string类型的strClass获得同名类“type”
                    object obj = System.Activator.CreateInstance(type);//创建type类的实例 "obj"
                    MethodInfo method = type.GetMethod(item.FunName);//取的方法描述
                    object[] objs = new object[] { SearchString(retFun, "[", "]", false), paraAllList };
                    var ret = method.Invoke(obj, objs);//t类实例obj,调用方法"method(DEPWD)"
                    sqlStr = sqlStr.Replace(retFun.ToString(), ret.ToString());
                }
            }
            if (HasFunction(sqlStr, funAllList))
            {
                return DoBefore(sqlStr, paraAllList, funAllList);
            }
            return sqlStr;
        }


        public static string DoAfter(string sqlStr, List<string> paraAllList, List<Function> funAllList)
        {
            List<Function> funList = new List<Function>();//筛选到的SQL语句包含的函数
            foreach (var item in funAllList)
            {
                if (sqlStr.Contains(item.FunName))
                {
                    funList.Add(item);
                }
                else
                {
                    continue;
                }
            }
            if (funList.Count > 0)
            {
                foreach (var item in funList)
                {
                    string retFun = SearchString(sqlStr, item.FunName, "]");//VALNULL[userid={0}]  DEPWD[{1}]
                    //执行函数
                    Type type = Type.GetType("SqlFilterHelper.SqlFilter");//通过string类型的strClass获得同名类“type”
                    object obj = System.Activator.CreateInstance(type);//创建type类的实例 "obj"
                    MethodInfo method = type.GetMethod(item.FunName);//取的方法描述
                    object[] objs = new object[] { SearchString(retFun, "[", "]", false), paraAllList };
                    var ret = method.Invoke(obj, objs);//t类实例obj,调用方法"method(DEPWD)"
                    sqlStr = sqlStr.Replace(retFun.ToString(), ret.ToString());
                }
            }

            if (HasFunction(sqlStr, funAllList))
            {
                return DoAfter(sqlStr, paraAllList, funAllList);
            }
            return sqlStr;
        }
        #endregion


        #region 函数库
        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string ENPWD(string condition,List<string>paraList)
        {
            string ret = string.Empty;
            int paraIndex = int.Parse(SearchString(condition, "{", "}", false));
            string paraStr = paraList[paraIndex];
            if (!string.IsNullOrEmpty(paraStr))
            {
                string pwd = DESEncrypt.Encrypt(paraStr);//加密业务逻辑
                ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'");
            }
            return ret;
        }
        /// <summary>
        /// 解密算法
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string DEPWD(string condition, List<string> paraList)// user={0}   {1}
        {
            string ret = string.Empty;
            int paraIndex = int.Parse(SearchString(condition, "{", "}", false));
            string paraStr = paraList[paraIndex];
            if (!string.IsNullOrEmpty(paraStr))
            {
                string pwd = DESEncrypt.Decrypt(paraStr);//加密业务逻辑
                ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'");
            }
            return ret;
        }
        public static string VALNULL(string condition, List<string> paraList)
        {
            string ret = string.Empty;
            int paraIndex = int.Parse(SearchString(condition, "{", "}", false));
            string paraStr = paraList[paraIndex];
            if (!string.IsNullOrEmpty(paraStr))
            {
                string pwd = paraStr;//加密业务逻辑
                ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'");
            }
            else
            {
                ret = " 1=1 ";
            }
            return ret;
        }
        public static string GETDATA(string condition, List<string> paraList)
        {
            string ret = string.Empty;

            ret = condition;

            return ret;
        }
        #endregion


        #region 扩展方法，截取字符串
        /// <summary>
        /// 获取搜索到的数目（包含起止位）
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="s1">开始位置</param>
        /// <param name="s2">结束位置</param>
        /// <param name="HasS1S2">是否包含起止位</param>
        /// <returns></returns>
        public static string SearchString(string s, string s1, string s2, bool HasS1S2 = true)
        {
            int n1, n2;
            if (HasS1S2)
            {
                n1 = s.IndexOf(s1, 0);   //开始位置
                n2 = s.IndexOf(s2, n1) + s2.Length;//结束位置
            }
            else
            {
                n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置
                n2 = s.IndexOf(s2, n1);               //结束位置
            }
            return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回
        }
        #endregion
    }
}
