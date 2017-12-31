using System;
using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        event EventHandler<DispatchEventArgs<ISpan>> OnSpanDispatch;

        bool Dispatch(ISpan span);
    }
}