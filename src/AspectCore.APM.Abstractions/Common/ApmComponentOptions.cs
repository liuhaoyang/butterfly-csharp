using System;
using System.Collections.Generic;
using System.Reflection;
using AspectCore.Extensions.Reflection;
using AspectCore.Injector;

namespace AspectCore.APM.Common
{
    public class ApmComponentOptions
    {
        public IServiceContainer Services { get; }

        public ApmComponentOptions()
        {
            Services = new ServiceContainer();
            var collection = (ICollection<ServiceDefinition>)typeof(ServiceContainer).GetTypeInfo().
                GetField("_collection", BindingFlags.NonPublic | BindingFlags.Instance).GetReflector().GetValue(Services);
            collection.Clear();
        }
    }
}