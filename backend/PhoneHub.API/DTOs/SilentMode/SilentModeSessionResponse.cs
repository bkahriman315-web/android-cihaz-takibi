namespace PhoneHub.API.DTOs.SilentMode;

public class SilentModeSessionResponse
{
    public Guid SessionId { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int OperationCount { get; set; }
    public List<string>? Operations { get; set; }
    public string? IPAddress { get; set; }
    public TimeSpan Duration { get; set; }
}
