namespace PhoneHub.Core.Models;

public class Device
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string AndroidVersion { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string? IMEI { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? LastSeen { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? DeviceToken { get; set; }
    public string? IPAddress { get; set; }

    public User? User { get; set; }
    public SilentModeSettings? SilentModeSettings { get; set; }
    public ICollection<DeviceStats>? DeviceStats { get; set; }
    public ICollection<ScreenFrame>? ScreenFrames { get; set; }
    public ICollection<FileTransfer>? FileTransfers { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
    public ICollection<SilentModeSession>? SilentModeSessions { get; set; }
    public ICollection<HiddenCommand>? HiddenCommands { get; set; }
}
