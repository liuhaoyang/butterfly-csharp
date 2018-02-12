using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyHostedService : IHostedService
    {
        private readonly IButterflyDispatcher _dispatcher;

        public ButterflyHostedService(IButterflyDispatcher dispatcher, IEnumerable<ITracingDiagnosticListener> tracingDiagnosticListeners, DiagnosticListener diagnosticListener)
        {
            _dispatcher = dispatcher;
            foreach (var tracingDiagnosticListener in tracingDiagnosticListeners)
            {
                diagnosticListener.SubscribeWithAdapter(tracingDiagnosticListener);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _dispatcher.InitializationAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _dispatcher.Dispose();
            return Task.CompletedTask;
        }
    }
}