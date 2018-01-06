namespace Butterfly.Client.AspNetCore
{
    public interface ITracingDiagnosticListener
    {
        string ListenerName { get; }
    }
}