using System;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public abstract class ButterflyDispatcherBase : IButterflyDispatcher
    {
        private const int BoundedCapacity = 1000000;
        private const int ConsumerCount = 2;
        private readonly BlockingCollection<object> _blockingCollection;
        private readonly Task[] _consumerTasks;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<DispatchEventArgs<Span>> OnSpanDispatch;

        // ReSharper disable once PublicConstructorInAbstractClass
        public ButterflyDispatcherBase(int boundedCapacity, int consumerCount)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _blockingCollection = new BlockingCollection<object>(boundedCapacity <= 0 ? BoundedCapacity : boundedCapacity);
            _consumerTasks = new Task[consumerCount <= 0 ? ConsumerCount : consumerCount];
            for (var i = 0; i < _consumerTasks.Length; i++)
            {
                _consumerTasks[i] = Task.Factory.StartNew(Consumer,
                    _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public bool DispatchInternal(Span span)
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
                        var onSpanDispatch = OnSpanDispatch;
                        onSpanDispatch?.Invoke(this, new DispatchEventArgs<Span>(span));
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