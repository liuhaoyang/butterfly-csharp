using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Butterfly.Client.AspNetCore
{
    public class TracingHostedService : IHostedService
    {
        private readonly IButterflyDispatcher _dispatcher;

        public TracingHostedService(IButterflyDispatcher dispatcher, IEnumerable<ITracingDiagnosticListener> tracingDiagnosticListeners, DiagnosticListener diagnosticListener)
        {
            _dispatcher = dispatcher;
            foreach (var tracingDiagnosticListener in tracingDiagnosticListeners)
            {
                diagnosticListener.SubscribeWithAdapter(tracingDiagnosticListener);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _dispatcher.Dispose();
            return Task.CompletedTask;
        }
    }
}