namespace PhoneHub.API.DTOs.SilentMode;

public class EnableSilentModeRequest
{
    /// <summary>
    /// Gizli mod PIN'i (isteğe bağlı)
    /// </summary>
    public string? PIN { get; set; }

    /// <summary>
    /// Ekranı kapalı tutma
    /// </summary>
    public bool KeepDisplayOff { get; set; } = true;

    /// <summary>
    /// Ses sistemini kapat
    /// </summary>
    public bool MuteAudio { get; set; } = true;

    /// <summary>
    /// Titreşimi devre dışı bırak
    /// </summary>
    public bool DisableVibration { get; set; } = true;

    /// <summary>
    /// Bildirim LED'ini kapat
    /// </summary>
    public bool DisableNotificationLED { get; set; } = true;

    /// <summary>
    /// Sistem bildirimlerini gizle
    /// </summary>
    public bool HideSystemNotifications { get; set; } = true;

    /// <summary>
    /// Logları devre dışı bırak
    /// </summary>
    public bool DisableLogs { get; set; } = true;

    /// <summary>
    /// Oturumun ardından geçmişi temizle
    /// </summary>
    public bool ClearHistoryAfterSession { get; set; } = true;
}
