using RedisStudy.DAL.Abstraction;
using System.Linq;

namespace RedisStudy.DAL.EF
{
    /// <summary>
    /// 用于扩展EF定制化的操作方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEFRepository<T> : IRepository<T>
    {
        /// <summary>
        /// 用于操作数据表
        /// </summary>
        IQueryable<T> Table { get; }
    }
}
