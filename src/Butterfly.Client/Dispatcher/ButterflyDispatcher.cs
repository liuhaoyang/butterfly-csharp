using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public class ButterflyDispatcher : IButterflyDispatcher
    {
        private readonly BlockingCollection<object> _limitCollection;

        public bool Dispatch(Span span)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}