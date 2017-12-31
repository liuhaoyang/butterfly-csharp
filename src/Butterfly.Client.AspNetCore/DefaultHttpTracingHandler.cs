using System.Net.Http;
using Butterfly.OpenTracing;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class DefaultHttpTracingHandler : HttpTracingHandler
    {
        public DefaultHttpTracingHandler(ITracer tracer, IOptions<ButterflyOptions> options) 
            : base(tracer, options.Value.Service, null)
        {
        }
    }
}