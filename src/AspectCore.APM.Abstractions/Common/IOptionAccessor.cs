using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.Common
{
    public interface IOptionAccessor<TOptions> where TOptions : class, new()
    {
        TOptions Value { get; }
    }
}