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
        private readonly ApplicationOptions _apmOptions;

        public RedisProfiler(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _apmOptions = optionAccessor.Value;
        }

        public Task Invoke(RedisProfilingContext profilingContext)
        {
            var points = new List<Point>();
            foreach(var command in profilingContext.ProfilingCommands)
            {
                var redisProfilingFields = new FieldCollection();
                var redisProfilingTags = new TagCollection();
                redisProfilingFields.Add(RedisProfilingConstants.Elapsed, command.Elapsed.Milliseconds);
                redisProfilingFields.Add(RedisProfilingConstants.OperationCount, command.OperationCount.ToString());
                redisProfilingTags.Add(ProfilingConstants.ApplicationName, _apmOptions.ApplicationName);
                redisProfilingTags.Add(ProfilingConstants.Environment, _apmOptions.Environment);
                redisProfilingTags.Add(RedisProfilingConstants.ClientName, command.ClientName);
                redisProfilingTags.Add(RedisProfilingConstants.Command, command.Command);
                redisProfilingTags.Add(RedisProfilingConstants.Db, command.Db.ToString());
                redisProfilingTags.Add(RedisProfilingConstants.Server, command.Server.ToString());
                points.Add(new Point(profilingContext.ProfilerName, redisProfilingFields, redisProfilingTags));
            }
            return Task.FromResult(_collector.Push(new Payload(points)));
        }
    }
}