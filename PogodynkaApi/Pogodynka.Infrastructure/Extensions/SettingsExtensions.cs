
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pogodynka.Infrastructure.Extensions
{
    public static class SettingExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration) where T : new()
        {
            var section = typeof(T).Name.Replace("Settings", string.Empty);
            var ConfigurationValue = new T();
            configuration.GetSection(section).Bind(ConfigurationValue);

            return ConfigurationValue;
        }
    }
}



