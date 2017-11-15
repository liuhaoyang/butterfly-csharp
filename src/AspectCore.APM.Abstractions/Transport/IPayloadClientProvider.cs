using AspectCore.DynamicProxy;

namespace AspectCore.APM.Transport
{
    [NonAspect]
    public interface IPayloadClientProvider
    {
        IPayloadClient GetPayloadClient();
    }
}
