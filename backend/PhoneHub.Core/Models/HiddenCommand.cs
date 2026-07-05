namespace PhoneHub.Core.Models;

/// <summary>
/// Gizli Komut - Telefonda izleme kaydı bırakmadan komut yürütülür
/// </summary>
public class HiddenCommand
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Guid SessionId { get; set; }

    /// <summary>
    /// Komut türü: Tap, Swipe, TypeText, OpenApp, DeleteFile, vb.
    /// </summary>
    public string CommandType { get; set; } = string.Empty;

    /// <summary>
    /// Komut parametreleri (JSON)
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Komut yürütüldü mü
    /// </summary>
    public bool IsExecuted { get; set; }

    /// <summary>
    /// Yürütülme zamanı
    /// </summary>
    public DateTime? ExecutedAt { get; set; }

    /// <summary>
    /// Hata mesajı (varsa)
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Komut oluşturulma zamanı
    /// </summary>
    public DateTime CreatedAt { get; set; }

    public SilentModeSession? SilentModeSession { get; set; }
    public Device? Device { get; set; }
}
