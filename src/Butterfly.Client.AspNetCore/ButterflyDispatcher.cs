using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyDispatcher : IButterflyDispatcher
    {
        private const int BoundedCapacity = 500000;
        private readonly BlockingCollection<object> _blockingCollection;
        private readonly Task _consumerTask;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IButterflySender _sender;

        public ButterflyDispatcher(IButterflySender sender, IOptions<ButterflyOptions> options)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _cancellationTokenSource = new CancellationTokenSource();
            _blockingCollection = new BlockingCollection<object>(options.Value.BoundedCapacity <= 0 ? BoundedCapacity : options.Value.BoundedCapacity);
            _consumerTask = Task.Factory.StartNew(Consumer,
                _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public bool Dispatch(Span span)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return DispatchInternal(span);
        }

        private bool DispatchInternal(object obj)
        {
            if (_blockingCollection.IsAddingCompleted)
            {
                return false;
            }

            _blockingCollection.Add(obj);
            return true;
        }

        private void Consumer()
        {
            foreach (var consumingItem in _blockingCollection.GetConsumingEnumerable())
            {
                switch (consumingItem)
                {
                    case Span span:
                        _sender.SendSpanAsync(new[] {span}, _cancellationTokenSource.Token);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}