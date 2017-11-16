using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.HttpProfiler
{
    public static class HttpProfiledConstants
    {
        public const string IdentityAuthenticationType = "identity_authentication_type";

        public const string IdentityName = "identity_name";

        public const string ConnectionLocalIpAddress = "connection_local_ip_address";

        public const string ConnectionLocalPort = "connection_local_port";

        public const string ConnectionRemoteIpAddress = "connection_remote_ip_address";

        public const string ConnectionRemotePort = "connection_remote_port";

        public const string RequestContentType = "request_content_type";

        public const string HttpHost = "http_host";

        public const string HttpPort = "http_port";

        public const string HttpMethod = "http_method";

        public const string HttpPath = "Http_path";

        public const string HttpProtocol = "http_protocol";

        public const string HttpScheme = "http_scheme";

        public const string ResponseContentType = "response_content_type";

        public const string StatusCode = "http_status_code";

        public const string Elapsed = "elapsed";

        public const string UserAgent = "user_agent";
    }
}