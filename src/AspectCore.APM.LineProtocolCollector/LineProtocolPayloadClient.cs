using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolPayloadClient : IPayloadClient, IDisposable
    {
        const int _defaultInterval = 10;
        const int _defaultBlockCapacity = 1000;

        private readonly ConcurrentDictionary<PointState, object> _pointMap;
        private readonly LineProtocolClient _lineProtocolClient;
        private readonly LineProtocolClientOptions _lineProtocolClientOptions;
        private readonly IInternalLogger _logger;
        private readonly Timer _flushTimer;
        private readonly int _blockCapacity;

        public LineProtocolPayloadClient(LineProtocolClientOptions lineProtocolClientOptions, IInternalLogger logger = null)
        {
            _logger = logger;
            _lineProtocolClientOptions = lineProtocolClientOptions ?? throw new ArgumentNullException(nameof(lineProtocolClientOptions));
            _lineProtocolClient = new InternalLineProtocolClient(
                _lineProtocolClientOptions.Server, _lineProtocolClientOptions.Database,
                _lineProtocolClientOptions.UserName, _lineProtocolClientOptions.Password);
            var interval = lineProtocolClientOptions.Interval.HasValue && lineProtocolClientOptions.Interval.Value > 0 ? lineProtocolClientOptions.Interval.Value : _defaultInterval;
            _blockCapacity = lineProtocolClientOptions.BlockCapacity.GetValueOrDefault(_defaultBlockCapacity);
            _pointMap = new ConcurrentDictionary<PointState, object>();
            _flushTimer = new Timer(async state => await FlushCallback(state), null, TimeSpan.FromSeconds(interval), TimeSpan.FromSeconds(interval));
            _logger?.LogInformation("Start LineProtocolCollector.");
        }

        public Task WriteAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var point in payload)
            {
                var lineProtocolPoint = new LineProtocolPoint(point.Name, point.GetFields(), point.GetTags(), point.Timestamp);
                _pointMap.TryAdd(new PointState(lineProtocolPoint), null);
            }
            return Task.FromResult(0);
        }

        private class InternalLineProtocolClient : LineProtocolClient
        {
            public InternalLineProtocolClient(string serverBaseAddress, string database, string username = null, string password = null)
                : base(new HttpClientHandler() { UseProxy = false, Proxy = null }, new Uri(serverBaseAddress), database, username, password)
            {
            }
        }

        private async Task FlushCallback(object obj)
        {
            var utcNow = DateTime.UtcNow;
            var oldPointStates = _pointMap.Keys.Where(x => x.UtcTimeStamp < utcNow && x.Status == PointStatus.Untreated).ToList();
            oldPointStates.ForEach(state => state.Status = PointStatus.Sending);
            foreach (var currentStates in Chunked(oldPointStates))
            {
                var states = currentStates.ToList();
                var lineProtocolPayload = new LineProtocolPayload();

                foreach (var state in states.OrderByDescending(x => x.UtcTimeStamp))
                    lineProtocolPayload.Add(state.LineProtocolPoint);

                try
                {
                    await _lineProtocolClient.WriteAsync(lineProtocolPayload);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex.Message, ex);
                }
                finally
                {
                    foreach (var state in states)
                        _pointMap.TryRemove(state, out _);
                }
            }
        }

        public void Dispose()
        {
            _flushTimer.Dispose();
            _logger?.LogInformation("Stop LineProtocolCollector.");
        }

        private IEnumerable<IEnumerable<PointState>> Chunked(IEnumerable<PointState> source)
        {
            while (source.Any())
            {
                yield return source.Take(_blockCapacity);
                source = source.Skip(_blockCapacity);
            }
        }
    }
}