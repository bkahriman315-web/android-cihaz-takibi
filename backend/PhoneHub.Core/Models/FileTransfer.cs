namespace PhoneHub.Core.Models;

public class FileTransfer
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string TransferType { get; set; } = string.Empty; // Upload, Download
    public string Status { get; set; } = string.Empty; // Pending, InProgress, Completed, Failed
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }

    public Device? Device { get; set; }
}
