using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStudy.Common
{
    /// <summary>
    /// Dependency Injection (DI) Container 【依赖注入容器】
    /// </summary>
    public class DIContainer
    {
        public static IContainer Container { get; private set; }

        public static void Register(IContainer container)
        {
            Container = container;
        }
    }
}
