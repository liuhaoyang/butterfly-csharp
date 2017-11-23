using AspectCore.DynamicProxy;

namespace AspectCore.APM.Core
{
    [NonAspect]
    public interface IComponentLifetime
    {
        bool Started { get; }

        bool Start();

        void Stop();
    }
}