using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Profiler;
using AspectCore.Extensions.Reflection;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationThreadingProfilingContext : IProfilingContext
    {
        public string ProfilerName => "application_threadpool";

        public int AvailableWorkerThreads { get; set; }

        public int AvailableCompletionPortThreads { get; set; }

        public int MinWorkerThreads { get; set; }

        public int MinCompletionPortThreads { get; set; }

        public int MaxWorkerThreads { get; set; }

        public int MaxCompletionPortThreads { get; set; }

        public static ApplicationThreadingProfilingContext GetSnapshot()
        {
            ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableCompletionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
            var context = new ApplicationThreadingProfilingContext
            {
                AvailableCompletionPortThreads = availableCompletionPortThreads,
                AvailableWorkerThreads = availableWorkerThreads,
                MaxCompletionPortThreads = maxCompletionPortThreads,
                MaxWorkerThreads = maxWorkerThreads,
                MinCompletionPortThreads = minCompletionPortThreads,
                MinWorkerThreads = minWorkerThreads,
            };

            return context;
        }
    }
}