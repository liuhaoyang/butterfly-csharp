using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Butterfly.DataContract.Tracing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class QueueHttpButterflySender : HttpButterflySender,IDisposable
    {
        const int _defaultInterval = 15;
        const int _defaultBlockCapacity = 100;

        private readonly ConcurrentDictionary<Span, SpanState> _queue;
        private readonly Timer _timer;
        private readonly ILogger<QueueHttpButterflySender> _logger;

        public QueueHttpButterflySender(IOptions<ButterflyOptions> options, ILogger<QueueHttpButterflySender> logger) : base(options.Value.CollectorUrl)
        {
            _logger = logger;
            _queue = new ConcurrentDictionary<Span, SpanState>();
            _timer = new Timer(async s => await FlushCallback(DateTimeOffset.UtcNow), null, TimeSpan.FromSeconds(_defaultInterval), TimeSpan.FromSeconds(_defaultInterval));
        }

        public override Task SendSpanAsync(Span[] spans, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (spans == null)
            {
                throw new ArgumentNullException(nameof(spans));
            }

            foreach (var span in spans)
                _queue.TryAdd(span, new SpanState());

            return Task.FromResult(0);
        }

        private async Task FlushCallback(DateTimeOffset utcNow)
        {
            var oldSpans = _queue.Where(x => x.Key.FinishTimestamp < utcNow && x.Value.State == SpanSendState.Untreated).ToList();

            oldSpans.ForEach(item => _queue[item.Key].State = SpanSendState.Sending);

            foreach (var currentSpans in Chunked(oldSpans))
            {
                try
                {
                    await base.SendSpanAsync(currentSpans.Select(x => x.Key).ToArray());
                    foreach (var item in currentSpans)
                    {
                        item.Value.State = SpanSendState.Sended;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Sender spans error.");
                    foreach (var item in currentSpans)
                    {
                        item.Value.Error();
                        item.Value.State = SpanSendState.Untreated;
                    }
                }
            }

            foreach (var span in oldSpans)
            {
                if (span.Value.State == SpanSendState.Sended || span.Value.ErrorCount >= 3)
                    _queue.TryRemove(span.Key, out _);
            }
        }

        private static IEnumerable<IEnumerable<T>> Chunked<T>(IEnumerable<T> source)
        {
            while (source.Any())
            {
                yield return source.Take(_defaultBlockCapacity);
                source = source.Skip(_defaultBlockCapacity);
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}