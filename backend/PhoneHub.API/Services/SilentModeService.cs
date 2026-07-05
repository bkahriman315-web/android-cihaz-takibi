using PhoneHub.Core.Models;
using PhoneHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace PhoneHub.API.Services;

public interface ISilentModeService
{
    Task<SilentModeSettings?> GetSilentModeSettingsAsync(Guid deviceId);
    Task<SilentModeSettings> EnableSilentModeAsync(Guid deviceId, Guid userId, Dictionary<string, object> settings, string? pin = null);
    Task<bool> DisableSilentModeAsync(Guid deviceId, string? pin = null);
    Task<SilentModeSession> StartSilentSessionAsync(Guid deviceId, Guid userId, string? ipAddress = null);
    Task<SilentModeSession?> EndSilentSessionAsync(Guid sessionId);
    Task<HiddenCommand> ExecuteHiddenCommandAsync(Guid deviceId, Guid sessionId, string commandType, string? parameters, string? pin = null);
    Task<List<HiddenCommand>> GetSessionCommandsAsync(Guid sessionId);
    Task<List<SilentModeSession>> GetDeviceSilentSessionsAsync(Guid deviceId, int limit = 50);
    bool VerifyPIN(string inputPin, string storedHash);
    string HashPIN(string pin);
}

