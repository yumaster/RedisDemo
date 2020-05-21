using RedisCommon;
using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    public class SqlFilter
    {
        public static void TestTwo(string sqlStr, List<string> paraList, List<Function> funAllList)
        {
            if (HasFunction(sqlStr, funAllList))
            {
                #region 先处理前置条件
                sqlStr = DoBefore(sqlStr, paraList, funAllList.Where(x=>x.FunType=="前置").ToList());
                #endregion
                
                #region 再处理后置条件
                //sqlStr = DoAfter(sqlStr, paraList, funAllList.Where(x => x.FunType == "后置").ToList());
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
        /// <param name="sqlStr">sql字符串</param>
        /// <param name="funAllList">所有的函数列表</param>
        /// <returns></returns>
        public static bool HasFunction(string sqlStr, List<Function> funAllList)
        {
            bool ret = false;
            foreach (var item in funAllList)
            {
                if (sqlStr.Contains(item.FunName))//如果字符串中包含函数名，则返回true，进行第二次sql处理，直到sql中不包含函数
                {
                    ret = true;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return ret;
        }

        /// <summary>
        /// 递归，处理前置函数  参数
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
            //funList为过滤之后的列表
            List<Function> lastList = new List<Function>();//从已经筛选过的函数列表进行排序，即从内到外的函数顺序
            foreach(var item in funList)
            {
                string retFun = SearchString(sqlStr, item.FunName, "]");//截取包含函数名的字符串 例如： VALNULL[userid={0}]  DEPWD[{1}]
                retFun = RetFunction(retFun, funAllList);//如果SQL语句的函数中有诸如此类的 BASE64[DEPWD[ENPWD[{0}]]] ，则先从最里面的开始，最先获取到的为  ENPWD、DEPWD、BASE64 依次排序
                var fun = funList.Single(x => retFun.Contains(x.FunName));
                if(!lastList.Contains(item))
                {
                    if (!lastList.Contains(fun))
                    {
                        lastList.Add(fun);
                    }
                    if(!lastList.Contains(item))
                    {
                        lastList.Add(item);
                    }
                }//lastList把所有的排序过的函数实体保存起来，进入下一步的sql处理
            }
            try
            {
                if (lastList.Count > 0)//如果有函数存在，则进入SQL中函数的处理
                {
                    foreach (var item in lastList)//
                    {
                        string retFun = SearchString(sqlStr, item.FunName, "]");//根据lastList函数的顺序，依次进行函数处理，此方法读取 “函数名[{参数}]”   VALNULL[userid={0}]  DEPWD[{1}]
                        Type type = Type.GetType("SqlFilterHelper.SqlFilter");//通过命名空间.类名获取类
                        object obj = System.Activator.CreateInstance(type);//创建type类的实例 "obj"
                        MethodInfo method = type.GetMethod(item.FunName);//取得方法描述
                        object[] objs = new object[] { SearchString(retFun, "[", "]", false), paraAllList };//传入方法的参数  参数1：函数名里面的内容、参数2：参数列表
                        var ret = method.Invoke(obj, objs);//t类实例obj,调用方法"method(DEPWD)"
                        sqlStr = sqlStr.Replace(retFun.ToString(), ret.ToString());//把原始sql语句中的 “函数名[{参数}]” 替换为  调用方法之后的返回值
                    }
                }
                if (HasFunction(sqlStr, funAllList))//处理完lastList中的函数之后，再次查看SQL语句是否还有未处理的函数，如果有，进行递归再次进行处理，直至处理完毕
                {
                    return DoBefore(sqlStr, paraAllList, funAllList);
                }
            }catch(Exception ex)
            {
                return sqlStr;
            }
            return sqlStr;
        }

        /// <summary>
        /// 递归，处理后置函数  整体
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paraAllList"></param>
        /// <param name="funAllList"></param>
        /// <returns></returns>
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
            try
            {
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
            }catch(Exception ex)
            {
                return sqlStr;
            }
            
            return sqlStr;
        }
        #endregion

        #region 函数库
        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string ENPWD(string condition,List<string>paraList)
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");  //如果condition 为用花括号包围的数字 则 获取参数的索引为大于-1的数字，否则为-1
            if (paraIndex > -1)//如果condition 为用花括号包围的数字 {0} 
            {
                string paraStr = paraList[paraIndex]; //从参数列表获取 参数
                string pwd = DESEncrypt.Encrypt(paraStr);// 参数进行 加密业务逻辑
                ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'"); //把原来的 condition：{0} 替换为用单引号括起来的处理后的参数 'pwd'
            }
            else//如果参数索引为-1,则证明condition中没有花括号，即没有数字参数索引，也可以理解为此参数已经被最内层的函数处理过
            {
                if (condition.IndexOf('\'') == 0 && condition.LastIndexOf('\'') == condition.Length - 1)//如果已经被最内层函数处理过一次，则condition的首末两位必定为单引号‘
                {
                    ret = "'" + DESEncrypt.Encrypt(condition.TrimStart('\'').TrimEnd('\'')) + "'";//再次进行函数的处理，处理完毕后用单引号包围
                }
                else//否则返回原表达式
                {
                    ret = condition;
                }
            }
            return ret;
        }
        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string DEPWD(string condition, List<string> paraList)// user={0}   {1}
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");//如果condition 为用花括号包围的数字 则 获取参数的索引为大于-1的数字，否则为-1
            if (paraIndex > -1)//如果condition 为用花括号包围的数字 {0} 
            {
                string paraStr = paraList[paraIndex]; //从参数列表获取 参数
                if (!string.IsNullOrEmpty(paraStr))//如果参数为空，则无法进行解密
                {
                    string pwd = DESEncrypt.Decrypt(paraStr);//解密业务逻辑
                    ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'");//把原来的 condition：{0} 替换为用单引号括起来的处理后的参数 'pwd'
                }
            }
            else//如果参数索引为-1,则证明condition中没有花括号，即没有数字参数索引，也可以理解为此参数已经被最内层的函数处理过
            {
                if (condition.IndexOf('\'') == 0 && condition.LastIndexOf('\'') == condition.Length - 1)//如果已经被最内层函数处理过一次，则condition的首末两位必定为单引号‘
                {
                    ret = "'" + DESEncrypt.Decrypt(condition.TrimStart('\'').TrimEnd('\'')) + "'";//再次进行函数的处理，处理完毕后用单引号包围
                } else
                {
                    ret = condition;
                }
            }
            return ret;
        }
        /// <summary>
        /// 验证字段是否为空，必须加在字段上用
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string VALNULL(string condition, List<string> paraList)
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");//如果condition 为用花括号包围的数字 则 获取参数的索引为大于-1的数字，否则为-1
            if (paraIndex>-1)//如果condition 为用花括号包围的数字 {0} 
            {
                string paraStr = paraList[paraIndex];//从参数列表获取 参数
                if (!string.IsNullOrEmpty(paraStr))//如果参数不为空
                {
                    string pwd = paraStr;
                    ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'");//把原来的 condition：{0} 替换为用单引号括起来的处理后的参数 'pwd'
                }
                else//否则替换为 1=1
                {
                    ret = " 1=1 ";
                }
            }
            else//如果参数索引为-1,则证明condition中没有花括号，即没有数字参数索引，也可以理解为此参数已经被最内层的函数处理过  直接返回
            {
                ret = condition;
            }
            return ret;
        }
        /// <summary>
        /// 后置函数，处理sql执行后的数据集
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string GETDATA(string condition, List<string> paraList)
        {
            string ret = string.Empty;

            ret = condition;

            return ret;

        }

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string BASE64(string condition,List<string>paraList)
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");
            if (paraIndex > -1)
            {
                string paraStr = paraList[paraIndex];
                if (!string.IsNullOrEmpty(paraStr))
                {
                    string pwd  = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(paraStr)); ;//加密业务逻辑
                    ret = condition.Replace("{" + paraIndex + "}", "'" + pwd + "'");
                }
            }
            else
            {
                if (condition.IndexOf('\'') == 0 && condition.LastIndexOf('\'') == condition.Length - 1)
                {
                    ret = condition.TrimStart('\'').TrimEnd('\'');
                    ret = "'" + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ret)) + "'";
                }
                else
                {
                    ret = condition;
                }
            }
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
            try
            {
                if (HasS1S2)//true：包含起止位
                {
                    n1 = s.IndexOf(s1, 0);   //开始位置
                    n2 = s.IndexOf(s2, n1) + s2.Length;//结束位置
                }
                else//false：不包含起止位
                {
                    n1 = s.IndexOf(s1, 0) + s1.Length; //开始位置
                    n2 = s.IndexOf(s2, n1); //结束位置
                }
                return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回
            }
            catch(Exception ex)
            {
                return s;//返回原始字符串
            }
        }
        /// <summary>
        /// 获取参数索引-均为此结构：{0}
        /// </summary>
        /// <param name="s">例{0}</param>
        /// <param name="s1">{</param>
        /// <param name="s2">}</param>
        /// <returns>例如{0},则从{和}中截取为0，如果异常返回-1</returns>
        public static int GetParaIndex(string s,string s1,string s2)
        {
            try
            {
                int n1, n2;
                n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置
                n2 = s.IndexOf(s2, n1);               //结束位置
                return int.Parse(s.Substring(n1, n2 - n1));   //取搜索的条数，用结束的位置-开始的位置,并返回
            }
            catch(Exception )
            {
                return -1;
            }
        }

        /// <summary>
        /// 递归，获取最内层的函数
        /// 如果SQL语句的函数中有诸如此类的 BASE64[DEPWD[ENPWD[{0}]]] ，则先从最里面的开始，最先获取到的为ENPWD  ----  ENPWD、DEPWD、BASE64 依次排序
        /// </summary>
        /// <param name="retFunOld">例如：</param>
        /// <param name="funAllList">函数列表</param>
        /// <returns></returns>
        public static string RetFunction(string retFunOld, List<Function> funAllList)
        {
            List<string> ListExist = new List<string>();
            foreach (var item in funAllList)
            {
                if (retFunOld.Contains(item.FunName))//如果retFunOld中包含函数，则加入到ListExist
                {
                    ListExist.Add(item.FunName);
                }
                else
                {
                    continue;
                }
            }
            if (ListExist.Count > 1)//如果retFunOld包含的函数大于1个，及2个及以上
            {
                retFunOld = retFunOld.Replace(ListExist.First(), "");
                return RetFunction(retFunOld, funAllList);
            }
            return retFunOld;
        }
        #endregion
    }
}
