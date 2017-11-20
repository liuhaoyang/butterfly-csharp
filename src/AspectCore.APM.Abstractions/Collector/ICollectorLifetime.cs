using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ICollectorLifetime
    {
        bool Started { get; }

        bool Start();

        void Stop();
    }
}