using AspectCore.DynamicProxy;

namespace AspectCore.APM.Profiler
{
    [NonAspect]
    public interface IProfilingSetup
    {
        bool Start();

        void Stop();
    }
}