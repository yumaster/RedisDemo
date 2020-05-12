using Autofac;
using RedisStudy.Common;
using RedisStudy.DAL.Abstraction;
using RedisStudy.DAL.EF;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //用ContainerBuilder注册依赖注入的对象
            var containerBuilder = new ContainerBuilder();
            //部分版本下，无论是通过name还是connectionString，都无法通过该注册方式使EF连接上数据库
            //containerBuilder.Register(db => new EFDbContext("connStr")).As<IDbContext>().InstancePerDependency();
            containerBuilder.Register(db => new EFDbContext()).As<IDbContext>().InstancePerDependency();

            //containerBuilder.RegisterType(typeof(EFRepository<User>)).As(typeof(IEFRepository<User>));
            //containerBuilder.RegisterType(typeof(EFRepository<Article>)).As(typeof(IEFRepository<Article>));
            //上面注册方法，每次添加一个实体类，都需要注册一行，下面代码直接注册泛型
            containerBuilder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IEFRepository<>));

            var container = containerBuilder.Build();
            DIContainer.Register(container);

        }
    }
}
