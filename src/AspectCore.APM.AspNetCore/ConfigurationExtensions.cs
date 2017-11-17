using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AspectCore.APM.AspNetCore
{
   public static class ConfigurationExtensions
    {
        public static IConfigurationSection GetLineProtocolSection(this IConfiguration configuration)
        {
            return configuration.GetSection("AspectCoreAPM:Collector:LineProtocol");
            
        }
    }
}
