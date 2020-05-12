using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStudy.DAL.Abstraction
{
    /// <summary>
    /// 控制整个仓储的事务
    /// </summary>
    public interface IDbContext
    {
        int Commit();
    }
}
