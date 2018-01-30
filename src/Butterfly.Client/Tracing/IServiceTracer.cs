using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public interface IServiceTracer
    {
        ITracer Tracer { get; }
        
        string ServiceName { get; }

        string EnvironmentName { get; }

        ISpan Start(ISpanBuilder spanBuilder);
    }
}