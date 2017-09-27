using Autofac;
using Microsoft.Extensions.Configuration;
using Pogodynka.Infrastructure.IoC.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pogodynka.Infrastructure.IoC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;
        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<SqlModule>();
            builder.RegisterModule(new SettingsModule(_configuration));
        }
    }
}
