using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Transport
{
    [NonAspect]
    public interface IPayloadClient
    {
        Task WriteAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken));
    }
}