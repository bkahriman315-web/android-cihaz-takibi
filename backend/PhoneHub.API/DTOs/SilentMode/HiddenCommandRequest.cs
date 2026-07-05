namespace PhoneHub.API.DTOs.SilentMode;

public class HiddenCommandRequest
{
    /// <summary>
    /// Komut türü: Tap, Swipe, TypeText, OpenApp, DeleteFile, etc.
    /// </summary>
    public string CommandType { get; set; } = string.Empty;

    /// <summary>
    /// Komut parametreleri (JSON)
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Gizli PIN (eğer ayarlanmışsa)
    /// </summary>
    public string? PIN { get; set; }
}
