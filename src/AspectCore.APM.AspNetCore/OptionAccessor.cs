using System;
using AspectCore.APM.Common;
using Microsoft.Extensions.Options;

namespace AspectCore.APM.AspNetCore
{
    public class OptionAccessor<TOptions> : IOptionAccessor<TOptions> where TOptions : class, new()
    {
        public TOptions Value { get; }

        public OptionAccessor(IOptions<TOptions> options)
        {
            Value = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
