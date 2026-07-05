namespace PhoneHub.Core.Models;

public class DeviceStats
{
    public long Id { get; set; }
    public Guid DeviceId { get; set; }
    public int BatteryLevel { get; set; }
    public bool IsCharging { get; set; }
    public int MemoryUsage { get; set; }
    public int TotalMemory { get; set; }
    public int StorageUsage { get; set; }
    public int TotalStorage { get; set; }
    public int CpuUsage { get; set; }
    public string? WiFiSSID { get; set; }
    public bool IsMobileDataEnabled { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime Timestamp { get; set; }

    public Device? Device { get; set; }
}
