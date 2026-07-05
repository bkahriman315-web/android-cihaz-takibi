using PhoneHub.Core.Enums;

namespace PhoneHub.API.DTOs.SilentMode;

public class SilentModeStatusResponse
{
    public Guid DeviceId { get; set; }
    public bool IsEnabled { get; set; }
    public bool KeepDisplayOff { get; set; }
    public bool MuteAudio { get; set; }
    public bool DisableVibration { get; set; }
    public bool DisableNotificationLED { get; set; }
    public bool HideSystemNotifications { get; set; }
    public bool DisableLogs { get; set; }
    public bool ClearHistoryAfterSession { get; set; }
    public DateTime? EnabledAt { get; set; }
    public DisplayState CurrentDisplayState { get; set; }
}
