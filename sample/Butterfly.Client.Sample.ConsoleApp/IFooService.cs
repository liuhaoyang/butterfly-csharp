using Butterfly.Client.Tracing;

namespace Butterfly.Client.Sample.ConsoleApp
{
    public interface IFooService
    {
        [Trace]
        string GetValues();
    }
}