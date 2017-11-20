using System;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;
using AspectCore.Injector;

namespace AspectCore.APM.HttpProfiler
{
    public static class ComponentExtensions
    {
        public static ApmComponentOptions AddHttpProfiler(this ApmComponentOptions apmComponent)
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