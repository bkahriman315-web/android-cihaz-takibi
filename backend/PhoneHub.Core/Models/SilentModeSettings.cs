namespace PhoneHub.Core.Models;

/// <summary>
/// Gizli Mod Ayarları
/// </summary>
public class SilentModeSettings
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    
    /// <summary>
    /// Gizli mod etkinleştirildi mi
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Ekranı kapalı tutma
    /// </summary>
    public bool KeepDisplayOff { get; set; }

    /// <summary>
    /// Ses sistemini kapat
    /// </summary>
    public bool MuteAudio { get; set; }

    /// <summary>
    /// Titreşimi devre dışı bırak
    /// </summary>
    public bool DisableVibration { get; set; }

    /// <summary>
    /// Bildirim LED'ini kapat
    /// </summary>
    public bool DisableNotificationLED { get; set; }

    /// <summary>
    /// Sistem bildirimlerini gizle
    /// </summary>
    public bool HideSystemNotifications { get; set; }

    /// <summary>
    /// Logları kaydetme (gizli işlemler kayıt altına alınmaz)
    /// </summary>
    public bool DisableLogs { get; set; }

    /// <summary>
    /// Gizli mode PIN koruması
    /// </summary>
    public string? PIN { get; set; }

    /// <summary>
    /// PIN hash'i (salt ile birlikte)
    /// </summary>
    public string? PINHash { get; set; }

    /// <summary>
    /// Gizli işlem geçmişini sil
    /// </summary>
    public bool ClearHistoryAfterSession { get; set; }

    /// <summary>
    /// Etkinleştirilme tarihi
    /// </summary>
    public DateTime EnabledAt { get; set; }

    /// <summary>
    /// Son güncellenme
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public Device? Device { get; set; }
}
