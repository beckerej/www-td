using System.Diagnostics.CodeAnalysis;

namespace www_td.Database.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class machinestats
    {
        public int id { get; set; }
        public string machinename { get; set; }
        public int totalMemory { get; set; }
        public int usedMemory { get; set; }
        public int freeMemory { get; set; }
        public int sharedMemory { get; set; }
        public int cacheMemory { get; set; }
        public int availableMemory { get; set; }
        public int diskUsage { get; set; }
        public float cpuUsage { get; set; }
        public float cpuIdle { get; set; }
    }
}
