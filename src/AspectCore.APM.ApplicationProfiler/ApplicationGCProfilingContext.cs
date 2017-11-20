using System;
using System.Runtime;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationGCProfilingContext : IProfilingContext
    {
        public string ProfilerName => "application_gc";

        public int MaxGeneration { get; set; }

        public int Gen0_CollectionCount { get; set; }

        public int Gen1_CollectionCount { get; set; }

        public int Gen2_CollectionCount { get; set; }

        public long TotalMemory { get; set; }

        public int TotalCollectionCount
        {
            get
            {
                return Gen0_CollectionCount + Gen1_CollectionCount + Gen2_CollectionCount;
            }
        }

        public string GCMode { get; set; }

        public string GCLatencyMode { get; set; }

        public static ApplicationGCProfilingContext GetSnapshot()
        {
            var context = new ApplicationGCProfilingContext()
            {
                MaxGeneration = GC.MaxGeneration,
                Gen0_CollectionCount = GC.CollectionCount(0),
                Gen1_CollectionCount = GC.CollectionCount(1),
                Gen2_CollectionCount = GC.CollectionCount(2),
                GCLatencyMode = GCSettings.LatencyMode.ToString(),
                GCMode = GCSettings.IsServerGC ? ApplicationProfilingConstants.GC_Server_Mode : ApplicationProfilingConstants.GC_Workstation_Mode,
                TotalMemory = GC.GetTotalMemory(false)
            };
            return context;
        }
    }
}