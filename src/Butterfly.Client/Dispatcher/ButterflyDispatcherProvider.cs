using System.Collections.Generic;
using Butterfly.Client.Logging;

namespace Butterfly.Client
{
    public class ButterflyDispatcherProvider : IButterflyDispatcherProvider
    {
        private readonly IEnumerable<IDispatchCallback> _dispatchCallbacks;
        private readonly ButterflyOptions _options;
        private readonly ILoggerFactory _loggerFactory;

        public ButterflyDispatcherProvider(IEnumerable<IDispatchCallback> dispatchCallbacks, ILoggerFactory loggerFactory, ButterflyOptions options)
        {
            _dispatchCallbacks = dispatchCallbacks;
            _loggerFactory = loggerFactory;
            _options = options;
        }

        public IButterflyDispatcher GetDispatcher()
        {
            var dispatcher = new ButterflyDispatcher(_dispatchCallbacks, _loggerFactory, _options.FlushInterval, _options.BoundedCapacity, _options.ConsumerCount);
            dispatcher.Initialization();
            return dispatcher;
        }
    }
}