using System.Diagnostics;
using System.Net;
using System.Reflection;
using Butterfly.Client.Logging;
using Butterfly.Client.Sender;
using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public class TracerManager
    {
        public static TracerManager Instence { get; private set; }

        public IServiceTracer ServiceTracer { get; }

        public static void Init(ButterflyOptions options, ILoggerFactory loggerFactory)
        {
            if (Instence == null)
                Instence = new TracerManager(options, loggerFactory);
        }

        private TracerManager(ButterflyOptions options, ILoggerFactory loggerFactory)
        {
            var senderProvider = new DefaultSenderProvider(options);

            var callback = new SpanDispatchCallback(senderProvider, loggerFactory);
            var dispatcher = new ButterflyDispatcher(new IDispatchCallback[] { callback }, loggerFactory, 0, 0, 0);

            var spanRecorder = new AsyncSpanRecorder(dispatcher);
            var tracer = new Tracer(spanRecorder);

#if NETSTANDARD1_6
            var environmentName = ((dynamic)Assembly.GetEntryAssembly().GetCustomAttribute<DebuggableAttribute>()).IsJITTrackingEnabled is bool debugMode ? debugMode ? "Development" : "Production" : "Unknown";
#else
            var environmentName = Assembly.GetEntryAssembly().GetCustomAttribute<DebuggableAttribute>().IsJITTrackingEnabled ? "Development" : "Production";
#endif
            var service = options.Service ?? Assembly.GetEntryAssembly().GetName().Name;
            var host = Dns.GetHostName();
            var identity = string.IsNullOrEmpty(options.ServiceIdentity) ? $"{service}@{host}" : options.ServiceIdentity;

            ServiceTracer = new ServiceTracer(tracer, service, environmentName, identity, host);
        }
    }
}