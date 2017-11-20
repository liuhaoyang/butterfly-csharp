using System;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;
using AspectCore.Injector;

namespace AspectCore.APM.ApplicationProfiler
{
    public static class ComponentExtensions
    {
        public static ApmComponentOptions AddApplicationProfiler(this ApmComponentOptions apmComponent)
        {
            return AddApplicationProfiler(apmComponent, null);
        }

        public static ApmComponentOptions AddApplicationProfiler(this ApmComponentOptions apmComponent, Action<ApplicationProfilingOptions> configure)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }
            var options = new ApplicationProfilingOptions();
            configure?.Invoke(options);
            apmComponent.Services.AddType<IOptionAccessor<ApplicationProfilingOptions>, ApplicationProfilingOptions>(Lifetime.Singleton);
            apmComponent.Services.AddType<IProfilerSetup, ApplicationProfilerSetup>(Lifetime.Singleton);
            apmComponent.Services.AddType<IProfiler<ApplicationGCProfilingContext>, ApplicationGCProfiler>(Lifetime.Singleton);
            return apmComponent;
        }
    }
}