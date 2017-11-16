using System;
using System.Net;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfiledCommand
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

        internal static RedisProfiledCommand Create(string command, EndPoint server, int db, DateTime commandCreated,
            TimeSpan creationToEnqueued, TimeSpan enqueuedToSending, TimeSpan sentToResponse, TimeSpan responseToCompletion, TimeSpan elapsed,
            string clientName, long operationCount)
        {
            var redisProfiledCommand = new RedisProfiledCommand();
            redisProfiledCommand.Command = command;
            redisProfiledCommand.Server = server;
            redisProfiledCommand.Db = db;
            redisProfiledCommand.CommandCreated = commandCreated;
            redisProfiledCommand.Elapsed = elapsed;
            redisProfiledCommand.CreationToEnqueued = creationToEnqueued;
            redisProfiledCommand.EnqueuedToSending = enqueuedToSending;
            redisProfiledCommand.SentToResponse = enqueuedToSending;
            redisProfiledCommand.ResponseToCompletion = responseToCompletion;
            redisProfiledCommand.ClientName = clientName;
            redisProfiledCommand.OperationCount = operationCount;
            return redisProfiledCommand;
        }
    }
}