public class SilentModeService : ISilentModeService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<SilentModeService> _logger;

    public SilentModeService(AppDbContext dbContext, ILogger<SilentModeService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<SilentModeSettings?> GetSilentModeSettingsAsync(Guid deviceId)
    {
        return await _dbContext.SilentModeSettings
            .Where(s => s.DeviceId == deviceId)
            .FirstOrDefaultAsync();
    }

    public async Task<SilentModeSettings> EnableSilentModeAsync(Guid deviceId, Guid userId, Dictionary<string, object> settings, string? pin = null)
    {
        var device = await _dbContext.Devices.FindAsync(deviceId)
            ?? throw new InvalidOperationException("Cihaz bulunamadı");

        var existingSettings = await GetSilentModeSettingsAsync(deviceId);

        if (existingSettings != null)
        {
            existingSettings.IsEnabled = true;
            existingSettings.KeepDisplayOff = (bool)settings.GetValueOrDefault("keepDisplayOff", true);
            existingSettings.MuteAudio = (bool)settings.GetValueOrDefault("muteAudio", true);
            existingSettings.DisableVibration = (bool)settings.GetValueOrDefault("disableVibration", true);
            existingSettings.DisableNotificationLED = (bool)settings.GetValueOrDefault("disableNotificationLED", true);
            existingSettings.HideSystemNotifications = (bool)settings.GetValueOrDefault("hideSystemNotifications", true);
            existingSettings.DisableLogs = (bool)settings.GetValueOrDefault("disableLogs", true);
            existingSettings.ClearHistoryAfterSession = (bool)settings.GetValueOrDefault("clearHistoryAfterSession", true);
            existingSettings.EnabledAt = DateTime.UtcNow;
            existingSettings.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(pin))
            {
                existingSettings.PIN = pin;
                existingSettings.PINHash = HashPIN(pin);
            }
        }
        else
        {
            var newSettings = new SilentModeSettings
            {
                Id = Guid.NewGuid(),
                DeviceId = deviceId,
                IsEnabled = true,
                KeepDisplayOff = (bool)settings.GetValueOrDefault("keepDisplayOff", true),
                MuteAudio = (bool)settings.GetValueOrDefault("muteAudio", true),
                DisableVibration = (bool)settings.GetValueOrDefault("disableVibration", true),
                DisableNotificationLED = (bool)settings.GetValueOrDefault("disableNotificationLED", true),
                HideSystemNotifications = (bool)settings.GetValueOrDefault("hideSystemNotifications", true),
                DisableLogs = (bool)settings.GetValueOrDefault("disableLogs", true),
                ClearHistoryAfterSession = (bool)settings.GetValueOrDefault("clearHistoryAfterSession", true),
                EnabledAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(pin))
            {
                newSettings.PIN = pin;
                newSettings.PINHash = HashPIN(pin);
            }

            _dbContext.SilentModeSettings.Add(newSettings);
            existingSettings = newSettings;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogWarning("Gizli mod etkinleştirildi - Cihaz: {DeviceId}, Kullanıcı: {UserId}", deviceId, userId);

        return existingSettings;
    }

    public async Task<bool> DisableSilentModeAsync(Guid deviceId, string? pin = null)
    {
        var settings = await GetSilentModeSettingsAsync(deviceId);
        if (settings == null) return false;

        if (!string.IsNullOrEmpty(settings.PINHash) && !VerifyPIN(pin ?? string.Empty, settings.PINHash))
        {
            throw new UnauthorizedAccessException("Gizli mod PIN'i yanlış");
        }

        settings.IsEnabled = false;
        settings.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        _logger.LogWarning("Gizli mod devre dışı bırakıldı - Cihaz: {DeviceId}", deviceId);

        return true;
    }

    public async Task<SilentModeSession> StartSilentSessionAsync(Guid deviceId, Guid userId, string? ipAddress = null)
    {
        var settings = await GetSilentModeSettingsAsync(deviceId)
            ?? throw new InvalidOperationException("Gizli mod ayarları bulunamadı");

        if (!settings.IsEnabled)
            throw new InvalidOperationException("Gizli mod etkinleştirilmemiş");

        var session = new SilentModeSession
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            OperationCount = 0,
            Operations = new List<string>(),
            IPAddress = ipAddress
        };

        _dbContext.SilentModeSessions.Add(session);
        await _dbContext.SaveChangesAsync();

        _logger.LogWarning("Gizli oturum başlatıldı - Oturum: {SessionId}, Cihaz: {DeviceId}", session.Id, deviceId);

        return session;
    }

    public async Task<SilentModeSession?> EndSilentSessionAsync(Guid sessionId)
    {
        var session = await _dbContext.SilentModeSessions.FindAsync(sessionId);
        if (session == null) return null;

        session.EndedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        _logger.LogWarning("Gizli oturum sonlandırıldı - Oturum: {SessionId}, İşlem Sayısı: {OperationCount}", 
            session.Id, session.OperationCount);

        return session;
    }

    public async Task<HiddenCommand> ExecuteHiddenCommandAsync(Guid deviceId, Guid sessionId, string commandType, 
        string? parameters, string? pin = null)
    {
        var settings = await GetSilentModeSettingsAsync(deviceId)
            ?? throw new InvalidOperationException("Gizli mod ayarları bulunamadı");

        if (!string.IsNullOrEmpty(settings.PINHash) && !VerifyPIN(pin ?? string.Empty, settings.PINHash))
        {
            throw new UnauthorizedAccessException("Gizli mod PIN'i yanlış");
        }

        var session = await _dbContext.SilentModeSessions.FindAsync(sessionId)
            ?? throw new InvalidOperationException("Oturum bulunamadı");

        var command = new HiddenCommand
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            SessionId = sessionId,
            CommandType = commandType,
            Parameters = parameters,
            IsExecuted = false,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.HiddenCommands.Add(command);

        session.OperationCount++;
        if (session.Operations == null) session.Operations = new List<string>();
        session.Operations.Add($"[{DateTime.UtcNow:HH:mm:ss}] {commandType}");

        if (!settings.DisableLogs)
        {
            var logEntry = $"Komut: {commandType}, Parametreler: {parameters ?? "Yok"}, Zaman: {DateTime.UtcNow:O}";
            command.OperationLogs = logEntry;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogWarning("Gizli komut yürütüldü - Komut: {CommandType}, Oturum: {SessionId}", commandType, sessionId);

        return command;
    }

    public async Task<List<HiddenCommand>> GetSessionCommandsAsync(Guid sessionId)
    {
        return await _dbContext.HiddenCommands
            .Where(c => c.SessionId == sessionId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<SilentModeSession>> GetDeviceSilentSessionsAsync(Guid deviceId, int limit = 50)
    {
        return await _dbContext.SilentModeSessions
            .Where(s => s.DeviceId == deviceId)
            .OrderByDescending(s => s.StartedAt)
            .Take(limit)
            .ToListAsync();
    }

    public bool VerifyPIN(string inputPin, string storedHash)
    {
        if (string.IsNullOrEmpty(inputPin) || string.IsNullOrEmpty(storedHash))
            return false;

        try
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(inputPin));
            var hashString = Convert.ToHexString(hash);
            return hashString == storedHash;
        }
        catch
        {
            return false;
        }
    }

    public string HashPIN(string pin)
    {
        if (string.IsNullOrEmpty(pin))
            throw new ArgumentException("PIN boş olamaz");

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
        return Convert.ToHexString(hash);
    }
}
