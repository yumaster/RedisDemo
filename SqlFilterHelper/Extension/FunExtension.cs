using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper
{
    public class FunExtension
    {
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
            catch (Exception ex)
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
        public static int GetParaIndex(string s, string s1, string s2)
        {
            try
            {
                int n1, n2;
                n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置
                n2 = s.IndexOf(s2, n1);               //结束位置
                return int.Parse(s.Substring(n1, n2 - n1));   //取搜索的条数，用结束的位置-开始的位置,并返回
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion
    }
}
