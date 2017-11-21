using System;
using System.Threading;
using AspectCore.APM.Collector;
using AspectCore.APM.Core;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolPayloadClientProvider : IPayloadClientProvider
    {
        private readonly Lazy<IPayloadClient> _payloadClient;

        public LineProtocolPayloadClientProvider(IOptionAccessor<LineProtocolClientOptions> optionAccessor, IInternalLogger logger = null)
        {
            if (optionAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionAccessor));
            }
            _payloadClient = new Lazy<IPayloadClient>(() => new LineProtocolPayloadClient(optionAccessor.Value, logger), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IPayloadClient GetPayloadClient()
        {
            return _payloadClient.Value;
        }
    }
}