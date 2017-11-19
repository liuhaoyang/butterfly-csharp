using System;
using AspectCore.APM.Common;

namespace AspectCore.APM.Collector
{
    public class AsyncCollertor : ICollector
    {
        private readonly IInternalLogger _logger;
        private readonly IPayloadDispatcher _payloadDispatcher;

        public AsyncCollertor(IPayloadDispatcher payloadDispatcher, IInternalLogger logger = null)
        {
            _payloadDispatcher = payloadDispatcher ?? throw new ArgumentNullException(nameof(payloadDispatcher));
            _logger = logger;
        }

        public bool Push(IPayload payload)
        {
            if (!_payloadDispatcher.Dispatch(payload))
            {
                _logger?.LogWarning("Couldn't dispatch payload, actor may be blocked by another operation.");
                return false;
            }
            return true;
        }
    }
}