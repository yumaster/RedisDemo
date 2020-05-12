using RedisStudy.DAL.Abstraction;
using RedisStudy.DAL.Abstraction.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RedisStudy.DAL.EF
{
    public class EFDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// 默认构造，想直接使用链接名，链接处应配置providerName
        /// </summary>
        public EFDbContext() : base("connStr")
        {

        }

        /// <summary>
        /// 部分版本通过IOC注册时传入的数据库链接，EF无法链接数据库，因此注册时要使用无参构造
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public EFDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        public int Commit() => base.SaveChanges();

        public DbSet<User> User { get; set; }

        public DbSet<Article> Article { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //移除表名复数  默认情况下会生成复数形式的表 如 Users
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //在上方通过DbSet或在此处将实体加入上下文，配置表约束
            //modelBuilder.Entity<Article>();
        }
    }
}
