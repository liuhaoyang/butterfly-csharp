using System;
using System.Collections.Generic;
using System.Text;
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
            apmComponent.Services.AddType<IProfilingSetup, ApplicationProfilingSetup>(Lifetime.Singleton);
            apmComponent.Services.AddType<IProfilingCallback<ApplicationGCProfilingCallbackContext>, ApplicationGCProfilingCallback>();
            return apmComponent;
        }
    }
}