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

            //��ContainerBuilderע������ע��Ķ���
            var containerBuilder = new ContainerBuilder();
            //���ְ汾�£�������ͨ��name����connectionString�����޷�ͨ����ע�᷽ʽʹEF���������ݿ�
            //containerBuilder.Register(db => new EFDbContext("connStr")).As<IDbContext>().InstancePerDependency();
            containerBuilder.Register(db => new EFDbContext()).As<IDbContext>().InstancePerDependency();

            //containerBuilder.RegisterType(typeof(EFRepository<User>)).As(typeof(IEFRepository<User>));
            //containerBuilder.RegisterType(typeof(EFRepository<Article>)).As(typeof(IEFRepository<Article>));
            //����ע�᷽����ÿ�����һ��ʵ���࣬����Ҫע��һ�У��������ֱ��ע�᷺��
            containerBuilder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IEFRepository<>));

            var container = containerBuilder.Build();
            DIContainer.Register(container);

        }
    }
}
