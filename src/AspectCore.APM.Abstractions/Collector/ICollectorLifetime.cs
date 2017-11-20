using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ICollectorLifetime
    {
        bool Start();

        void Stop();
    }
}