using AspectCore.DynamicProxy;

namespace AspectCore.APM.Profiler
{
    [NonAspect]
    public interface IProfilerSetup
    {
        bool Start();

        void Stop();
    }
}