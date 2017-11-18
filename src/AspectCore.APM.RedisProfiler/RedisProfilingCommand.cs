using System;
using System.Net;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfilingCommand
    {
        public EndPoint Server { get; private set; }

        public int Db { get; private set; }

        public string Command { get; private set; }

        public DateTime CommandCreated { get; private set; }

        public TimeSpan Elapsed { get; private set; }

        public TimeSpan CreationToEnqueued { get; private set; }

        public TimeSpan EnqueuedToSending { get; private set; }

        public TimeSpan SentToResponse { get; private set; }

        public TimeSpan ResponseToCompletion { get; private set; }

        public long OperationCount { get; private set; }

        public string ClientName { get; private set; }

        public override string ToString()
        {
            return $"server-{Server}  db-{Db}  command-{Command}  elapsed-{Elapsed}";
        }

        internal static RedisProfilingCommand Create(string command, EndPoint server, int db, DateTime commandCreated,
            TimeSpan creationToEnqueued, TimeSpan enqueuedToSending, TimeSpan sentToResponse, TimeSpan responseToCompletion, TimeSpan elapsed,
            string clientName, long operationCount)
        {
            var redisProfilingCommand = new RedisProfilingCommand();
            redisProfilingCommand.Command = command;
            redisProfilingCommand.Server = server;
            redisProfilingCommand.Db = db;
            redisProfilingCommand.CommandCreated = commandCreated;
            redisProfilingCommand.Elapsed = elapsed;
            redisProfilingCommand.CreationToEnqueued = creationToEnqueued;
            redisProfilingCommand.EnqueuedToSending = enqueuedToSending;
            redisProfilingCommand.SentToResponse = enqueuedToSending;
            redisProfilingCommand.ResponseToCompletion = responseToCompletion;
            redisProfilingCommand.ClientName = clientName;
            redisProfilingCommand.OperationCount = operationCount;
            return redisProfilingCommand;
        }
    }
}