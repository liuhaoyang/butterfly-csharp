using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspectCore.APM.Collector
{
    public sealed class PayloadSender : IPayloadSender
    {
        private readonly IEnumerable<IPayloadClientProvider> _payloadClientProviders;

        public PayloadSender(IEnumerable<IPayloadClientProvider> payloadClientProviders)
        {
            _payloadClientProviders = payloadClientProviders ?? throw new ArgumentNullException(nameof(payloadClientProviders));
        }

        public async Task SendAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var provider in _payloadClientProviders)
                await provider.GetPayloadClient().WriteAsync(payload, cancellationToken);
        }
    }
}