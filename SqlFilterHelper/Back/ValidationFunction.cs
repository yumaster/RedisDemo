using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    public class ValidationFunction
    {
        public static DataSet DataResultFilter(string sqlstr, string paras)
        {
            sqlstr = "FunName[ select * from (SELECT  ( a.object_id) as NO,a.name AS name,isnull(g.[value],'-') as TableExplain from sys.tables a left join sys.extended_properties g on(a.object_id =g.major_id AND g.minor_id=0)) A order by NO]";
            string funName = sqlstr.Substring(0, sqlstr.IndexOf('[')).Trim();
            sqlstr = SqlFilter(Search_string(sqlstr, "[", "]"), paras);


            //return HSDZLibrary.DBLibrary.GetDataSet(sqlstr);
            return new DataSet();
        }

        public static string SqlFilterTwo(string sqlstr, string paras)
        {
            #region SQL前置 参数处理
            List<string> paraList = paras.Split('$').ToList();
            //1.首先把SQL语句从WHERE处分割，只获取WHERE条件后的参数
            var sqlLeft = sqlstr.Split(new string[] { "WHERE" }, StringSplitOptions.RemoveEmptyEntries)[0];
            var sqlRight = sqlstr.Split(new string[] { "WHERE" }, StringSplitOptions.RemoveEmptyEntries)[1];
            string[] sArray = sqlRight.Split(new string[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
            sArray = sArray.Where(x => x.Contains("{") && x.Contains("}")).ToArray();
            sqlLeft += " WHERE 1=1 ";
            for (int i = 0; i < sArray.Length; i++) //userid={CVTDATE[0]}
            {
                string paraLeft = sArray[i].Split('=')[0];
                string paraRight = sArray[i].Split('=')[1];
                string paraStr = paras.Split('$')[i];
                if (paraRight.Contains("{") && paraRight.Contains("}") && paraRight.Contains("[") && paraRight.Contains("]"))//如果包含{CVTDATE[0]} 则为参数带有函数
                {
                    paraRight = Search_string(paraRight, "{", "}");//例如 CVTDATE[0]
                    //调用函数处理，返回处理过后 = 右边的数值，去除函数名及[]符号，返回{0}，并且把参数修改
                    string paraNew = ValidationPara(paraRight, paraStr);
                    paraList[i] = paraNew;
                    sqlLeft += " AND " + paraLeft + "={" + paraRight.Replace(paraRight.Split('[')[0], "").Replace("[", "").Replace("]", "") + "}";
                }
                else//如果不包含，则可能为SQL处理 即 VALNULL[USERID={0}]/或者 USERID={0}
                {
                    sqlLeft += " AND " + sArray[i];
                }
            }
            sqlstr = sqlLeft;
            return sqlstr;
            #endregion
        }



        public static string SqlFilter(string sqlstr, string paras)
        {
            var sqlLeft = sqlstr.Split(new string[] { "WHERE" }, StringSplitOptions.RemoveEmptyEntries)[0];
            var sqlRight = sqlstr.Split(new string[] { "WHERE" }, StringSplitOptions.RemoveEmptyEntries)[1];
            string[] sArray = sqlRight.Split(new string[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
            sArray = sArray.Where(x => x.Contains("{") && x.Contains("}")).ToArray();
            sqlLeft += " WHERE 1=1 ";
            for (int i = 0; i < sArray.Length; i++)
            {
                string tempStr = sArray[i];
                string paraStr = paras.Split('$')[i];
                string ret = Validation(tempStr, paraStr);
                if (string.IsNullOrEmpty(ret))
                {
                    continue;
                }
                else
                {
                    sqlLeft += " AND" + ret;
                }
            }
            sqlstr = sqlLeft;
            return sqlstr;
        }
        /// <summary>
        /// 根据条件参数，匹配对应的函数
        /// </summary>
        /// <param name="condition"></param>
        public static string Validation(string condition, string paraNow)
        {
            string retStr = string.Empty;
            if (condition.Contains("[") && condition.Contains("]"))//如果包含[] 则其为函数
            {
                //获取函数名
                string funName = condition.Split('[')[0];
                //反射
                Type type = Type.GetType("CSIHelper.VF");//通过string类型的strClass获得同名类“type”
                object obj = System.Activator.CreateInstance(type);//创建type类的实例 "obj"

                MethodInfo method = type.GetMethod(funName.Trim(), new Type[] { typeof(String), typeof(String) });//取的方法描述//2
                object[] objs = new object[] { Search_string(condition, "[", "]"), paraNow };
                var ret = method.Invoke(obj, objs);//t类实例obj,调用方法"method(testcase)"//2

                retStr = ret.ToString();
            }
            else
            {
                retStr = condition.Replace("{" + Search_string(condition, "{", "}") + "}", "'" + paraNow + "'");
            }
            return retStr;
        }
        /// <summary>
        /// 验证是否为空
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraNow"></param>
        /// <returns></returns>
        public static string VALNULL(string condition, string paraNow)
        {
            string ret = string.Empty;
            if (!string.IsNullOrEmpty(paraNow))
            {
                string oldStr = "{" + Search_string(condition, "{", "}") + "}";
                ret = condition.Replace(oldStr, "'" + paraNow + "'");
            }
            return ret;
        }





        public static string ValidationPara(string condition, string paraNow)
        {
            string retStr = string.Empty;
            if (condition.Contains("[") && condition.Contains("]"))//如果包含[] 则其为函数
            {
                //获取函数名
                string funName = condition.Split('[')[0];
                //反射
                Type type = Type.GetType("CSIHelper.VF");//通过string类型的strClass获得同名类“type”
                object obj = System.Activator.CreateInstance(type);//创建type类的实例 "obj"

                MethodInfo method = type.GetMethod(funName.Trim(), new Type[] { typeof(String), typeof(String) });//取的方法描述//2
                object[] objs = new object[] { condition, paraNow };
                var ret = method.Invoke(obj, objs);//t类实例obj,调用方法"method(testcase)"//2

                retStr = ret.ToString();
            }
            else
            {
                retStr = condition.Replace(condition.Split('[')[0], "").Replace("[", "").Replace("]", "");
            }
            return retStr;
        }
        /// <summary>
        /// 转换为SQL中的日期
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraNow"></param>
        /// <returns></returns>
        public static string CVTDATE(string condition, string paraNow)
        {
            if (!string.IsNullOrEmpty(paraNow))
            {
                return "TO_DATE('" + paraNow + "','YYYY/MM/DD')";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 搜索指定字符串中间的字符串
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="s1">左</param>
        /// <param name="s2">右</param>
        /// <returns></returns>
        public static string Search_string(string s, string s1, string s2)  //获取搜索到的数目
        {
            int n1, n2;
            n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置
            //n2 = s.IndexOf(s2, n1);               //结束位置
            n2 = s.LastIndexOf(s2);               //结束位置
            return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回
        }
    }
}
