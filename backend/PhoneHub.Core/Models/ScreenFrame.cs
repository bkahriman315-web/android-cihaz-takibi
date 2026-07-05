namespace PhoneHub.Core.Models;

public class ScreenFrame
{
    public long Id { get; set; }
    public Guid DeviceId { get; set; }
    public byte[]? FrameData { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int FrameRate { get; set; }
    public DateTime Timestamp { get; set; }
    public string? CompressionType { get; set; }

    public Device? Device { get; set; }
}
