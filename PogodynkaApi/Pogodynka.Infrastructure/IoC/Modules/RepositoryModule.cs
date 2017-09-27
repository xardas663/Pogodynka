using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Pogodynka.Core.Repositiories;

namespace Pogodynka.Infrastructure.IoC.Modules
{
    public class RepositoryModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(SqlModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
