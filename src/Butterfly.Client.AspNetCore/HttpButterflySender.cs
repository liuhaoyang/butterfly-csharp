using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class HttpButterflySender:HttpButterflySenderBase
    {
        public HttpButterflySender(IOptions<ButterflyOptions> options) : base(options.Value.CollectorUrl)
        {
        }
    }
}