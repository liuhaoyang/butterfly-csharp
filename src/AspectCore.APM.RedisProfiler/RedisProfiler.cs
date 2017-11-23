using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public class RedisProfiler : IProfiler<RedisProfilingContext>
    {
        private readonly ICollector _collector;
        private readonly IGlobalTagReader _tagReader;

        public RedisProfiler(ICollector collector, IGlobalTagReader tagReader)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _tagReader = tagReader ?? throw new ArgumentNullException(nameof(tagReader));
        }

        public Task Invoke(RedisProfilingContext profilingContext)
        {
            var points = new List<Point>();
            foreach (var command in profilingContext.ProfilingCommands)
            {
                var redisProfilingFields = new FieldCollection();
                var redisProfilingTags = new TagCollection();
                _tagReader.Read(redisProfilingTags);
                redisProfilingFields[RedisProfilingConstants.Elapsed] = command.Elapsed.Milliseconds;
                redisProfilingFields[RedisProfilingConstants.OperationCount] = command.OperationCount.ToString();
                redisProfilingTags[RedisProfilingConstants.ClientName] = command.ClientName;
                redisProfilingTags[RedisProfilingConstants.Command] = command.Command;
                redisProfilingTags[RedisProfilingConstants.Db] = command.Db.ToString();
                redisProfilingTags[RedisProfilingConstants.Server] = command.Server.ToString();
                points.Add(new Point(profilingContext.ProfilerName, redisProfilingFields, redisProfilingTags));
            }
            return Task.FromResult(_collector.Push(new Payload(points)));
        }
    }
}