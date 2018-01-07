using System;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;
using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        bool Dispatch(Span span);
    }
}