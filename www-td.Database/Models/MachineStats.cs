namespace www_td.Database.Models
{
    public class MachineStats
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

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(MachineName)}: {MachineName}, " +
                   $"{nameof(TotalMemory)}: {TotalMemory}, " +
                   $"{nameof(UsedMemory)}: {UsedMemory}, " +
                   $"{nameof(FreeMemory)}: {FreeMemory}, " +
                   $"{nameof(SharedMemory)}: {SharedMemory}, " +
                   $"{nameof(CacheMemory)}: {CacheMemory}, " +
                   $"{nameof(AvailableMemory)}: {AvailableMemory}, " +
                   $"{nameof(DiskUsage)}: {DiskUsage}, " +
                   $"{nameof(CpuUsage)}: {CpuUsage}, " +
                   $"{nameof(CpuIdle)}: {CpuIdle}";
        }
    }
}
