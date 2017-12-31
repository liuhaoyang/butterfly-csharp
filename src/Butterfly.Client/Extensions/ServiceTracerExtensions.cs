using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Butterfly.OpenTracing;
using Butterfly.OpenTracing.Extensions;

namespace Butterfly.Client
{
    public static class ServiceTracerExtensions
    {
        public static Task ChildTraceAsync(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ISpan, Task> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var spanBuilder = new SpanBuilder(operationName, startTimestamp);
            var spanContext = tracer.Tracer.GetCurrentSpan()?.SpanContext;
            if (spanContext != null)
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return TraceAsync(tracer, spanBuilder, operation);
        }

        public static Task<TResult> ChildTraceAsync<TResult>(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ISpan, Task<TResult>> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var spanBuilder = new SpanBuilder(operationName, startTimestamp);
            var spanContext = tracer.Tracer.GetCurrentSpan()?.SpanContext;
            if (spanContext != null)
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return TraceAsync(tracer, spanBuilder, operation);
        }

        public static async Task TraceAsync(this IServiceTracer tracer, ISpanBuilder spanBuilder, Func<ISpan, Task> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var span = tracer.Start(spanBuilder);

            var curr = tracer.Tracer.GetCurrentSpan();
            try
            {
                tracer.Tracer.SetCurrentSpan(span);
                await operation?.Invoke(span);
            }
            catch (Exception exception)
            {
                span.Tags.Error(true);
                span.Log(LogField.CreateNew().EventError().ErrorKind(exception.GetType().Name).ErrorObject(exception));
                throw;
            }
            finally
            {
                span.Finish(DateTimeOffset.UtcNow);
                tracer.Tracer.SetCurrentSpan(curr);
            }
        }

        public static async Task<TResult> TraceAsync<TResult>(this IServiceTracer tracer, ISpanBuilder spanBuilder, Func<ISpan, Task<TResult>> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var span = tracer.Start(spanBuilder);

            var curr = tracer.Tracer.GetCurrentSpan();
            try
            {
                tracer.Tracer.SetCurrentSpan(span);
                return await operation?.Invoke(span);
            }
            catch (Exception exception)
            {
                span.Tags.Error(true);
                span.Log(LogField.CreateNew().EventError().ErrorKind(exception.GetType().Name).ErrorObject(exception));
                throw;
            }
            finally
            {
                span.Finish(DateTimeOffset.UtcNow);
                tracer.Tracer.SetCurrentSpan(curr);
            }   
        }

        public static ISpan StartChild(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp = null)
        {
            var spanBuilder = new SpanBuilder(operationName, startTimestamp);
            var spanContext = tracer.Tracer.GetCurrentSpan()?.SpanContext;
            if (spanContext != null)
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return tracer.Start(spanBuilder);
        }
    }
}