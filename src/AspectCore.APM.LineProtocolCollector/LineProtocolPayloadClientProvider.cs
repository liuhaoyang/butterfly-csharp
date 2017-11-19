using System;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolPayloadClientProvider : IPayloadClientProvider
    {
        private readonly IPayloadClient _payloadClient;

        public LineProtocolPayloadClientProvider(IOptionAccessor<LineProtocolClientOptions> optionAccessor, IInternalLogger logger = null)
        {
            if (optionAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionAccessor));
            }
            _payloadClient = new LineProtocolPayloadClient(optionAccessor.Value, logger);
        }

        public IPayloadClient GetPayloadClient()
        {
            return _payloadClient;
        }
    }
}
