using System.Linq;
using Butterfly.DataContract.Tracing;
using Butterfly.OpenTracing;
using BaggageContract = Butterfly.DataContract.Tracing.Baggage;
using LogFieldContract = Butterfly.DataContract.Tracing.LogField;
using SpanReferenceContract = Butterfly.DataContract.Tracing.SpanReference;

namespace Butterfly.Client.Tracing
{
    public static class SpanContractUtils
    {
        public static Span CreateFromSpan(ISpan span)
        {
            var spanContract = new Span
            {
                FinishTimestamp = span.FinishTimestamp,
                StartTimestamp = span.StartTimestamp,
                Sampled = span.SpanContext.Sampled,
                SpanId = span.SpanContext.SpanId,
                TraceId = span.SpanContext.TraceId,
                OperationName = span.OperationName,
                Duration = (span.FinishTimestamp - span.StartTimestamp).GetMicroseconds()
            };

            spanContract.Baggages = span.SpanContext.Baggage?.Select(x => new BaggageContract { Key = x.Key, Value = x.Value }).ToList();
            spanContract.Logs = span.Logs?.Select(x =>
                new Log
                {
                    Timestamp = x.Timestamp,
                    Fields = x.Fields.Select(f => new LogFieldContract { Key = f.Key, Value = f.Value?.ToString() }).ToList()
                }).ToList();

            spanContract.Tags = span.Tags?.Select(x => new Tag { Key = x.Key, Value = x.Value }).ToList();

            spanContract.References = span.SpanContext.References?.Select(x =>
                new SpanReferenceContract { ParentId = x.SpanContext.SpanId, Reference = x.SpanReferenceOptions.ToString() }).ToList();

            return spanContract;
        }
    }
}