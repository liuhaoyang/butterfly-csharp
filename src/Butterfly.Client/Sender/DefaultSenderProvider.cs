using System;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.Client.Sender
{
    internal class DefaultSenderProvider : IButterflySenderProvider
    {
        private readonly ButterflyOptions _options;

        public DefaultSenderProvider(ButterflyOptions options)
        {
            _options = options;
        }

        public IButterflySender GetSender()
        {
            return new HttpButterflySender(_options.CollectorUrl);
        }
    }
}