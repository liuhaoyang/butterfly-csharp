using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Core
{
    [NonAspect]
    public interface IOptionAccessor<TOptions> where TOptions : class, new()
    {
        TOptions Value { get; }
    }
}