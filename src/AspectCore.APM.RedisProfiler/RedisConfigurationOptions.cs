using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using AspectCore.APM.Common;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public class RedisConfigurationOptions : IOptionAccessor<RedisConfigurationOptions>
    {
        public RedisConfigurationOptions Value => this;

        public List<string> Servers { get; set; }

        public string ClientName { get; set; }

        public string ServiceName { get; set; }

        public int? keepAlive { get; set; }

        public int? SyncTimeout { get; set; }

        public bool? AllowAdmin { get; set; }

        public int? ConnectTimeout { get; set; }

        public string Password { get; set; }

        public string TieBreaker { get; set; }

        public bool? Ssl { get; set; }

        public string SslHost { get; set; }

        public bool? HighPrioritySocketThreads { get; set; }

        public string ConfigChannel { get; set; }

        public bool? AbortConnect { get; set; }

        public int? ConnectRetry { get; set; }

        public int? ResponseTimeout { get; set; }

        public int? KeepAlive { get; set; }

        public int? ConfigCheckSeconds { get; set; }

        public int? DefaultDatabase { get; set; }

        public int? WriteBuffer { get; set; }

        public string Proxy { get; set; }

        public bool? ResolveDns { get; set; }

        public SslProtocols SslProtocols { get; set; }

        public string ChannelPrefix { get; set; }

        public IReconnectRetryPolicy ReconnectRetryPolicy { get; set; }

        public CommandMap CommandMap { get; set; }

        public RedisConfigurationOptions()
        {
            Servers = new List<string>();
        }

        internal ConfigurationOptions Copy()
        {
            ConfigurationOptions configurationOptions = new ConfigurationOptions()
            {
                ClientName = ClientName,
                ServiceName = ServiceName,
                KeepAlive = KeepAlive.GetValueOrDefault(-1),
                SyncTimeout = SyncTimeout.GetValueOrDefault(1000),
                AllowAdmin = AllowAdmin.GetValueOrDefault(),
                Password = Password,
                TieBreaker = TieBreaker ?? "__Booksleeve_TieBreak",
                WriteBuffer = WriteBuffer.GetValueOrDefault(4096),
                Ssl = Ssl.GetValueOrDefault(),
                SslHost = SslHost,
                HighPrioritySocketThreads = HighPrioritySocketThreads ?? true,
                ConfigurationChannel = ConfigChannel,
                ResolveDns = ResolveDns.GetValueOrDefault(),
                CommandMap = CommandMap,
                ChannelPrefix = ChannelPrefix,
                ConnectRetry = ConnectRetry??3,
                ConfigCheckSeconds = ConfigCheckSeconds.GetValueOrDefault(60),
                DefaultDatabase = DefaultDatabase,
                ReconnectRetryPolicy = ReconnectRetryPolicy,
                SslProtocols = this.SslProtocols
            };

            foreach (var server in Servers)
                configurationOptions.EndPoints.Add(server);

            configurationOptions.ConnectTimeout = ConnectTimeout ?? Math.Max(5000, configurationOptions.SyncTimeout);
            configurationOptions.ResponseTimeout = ResponseTimeout ?? configurationOptions.SyncTimeout;
            configurationOptions.AbortOnConnectFail = AbortConnect ?? GetDefaultAbortOnConnectFailSetting(configurationOptions.EndPoints);

            if (Enum.TryParse<Proxy>(Proxy, true, out var proxy))
                configurationOptions.Proxy = proxy;

            return configurationOptions;
        }

        internal static bool GetDefaultAbortOnConnectFailSetting(EndPointCollection endPoints)
        {
            return !IsAzureEndpoint(endPoints);
        }

        internal static bool IsAzureEndpoint(EndPointCollection endPoints)
        {
            bool flag = false;
            foreach (DnsEndPoint dnsEndPoint in endPoints.Select(endpoint => endpoint as DnsEndPoint).Where(ep => ep != null))
            {
                int startIndex = dnsEndPoint.Host.IndexOf('.');
                if (startIndex >= 0)
                {
                    string lowerInvariant = dnsEndPoint.Host.Substring(startIndex).ToLowerInvariant();
                    if (lowerInvariant == ".redis.cache.windows.net" || lowerInvariant == ".redis.cache.chinacloudapi.cn" || (lowerInvariant == ".redis.cache.usgovcloudapi.net" || lowerInvariant == ".redis.cache.cloudapi.de"))
                        return true;
                }
            }
            return flag;
        }
    }
}