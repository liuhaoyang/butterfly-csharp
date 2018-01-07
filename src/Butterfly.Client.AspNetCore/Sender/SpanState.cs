namespace Butterfly.Client.AspNetCore
{
    public class SpanState
    {
        private int _counter = 0;

        public SpanSendState State { get; set; } = SpanSendState.Untreated;

        public int ErrorCount
        {
            get { return _counter; }
        }

        public int Error()
        {
            return System.Threading.Interlocked.Increment(ref _counter);
        }
    }

    public enum SpanSendState
    {
        
        Untreated,
        Sending,
        Sended
    }
}