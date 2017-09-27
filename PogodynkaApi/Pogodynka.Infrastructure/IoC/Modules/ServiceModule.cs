using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Autofac;
using Pogodynka.Infrastructure.Services;

namespace Pogodynka.Infrastructure.IoC.Modules
{
    public class ServiceModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(SqlModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                        .Where(x => x.IsAssignableTo<IService>())
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
        }
    }
}
