using Autofac;
using Microsoft.Extensions.Configuration;
using Pogodynka.Infrastructure.EF;
using Pogodynka.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pogodynka.Infrastructure.IoC.Modules
{
    public class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;
        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<SqlSettings>())
                .SingleInstance();
           
        }
    }
}
