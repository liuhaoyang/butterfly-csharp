using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    public class HttpProfilingCallbackContext : IProfilingCallbackContext
    {
        public IProfilingContext ProfilingContext { get; } = new HttpProfilingContext();

        public string IdentityAuthenticationType { get; set; }

        public string IdentityName { get; set; }

        public string RequestContentType { get; set; }

        public string HttpHost { get; set; }

        public string HttpPort { get; set; }

        public string HttpMethod { get; set; }

        public string HttpPath { get; set; }

        public string HttpProtocol { get; set; }

        public string HttpScheme { get; set; }

        public string ResponseContentType { get; set; }

        public string StatusCode { get; set; }

        public float Elapsed { get; set; }

        public long? RequestContentLength { get; set; }

        public long? ResponseContentLength { get; set; }
    }
}
