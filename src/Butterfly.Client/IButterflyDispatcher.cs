using System;
using Butterfly.DataContract.Tracing;
using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        event EventHandler<DispatchEventArgs<Span>> OnSpanDispatch;

        bool Dispatch(Span span);
    }
}