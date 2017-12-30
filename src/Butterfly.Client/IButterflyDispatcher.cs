using System;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        event EventHandler<DispatchEventArgs<Span>> OnSpanDispatch;

        bool DispatchInternal(Span span);
    }
}