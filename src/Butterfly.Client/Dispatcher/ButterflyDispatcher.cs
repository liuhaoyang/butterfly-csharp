using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.DataContract.Tracing;

namespace Butterfly.Client
{
    public class ButterflyDispatcher : IButterflyDispatcher
    {
        private const int DefaultBoundedCapacity = 1000000;
        private const int DefaultConsumerCount = 2;
        private readonly BlockingCollection<IDispatchable> _limitCollection;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly TimerDispatchHandler _timerDispatchHandler;
        private readonly ICollection<Task> _consumerTask;
        private readonly int _boundedCapacity;
        private readonly int _consumerCount;

        public ButterflyDispatcher(IEnumerable<IDispatchCallback> callbacks, int flushInterval, int boundedCapacity, int consumerCount)
        {
            _consumerCount = consumerCount <= 0 ? DefaultConsumerCount : consumerCount;
            _boundedCapacity = boundedCapacity <= 0 ? DefaultBoundedCapacity : boundedCapacity;
            _limitCollection = new BlockingCollection<IDispatchable>(_boundedCapacity);
            _cancellationTokenSource = new CancellationTokenSource();
            _timerDispatchHandler = new TimerDispatchHandler(callbacks, flushInterval);
            _consumerTask = new List<Task>(_consumerCount);
            InitializationConsumer();
        }

        private void InitializationConsumer()
        {     
            for (var i = 0; i < _consumerCount; i++)
            {
                _consumerTask.Add(CreateConsumer());
            }
        }

        private Task CreateConsumer()
        {
            return Task.Factory.StartNew(
                Consumer, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public bool Dispatch(Span span)
        {
            if (_limitCollection.IsAddingCompleted)
            {
                return false;
            }
             
            _limitCollection.Add(new Dispatchable<Span>(DispatchableToken.SpanToken, span));
            return true;
        }

        private void Consumer()
        {
            foreach (var consumingItem in _limitCollection.GetConsumingEnumerable())
                _timerDispatchHandler.Post(consumingItem);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _limitCollection.CompleteAdding();
        }
    }
}