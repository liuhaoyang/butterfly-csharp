using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public class SpanDispatchCallback : IDispatchCallback
    {
        private const int DefaultChunked = 200;
        private readonly IButterflySender _butterflySender;
        private readonly Func<DispatchableToken, bool> _filter;
        public SpanDispatchCallback(IButterflySenderProvider senderProvider)
        {
            _butterflySender = senderProvider.GetSender();
            _filter = token => token == DispatchableToken.SpanToken;
        }

        public Func<DispatchableToken, bool> Filter => _filter;

        public async Task Accept(IEnumerable<IDispatchable> dispatchables)
        {
            foreach(var block in dispatchables.Chunked(DefaultChunked))
            {
                try
                {
                    await _butterflySender.SendSpanAsync(block.Select(x => x.RawInstance).OfType<Span>().ToArray());
                }
                catch
                {
                    foreach(var item in block)
                    {
                        item.State = SendState.Untreated;
                        item.Error();
                    }
                    throw;
                }
            }
        }
    }
}
