using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyDispatcherProvider : IButterflyDispatcherProvider
    {
        private readonly IEnumerable<IDispatchCallback> _dispatchCallbacks;
        private readonly ButterflyOptions _options;

        public ButterflyDispatcherProvider(IEnumerable<IDispatchCallback> dispatchCallbacks, IOptions<ButterflyOptions> options)
        {
            _dispatchCallbacks = dispatchCallbacks;
            _options = options.Value;
        }

        public IButterflyDispatcher GetDispatcher()
        {
            return new ButterflyDispatcher(_dispatchCallbacks, _options.FlushInterval, _options.BoundedCapacity, _options.ConsumerCount);
        }
    }
}