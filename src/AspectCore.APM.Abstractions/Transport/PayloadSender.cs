using System;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Collector;

namespace AspectCore.APM.Transport
{
    public sealed class PayloadSender : IPayloadSender
    {
        private readonly IPayloadClient _payloadClient;

        public PayloadSender(IPayloadClientProvider payloadClientProvider)
        {
            _payloadClient = payloadClientProvider?.GetPayloadClient() ?? throw new ArgumentNullException(nameof(payloadClientProvider));
        }

        public Task SendAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _payloadClient.WriteAsync(payload, cancellationToken);
        }
    }
}