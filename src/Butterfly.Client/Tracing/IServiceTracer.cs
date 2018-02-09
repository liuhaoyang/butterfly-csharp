using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public interface IServiceTracer
    {
        ITracer Tracer { get; }
        
        string ServiceName { get; }

        string Environment { get; }

        string Identity { get; }

        ISpan Start(ISpanBuilder spanBuilder);
    }
}