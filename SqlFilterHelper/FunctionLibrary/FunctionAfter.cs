using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper.FunctionLibrary
{
    public partial class FunctionExe
    {
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
    }
}
