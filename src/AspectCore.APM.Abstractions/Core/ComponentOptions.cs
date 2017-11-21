using System.Collections.Generic;
using System.Reflection;
using AspectCore.Extensions.Reflection;
using AspectCore.Injector;

namespace AspectCore.APM.Core
{
    public class ComponentOptions
    {
        public IServiceContainer Services { get; }

        public ComponentOptions()
        {
            Services = new ServiceContainer();
            var collection = (ICollection<ServiceDefinition>)typeof(ServiceContainer).GetTypeInfo().
                GetField("_collection", BindingFlags.NonPublic | BindingFlags.Instance).GetReflector().GetValue(Services);
            collection.Clear();
        }
    }
}