using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolPayloadClient : IPayloadClient
    {
        private readonly LineProtocolClient _lineProtocolClient;
        private readonly LineProtocolClientOptions _lineProtocolClientOptions;

        public LineProtocolPayloadClient(LineProtocolClientOptions lineProtocolClientOptions)
        {
            _lineProtocolClientOptions = lineProtocolClientOptions ?? throw new ArgumentNullException(nameof(lineProtocolClientOptions));
            _lineProtocolClient = new InternalLineProtocolClient(
                _lineProtocolClientOptions.ServerAddress, _lineProtocolClientOptions.Database,
                _lineProtocolClientOptions.UserName, _lineProtocolClientOptions.Password);
        }

        public Task WriteAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken))
        {
            var lineProtocolPayload = new LineProtocolPayload();
            foreach (var point in payload)
            {
                lineProtocolPayload.Add(new LineProtocolPoint(point.Name, point.GetFields(), point.GetTags(), point.Timestamp));
            }
            return _lineProtocolClient.WriteAsync(lineProtocolPayload, cancellationToken);
        }

        private class InternalLineProtocolClient : LineProtocolClient
        {
            public InternalLineProtocolClient(string serverBaseAddress, string database, string username = null, string password = null)
                : base(new HttpClientHandler() { UseProxy = false, Proxy = null }, new Uri(serverBaseAddress), database, username, password)
            {
            }
        }
    }
}