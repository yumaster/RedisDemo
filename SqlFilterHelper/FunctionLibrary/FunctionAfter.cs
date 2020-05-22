using System;
using System.Collections.Generic;
using System.Data;
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
        /// <param name="obj"></param>
        /// <returns></returns>
        [CustomFun(FunName = "GETDATA", FunRemark = "处理数据集", FunType = "后置", ExecMode = "GETDATA[object]")]
        public static object GETDATA(object obj)
        {

            DataSet dstmp = new DataSet();
            if (dstmp.Tables.Contains("TAB_NM"))//已经存在该表的话，删除掉  
                dstmp.Tables.Remove("TAB_NM");
            //创建虚拟数据表  
            DataTable table = new DataTable("TAB_NM");
            //获取列集合,添加列  
            DataColumnCollection columns = table.Columns;
            columns.Add("col_id", typeof(String));
            columns.Add("opr_date", typeof(DateTime));
            //添加一行数据  
            for (int i = 1; i < 2; i++)
            {
                DataRow row = table.NewRow();
                row["col_id"] = "id" + i;
                row["opr_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                table.Rows.Add(row);
            }

            dstmp.Tables.Add(table); //把信息表放入DataSet中  

            obj = dstmp;



            return obj;
        }
        /// <summary>
        /// 获取list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [CustomFun(FunName = "GETLIST", FunRemark = "处理数据列表", FunType = "后置", ExecMode = "GETLIST[object]")]
        public static object GETLIST(object obj)
        {

            DataSet dstmp = new DataSet();
            if (dstmp.Tables.Contains("TAB_NM"))//已经存在该表的话，删除掉  
                dstmp.Tables.Remove("TAB_NM");
            //创建虚拟数据表  
            DataTable table = new DataTable("TAB_NM");
            //获取列集合,添加列  
            DataColumnCollection columns = table.Columns;
            columns.Add("col_id", typeof(String));
            columns.Add("opr_date", typeof(DateTime));
            //添加一行数据  
            for (int i = 1; i < 5; i++)
            {
                DataRow row = table.NewRow();
                row["col_id"] = "id" + i ;
                row["opr_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                table.Rows.Add(row);
            }

            dstmp.Tables.Add(table); //把信息表放入DataSet中  

            obj = dstmp;
            return obj;
        }
    }
}
