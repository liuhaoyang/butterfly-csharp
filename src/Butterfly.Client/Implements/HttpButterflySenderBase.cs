using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;
using Newtonsoft.Json;

namespace Butterfly.Client
{
    public abstract class HttpButterflySenderBase : IButterflySender
    {
        protected const string spanUrl = "/api/span";

        private readonly HttpClient _httpClient;

        // ReSharper disable once PublicConstructorInAbstractClass
        public HttpButterflySenderBase(string collectorUrl)
            : this(new HttpClient(), collectorUrl)
        {
        }

        // ReSharper disable once PublicConstructorInAbstractClass
        public HttpButterflySenderBase(HttpClient httpClient, string collectorUrl)
        {
            if (collectorUrl == null)
            {
                throw new ArgumentNullException(nameof(collectorUrl));
            }

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(collectorUrl);
        }

        public virtual Task SendSpanAsync(Span[] spans, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (spans == null)
            {
                throw new ArgumentNullException(nameof(spans));
            }

            var content = new StringContent(JsonConvert.SerializeObject(spans), Encoding.UTF8, "application/json");
            return _httpClient.PostAsync(spanUrl, content, cancellationToken);
        }
    }
}