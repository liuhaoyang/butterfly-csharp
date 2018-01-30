using System;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        bool Dispatch(Span span);
    }
}