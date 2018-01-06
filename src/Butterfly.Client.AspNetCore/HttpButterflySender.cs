using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class QueueHttpButterflySender: HttpButterflySender
    {
        public QueueHttpButterflySender(IOptions<ButterflyOptions> options) : base(options.Value.CollectorUrl)
        {
        }
    }
}