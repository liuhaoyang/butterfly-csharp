using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public class RedisProfiledCallback : IProfiledCallback<RedisProfiledCallbackContext>
    {
        private readonly ICollector _collector;

        public Task Invoke(RedisProfiledCallbackContext callbackContext)
        {
            var points = new List<Point>();
            foreach(var command in callbackContext.ProfiledCommands)
            {
                var redisProfiledFields = new FieldCollection();
                var redisProfiledTags = new TagCollection();
                redisProfiledFields.Add(nameof(command.Elapsed), command.Elapsed.Milliseconds);
                redisProfiledTags.Add(nameof(command.ClientName), command.ClientName);
                redisProfiledTags.Add(nameof(command.Command), command.Command);
                redisProfiledTags.Add(nameof(command.Db), command.Db.ToString());
                redisProfiledTags.Add(nameof(command.OperationCount), command.OperationCount.ToString());
                redisProfiledTags.Add(nameof(command.Server), command.Server.ToString());
                points.Add(new Point(callbackContext.ProfiledContext.ProfilerName, redisProfiledFields, redisProfiledTags));
            }
            return Task.FromResult(_collector.Push(new Payload(points)));
        }
    }
}