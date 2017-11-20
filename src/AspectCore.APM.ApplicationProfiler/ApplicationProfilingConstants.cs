using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.ApplicationProfiler
{
    public static class ApplicationProfilingConstants
    {
        public const string GC_Workstation_Mode = "Workstation";

        public const string GC_Server_Mode = "Server";

        public const string MaxGeneration = "max_gen";

        public const string Gen0_CollectionCount = "gen0_collect_count";

        public const string Gen1_CollectionCount = "gen1_collect_count";

        public const string Gen2_CollectionCount = "gen2_collect_count";

        public const string TotalCollectionCount = "total_collect_count";

        public const string TotalMemory = "total_memory";

        public const string GCMode = "gc_mode";

        public const string GCLatencyMode = "gc_latency_mode";

        public const string AvailableWorkerThreads = "available_worker_threads";

        public const string AvailableCompletionPortThreads = "available_completion_port_threads";

        public const string MinWorkerThreads = "min_worker_threads";

        public const string MinCompletionPortThreads = "min_completion_port_threads";

        public const string MaxWorkerThreads = "max_worker_threads";

        public const string MaxCompletionPortThreads = "max_completion_port_threads";

        public const string RunningCompletionPortThreads = "running_completion_port_threads";

        public const string RunningWorkerThreads = "running_worker_threads";
    }
}