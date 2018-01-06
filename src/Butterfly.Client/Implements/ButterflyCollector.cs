using System;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;
using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public class ButterflyCollector : IButterflyCollector
    {
        private readonly IButterflyDispatcher _dispatcher;
        private readonly IButterflySender _sender;

        public ButterflyCollector(IButterflyDispatcher butterflyDispatcher, IButterflySender butterflySender)
        {
            _dispatcher = butterflyDispatcher ?? throw new ArgumentNullException(nameof(butterflyDispatcher));
            _sender = butterflySender ?? throw new ArgumentNullException(nameof(butterflySender));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _dispatcher.OnSpanDispatch += DispatcherOnOnSpanDispatch;
            return Task.FromResult(0);
        }

        private void DispatcherOnOnSpanDispatch(object sender, DispatchEventArgs<Span> dispatchEventArgs)
        {
            _sender.SendSpanAsync(new Span[] {dispatchEventArgs.Data});
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _dispatcher.Dispose();
            _dispatcher.OnSpanDispatch -= DispatcherOnOnSpanDispatch;
            return Task.FromResult(0);
        }
    }
}