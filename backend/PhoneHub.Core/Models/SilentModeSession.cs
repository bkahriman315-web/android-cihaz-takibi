namespace PhoneHub.Core.Models;

/// <summary>
/// Gizli Mod Oturumu - Gizli işlemleri izlemek için
/// </summary>
public class SilentModeSession
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Guid UserId { get; set; }

    /// <summary>
    /// Oturum başlangıcı
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Oturum bitişi
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// Gizli işlem sayısı
    /// </summary>
    public int OperationCount { get; set; }

    /// <summary>
    /// Yapılan işlemler
    /// Örnek: "Uygulama açıldı", "Dosya silinit", "SMS gönderildi"
    /// </summary>
    public List<string>? Operations { get; set; } = new();

    /// <summary>
    /// İşlemin dökmesi - JSON olarak
    /// </summary>
    public string? OperationLogs { get; set; }

    /// <summary>
    /// Gizli mod sırasında ekran durumu kaydı
    /// </summary>
    public string? DisplayStateHistory { get; set; }

    /// <summary>
    /// IP Adresi
    /// </summary>
    public string? IPAddress { get; set; }

    public User? User { get; set; }
    public Device? Device { get; set; }
}
