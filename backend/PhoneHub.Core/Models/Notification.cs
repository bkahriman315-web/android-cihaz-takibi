namespace PhoneHub.Core.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string ApplicationName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public bool IsRead { get; set; }

    public Device? Device { get; set; }
}
