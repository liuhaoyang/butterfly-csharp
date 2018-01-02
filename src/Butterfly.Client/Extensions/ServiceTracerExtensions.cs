using System;
using System.Threading.Tasks;
using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public static class ServiceTracerExtensions
    {
        public static void ChildTrace(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp, Action<ISpan> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            Trace(tracer, CreateChildSpanBuilder(tracer, operationName, startTimestamp), operation);
        }

        public static TResult ChildTrace<TResult>(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ISpan, TResult> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return Trace(tracer, CreateChildSpanBuilder(tracer, operationName, startTimestamp), operation);
        }

        public static void Trace(this IServiceTracer tracer, ISpanBuilder spanBuilder, Action<ISpan> operation)
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
                operation?.Invoke(span);
            }
            catch (Exception exception)
            {
                span.Exception(exception);
                throw;
            }
            finally
            {
                span.Finish(DateTimeOffset.UtcNow);
                tracer.Tracer.SetCurrentSpan(curr);
            }
        }

        public static TResult Trace<TResult>(this IServiceTracer tracer, ISpanBuilder spanBuilder, Func<ISpan, TResult> operation)
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

                if (operation == null)
                {
                    return default(TResult);
                }

                return operation(span);
            }
            catch (Exception exception)
            {
                span.Exception(exception);
                throw;
            }
            finally
            {
                span.Finish(DateTimeOffset.UtcNow);
                tracer.Tracer.SetCurrentSpan(curr);
            }
        }

        public static Task ChildTraceAsync(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ISpan, Task> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return TraceAsync(tracer, CreateChildSpanBuilder(tracer, operationName, startTimestamp), operation);
        }

        public static Task<TResult> ChildTraceAsync<TResult>(this IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp, Func<ISpan, Task<TResult>> operation)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return TraceAsync(tracer, CreateChildSpanBuilder(tracer, operationName, startTimestamp), operation);
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
                span.Exception(exception);
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
                span.Exception(exception);
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
            return tracer.Start(CreateChildSpanBuilder(tracer, operationName, startTimestamp));
        }

        private static ISpanBuilder CreateChildSpanBuilder(IServiceTracer tracer, string operationName, DateTimeOffset? startTimestamp = null)
        {
            var spanBuilder = new SpanBuilder(operationName, startTimestamp);
            var spanContext = tracer.Tracer.GetCurrentSpan()?.SpanContext;
            if (spanContext != null)
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return spanBuilder;
        }
    }
}