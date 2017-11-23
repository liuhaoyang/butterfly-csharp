using System;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;
using AspectCore.Injector;

namespace AspectCore.APM.HttpProfiler
{
    public static class ComponentExtensions
    {
        public static ComponentOptions AddHttpProfiler(this ComponentOptions apmComponent)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }
            apmComponent.Services.AddType<IProfiler<HttpProfilingContext>, HttpProfiler>(Lifetime.Singleton);
            return apmComponent;
        }
    }
}