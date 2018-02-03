using System;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        Task Initialization();
        bool Dispatch(Span span);
    }
}