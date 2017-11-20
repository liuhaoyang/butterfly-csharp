using System.Threading;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IPayloadSender
    {
        Task SendAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken));
    }
}