using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Butterfly.OpenTracing;
using Butterfly.OpenTracing.Extensions;

namespace Butterfly.Client
{
    public static class TracerExtensions
    {
        public static void Inject<T>(this ITracer tracer, ISpanContext spanContext, T carrier, Action<T, string, string> injector)
            where T : class, IEnumerable
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }
            
            tracer.Inject(spanContext, new TextMapCarrierWriter(), new DelegatingCarrier<T>(carrier, injector));
        }

        public static bool TryExtract<T>(this ITracer tracer, out ISpanContext spanContext, T carrier, Func<T, string, string> extractor, Func<T, IEnumerator<KeyValuePair<string, string>>> enumerator = null)
            where T : class, IEnumerable
        {
            spanContext = tracer.Extract(new TextMapCarrierReader(new SpanContextFactory()), new DelegatingCarrier<T>(carrier, extractor, enumerator));
            return spanContext != null;
        }

        public static Task ChildTraceAsync(this ITracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ITracer, ISpan, Task> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var spanBuilder = new SpanBuilder(operationName, startTimestamp);
            var spanContext = tracer.GetCurrentSpan()?.SpanContext;
            if (spanContext != null)
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return TraceAsync(tracer, spanBuilder, operation);
        }

        public static Task<TResult> ChildTraceAsync<TResult>(this ITracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ITracer, ISpan, Task<TResult>> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var spanBuilder = new SpanBuilder(operationName, startTimestamp);
            var spanContext = tracer.GetCurrentSpan()?.SpanContext;
            if (spanContext != null)
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return TraceAsync(tracer, spanBuilder, operation);
        }

        public static async Task TraceAsync(this ITracer tracer, ISpanBuilder spanBuilder, Func<ITracer, ISpan, Task> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            using (var span = tracer.Start(spanBuilder))
            {
                try
                {
                    await operation?.Invoke(tracer, span);
                }
                catch (Exception exception)
                {
                    span.Tags.Error(true);
                    span.Log(LogField.CreateNew().EventError().ErrorKind(exception.GetType().Name).ErrorObject(exception));
                    throw;
                }
            }
        }

        public static async Task<TResult> TraceAsync<TResult>(this ITracer tracer, ISpanBuilder spanBuilder, Func<ITracer, ISpan, Task<TResult>> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            using (var span = tracer.Start(spanBuilder))
            {
                try
                {
                    return await operation?.Invoke(tracer, span);
                }
                catch (Exception exception)
                {
                    span.Tags.Error(true);
                    span.Log(LogField.CreateNew().EventError().ErrorKind(exception.GetType().Name).ErrorObject(exception));
                    throw;
                }
            }
        }
    }
}