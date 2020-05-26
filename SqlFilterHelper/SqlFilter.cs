using Newtonsoft.Json;
using RedisStudy.DAL.Abstraction.Models;
using SqlFilterHelper.FunctionLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    public class SqlFilter : FunExtension
    {
        #region 前置-获取过滤前置条件后的sql
        /// <summary>
        /// 获取过滤后的sql
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static Task<string> GetFilterSql(string sqlStr, List<string> paraList)
        {
            List<Function> funAllList = GetFunctionList().Where(x => x.FunType == "前置").ToList();
            var task = Task.Run(() =>
            {
                if (HasFunction(sqlStr, funAllList))
                {
                    //处理前置条件
                    sqlStr = DoBefore(sqlStr, paraList, funAllList);
                }
                else//如果没有前置条件，直接替换参数即可
                {
                    List<string> paraNewList = paraList.Select(x => x.Replace("{", "`").Replace("}", "^")).ToList();
                    //for (int i = 0; i < paraNewList.Count; i++)
                    //{
                    //    string[] sparas = paraNewList[i].Split('~');
                    //    string moresql = "";
                    //    for (int j = 0; j < sparas.Length; j++)
                    //    {
                    //        if (i == 0)//第一次拼sql
                    //        {
                    //            moresql += sqlStr.Replace("{" + i + "}", "'" + sparas[j] + "'") + ";";
                    //        }
                    //        else//只替换如：{0}{1}{2}...
                    //        {
                    //            moresql += sqlStr.Split(';')[j].Replace("{" + i + "}", "'" + sparas[j] + "'") + ";";
                    //        }
                    //    }
                    //    sqlStr = moresql;
                    //}

                    sqlStr = ReplacePara(sqlStr, paraNewList);
                    sqlStr = sqlStr.Replace("`", "{").Replace("^", "}");
                }
                return sqlStr;
            });
            return task;
        }
        /// <summary>
        /// 递归，处理前置函数  参数
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paraAllList"></param>
        /// <param name="funAllList"></param>
        /// <returns></returns>
        public static string DoBefore(string sqlStr, List<string> paraAllListOld, List<Function> funAllList)
        {
            List<string> paraAllList = paraAllListOld.Select(x => x.Replace("{", "`").Replace("}", "^")).ToList();//把参数中花括号进行替换

            List<Function> lastList = GetOrderFunction(sqlStr, funAllList);//从已经筛选过的函数列表进行排序，即从内到外的函数顺序
            try
            {
                if (lastList.Count > 0)//如果有函数存在，则进入SQL中函数的处理
                {
                    foreach (var item in lastList)//
                    {
                        string retFun = SearchString(sqlStr, item.FunName, "]");//根据lastList函数的顺序，依次进行函数处理，此方法读取 “函数名[{参数}]”   VALNULL[userid={0}]  DEPWD[{1}]
                        Type type = Type.GetType("SqlFilterHelper.FunctionLibrary.FunctionExe");//通过命名空间.类名获取类
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
            }
            catch (Exception ex)
            {
                return sqlStr;
            }
            sqlStr = ReplacePara(sqlStr, paraAllList);//替换没有包含函数的参数
            sqlStr = sqlStr.Replace("`", "{").Replace("^", "}");//整条SQL把花括号还原，即还原之前参数中的花括号
            return sqlStr;
        }
        #endregion


        #region 后置-获取后置函数处理后的SQL

        /// <summary>
        /// 根据已经过滤后的函数列表，顺序执行后置函数，结果返回object
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="objPara"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object GetAfterFun(string sqlStr, object objPara)
        {
            List<Function> funAllList = GetFunctionList().Where(x => x.FunType == "后置").ToList();
            List<Function> lastList = GetOrderFunction(sqlStr, funAllList);

            object objRet = objPara;
            try
            {
                if (lastList.Count > 0)//如果有函数存在，则进入SQL中函数的处理
                {
                    foreach (var item in lastList)//
                    {
                        Type type = Type.GetType("SqlFilterHelper.FunctionLibrary.FunctionExe");//通过命名空间.类名获取类
                        object obj = System.Activator.CreateInstance(type);//创建type类的实例 "obj"
                        MethodInfo method = type.GetMethod(item.FunName);//取得方法描述
                        object[] objs = new object[] { objRet };//传入方法的参数  参数1：函数名里面的内容、参数2：参数列表
                        objRet = method.Invoke(obj, objs);//t类实例obj,调用方法"method(DEPWD)"
                    }
                }
            }
            catch (Exception ex)
            {
                return objRet;
            }
            return objRet;
        }


        /// <summary>
        /// 获取后置函数处理后的SQL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Task<string> GetAfterSql(string sqlStr)
        {
            List<Function> funAllList = GetFunctionList().Where(x => x.FunType == "后置").ToList();
            var task = Task.Run(() =>
            {
                if (HasFunction(sqlStr, funAllList))
                {
                    sqlStr = DoAfter(sqlStr, funAllList);
                }
                return sqlStr;
            });
            return task;
        }
        /// <summary>
        /// 递归，处理后置函数  整体
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paraAllList"></param>
        /// <param name="funAllList"></param>
        /// <returns></returns>
        public static string DoAfter(string sqlStr, List<Function> funAllList)
        {
            List<Function> lastList = GetOrderFunction(sqlStr, funAllList);//从已经筛选过的函数列表进行排序，即从内到外的函数顺序
            try
            {
                if (lastList.Count > 0)
                {
                    string retFun = SearchString(sqlStr, lastList[0].FunName, "]");//VALNULL[userid={0}]  DEPWD[{1}]
                    sqlStr = sqlStr = sqlStr.Replace(retFun.ToString(), SearchString(retFun, "[", "]", false));
                }
                if (HasFunction(sqlStr, funAllList))
                {
                    return DoAfter(sqlStr, funAllList);
                }
            }
            catch (Exception ex)
            {
                return sqlStr;
            }
            return sqlStr;
        }
        #endregion
        

        #region 扩展方法  获取排序后的函数列表、  获取最内层的函数
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
                    int nameIndex = sqlStr.IndexOf(item.FunName);//获取函数的位置，检查右边第一个字符是否为 [  避免死循环
                    if (sqlStr.Substring(nameIndex + item.FunName.Length, 1) == "[")
                    {
                        ret = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取排序之后的函数列表。即由内而外进行降序排列  最内层的函数最先执行
        /// 函数名称不能重复
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="funAllList"></param>
        /// <returns></returns>
        public static List<Function> GetOrderFunction(string sqlStr, List<Function> funAllList)
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
            foreach (var item in funList)
            {
                string retFun = SearchString(sqlStr, item.FunName, "]");//截取包含函数名的字符串 例如： VALNULL[userid={0}]  DEPWD[{1}]
                retFun = RetFunction(retFun, funAllList);//如果SQL语句的函数中有诸如此类的 BASE64[DEPWD[ENPWD[{0}]]] ，则先从最里面的开始，最先获取到的为  ENPWD、DEPWD、BASE64 依次排序
                var fun = funList.Single(x => retFun.Contains(x.FunName));
                if (!lastList.Contains(item))
                {
                    if (!lastList.Contains(fun))
                    {
                        lastList.Add(fun);
                    }
                    if (!lastList.Contains(item))
                    {
                        lastList.Add(item);
                    }
                }//lastList把所有的排序过的函数实体保存起来，进入下一步的sql处理
            }
            return lastList;
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

        #region 外部调用  获取函数列表
        /// <summary>
        /// 获取函数列表
        /// </summary>
        /// <returns></returns>
        public static List<Function> GetFunctionList()
        {
            //SqlFilterHelper.FunctionLibrary.FunctionExe
            //反射自己这个类
            List<Function> funAllList = new List<Function>();
            Type t = Type.GetType("SqlFilterHelper.FunctionLibrary.FunctionExe");
            //拿去本类的方法
            MethodInfo[] methodList = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var item in methodList)
            {
                object[] _attrs = item.GetCustomAttributes(typeof(CustomFunAttribute), false);  //反射获得用户自定义属性
                if (_attrs.Length != 0)
                {
                    string ret = JsonConvert.SerializeObject(_attrs[0]);
                    Function fun = JsonConvert.DeserializeObject<Function>(ret);
                    funAllList.Add(fun);
                }
            }
            return funAllList;
        }
        #endregion
    }
}
