using System;
using AspectCore.APM.Common;

namespace AspectCore.APM.Collector
{
    public class AsyncCollertor : ICollector
    {
        private readonly IPayloadDispatcher _payloadDispatcher;

        public AsyncCollertor(IPayloadDispatcher payloadDispatcher)
        {
            _payloadDispatcher = payloadDispatcher ?? throw new ArgumentNullException(nameof(payloadDispatcher));
        }

        public bool Push(IPayload payload)
        {
            return _payloadDispatcher.Dispatch(payload);
        }
    }
}