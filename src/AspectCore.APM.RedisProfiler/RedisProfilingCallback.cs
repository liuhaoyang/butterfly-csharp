using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public class RedisProfilingCallback : IProfilingCallback<RedisProfilingCallbackContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _apmOptions;

        public RedisProfilingCallback(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _apmOptions = optionAccessor.Value;
        }

        public Task Invoke(RedisProfilingCallbackContext callbackContext)
        {
            var points = new List<Point>();
            foreach(var command in callbackContext.ProfilingCommands)
            {
                var redisProfilingFields = new FieldCollection();
                var redisProfilingTags = new TagCollection();
                redisProfilingFields.Add(RedisProfilingConstants.Elapsed, command.Elapsed.Milliseconds);
                redisProfilingFields.Add(RedisProfilingConstants.OperationCount, command.OperationCount.ToString());
                redisProfilingTags.Add(ProfilingConstants.ApplicationName, _apmOptions.ApplicationName);
                redisProfilingTags.Add(ProfilingConstants.Environment, _apmOptions.Environment);
                redisProfilingTags.Add(ProfilingConstants.Host, _apmOptions.Host);
                redisProfilingTags.Add(RedisProfilingConstants.ClientName, command.ClientName);
                redisProfilingTags.Add(RedisProfilingConstants.Command, command.Command);
                redisProfilingTags.Add(RedisProfilingConstants.Db, command.Db.ToString());
                redisProfilingTags.Add(RedisProfilingConstants.Server, command.Server.ToString());
                points.Add(new Point(callbackContext.ProfilingContext.ProfilerName, redisProfilingFields, redisProfilingTags));
            }
            return Task.FromResult(_collector.Push(new Payload(points)));
        }
    }
}