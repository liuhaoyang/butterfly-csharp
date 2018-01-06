using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public interface IServiceTracer
    {
        ITracer Tracer { get; }
        
        string ServiceName { get; }

        ISpan Start(ISpanBuilder spanBuilder);
    }
}