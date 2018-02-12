using System;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public interface IButterflyDispatcher : IDisposable
    {
        Task InitializationAsync();

        void Initialization();

        bool Dispatch(Span span);
    }
}