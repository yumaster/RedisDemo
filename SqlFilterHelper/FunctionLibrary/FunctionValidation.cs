﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFilterHelper.FunctionLibrary
{
    public partial class FunctionExe
    {
        /// <summary>
        /// 验证字段是否为空，必须加在字段上用
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        [CustomFun(FunName = "VALNULL", FunRemark = "验证是否为空",FunType ="前置",ExecMode = "例如：VALNULL[userid={0}]")]
        public static string VALNULL(string condition, List<string> paraList)
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");//如果condition 为用花括号包围的数字 则 获取参数的索引为大于-1的数字，否则为-1
            if (paraIndex > -1)//如果condition 为用花括号包围的数字 {0} 
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
        /// 参数不添加单引号
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        [CustomFun(FunName = "RMVDYH", FunRemark = "参数不添加单引号", FunType = "前置", ExecMode = "例如：RMVDYH[{0}]")]
        public static string RMVDYH(string condition, List<string> paraList)
        {
            string ret = string.Empty;
            int paraIndex = GetParaIndex(condition, "{", "}");//如果condition 为用花括号包围的数字 则 获取参数的索引为大于-1的数字，否则为-1
            if (paraIndex > -1)//如果condition 为用花括号包围的数字 {0} 
            {
                string paraStr = paraList[paraIndex];//从参数列表获取 参数
                ret = condition.Replace("{" + paraIndex + "}", "" + paraStr + "");//把原来的 condition：{0} 替换为用单引号括起来的处理后的参数 'pwd'
            }
            else//如果参数索引为-1,则证明condition中没有花括号，即没有数字参数索引，也可以理解为此参数已经被最内层的函数处理过  直接返回
            {
                ret = condition;
            }
            return ret;
        }

    }
}
