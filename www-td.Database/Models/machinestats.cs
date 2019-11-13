using System.Diagnostics.CodeAnalysis;

namespace www_td.Database.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class machinestats
    {
        public int id { get; set; }
        public string machinename { get; set; }
        public int totalmemory { get; set; }
        public int usedmemory { get; set; }
        public int freememory { get; set; }
        public int sharedmemory { get; set; }
        public int cachememory { get; set; }
        public int availablememory { get; set; }
        public int diskusage { get; set; }
        public float cpuusage { get; set; }
        public float cpuidle { get; set; }
    }
}
