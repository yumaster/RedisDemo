using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper.FunctionLibrary
{
    public partial class FunctionExe : FunExtension
    {
        #region 函数库
        /// <summary>
        /// md5加密函数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string ENPWD(string condition, List<string> paraList)
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
        /// md5解密函数
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
                }
                else
                {
                    ret = condition;
                }
            }
            return ret;
        }
        



        /// <summary>
        /// BASE64函数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static string BASE64(string condition, List<string> paraList)
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");
            if (paraIndex > -1)
            {
                string paraStr = paraList[paraIndex];
                if (!string.IsNullOrEmpty(paraStr))
                {
                    string pwd = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(paraStr)); ;//加密业务逻辑
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
    }
}
