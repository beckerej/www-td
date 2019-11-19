using System.Diagnostics.CodeAnalysis;

namespace www_td.Database.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class machinestats
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public int TotalMemory { get; set; }
        public int UsedMemory { get; set; }
        public int FreeMemory { get; set; }
        public int SharedMemory { get; set; }
        public int CacheMemory { get; set; }
        public int AvailableMemory { get; set; }
        public int DiskUsage { get; set; }
        public float CpuUsage { get; set; }
        public float CpuIdle { get; set; }
    }
}
