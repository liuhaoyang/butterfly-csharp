using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    public class HttpProfiledCallbackContext : IProfiledCallbackContext
    {
        public IProfiledContext ProfiledContext { get; } = new HttpProfiledContext();

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

        public long Elapsed { get; set; }
    }
}
