namespace PhoneHub.Core.Enums;

/// <summary>
/// Kontrol modu - Normal veya Gizli
/// </summary>
public enum ControlMode
{
    /// <summary>
    /// Normal mod - Tüm işlemler telefonda görünür
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Gizli mod - İşlemler telefonda görünmez, ekran kapalı kalır
    /// </summary>
    Silent = 1
}
