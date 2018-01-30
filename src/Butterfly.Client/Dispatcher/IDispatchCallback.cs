using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Butterfly.Client
{
    public interface IDispatchCallback
    {
        Func<DispatchableToken, bool> Filter { get; }

        Task Accept(IEnumerable<IDispatchable> dispatchables);
    }
}
