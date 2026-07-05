package com.phonehub.android.network

import android.content.Context
import android.util.Log
import com.phonehub.android.models.HiddenCommand
import com.phonehub.android.models.SilentModeSession
import com.phonehub.android.models.SilentModeSettings
import kotlinx.coroutines.*
import okhttp3.*
import okhttp3.MediaType.Companion.toMediaType
import org.json.JSONObject
import java.io.IOException
import java.util.concurrent.TimeUnit

/**
 * Sunucu ile iletişim sağlayan NetworkService
 * Gizli mod komutlarını sunucudan alır ve yürütür
 */
class SilentModeNetworkService(private val context: Context) {

    private val baseUrl = "https://api.phonehub.local:7001"
    private val httpClient = OkHttpClient.Builder()
        .connectTimeout(30, TimeUnit.SECONDS)
        .readTimeout(30, TimeUnit.SECONDS)
        .writeTimeout(30, TimeUnit.SECONDS)
        .build()

    private val scope = CoroutineScope(Dispatchers.IO + Job())
    private var authToken: String? = null
    private var deviceId: String? = null
    private var currentSessionId: String? = null

    /**
     * Sunucuya bağlan ve gizli mod oturumunu başlat
     */
    fun connectAndStartSession(
        deviceId: String,
        userId: String,
        authToken: String,
        onSuccess: (SilentModeSession) -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                this@SilentModeNetworkService.authToken = authToken
                this@SilentModeNetworkService.deviceId = deviceId

                // Gizli oturum başlat endpoint'ini çağır
                val request = Request.Builder()
                    .url("$baseUrl/api/silentmode/session/start/$deviceId")
                    .addHeader("Authorization", "Bearer $authToken")
                    .post(RequestBody.create("{}".toByteArray(), "application/json".toMediaType()))
                    .build()

                val response = httpClient.newCall(request).execute()

                if (response.isSuccessful) {
                    val responseBody = response.body?.string() ?: "{}"
                    val json = JSONObject(responseBody)
                    val sessionData = json.optJSONObject("data") ?: json

                    val session = SilentModeSession(
                        id = sessionData.optString("sessionId", ""),
                        deviceId = deviceId,
                        userId = userId,
                        startedAt = System.currentTimeMillis()
                    )

                    currentSessionId = session.id

                    Log.d("SilentModeNetwork", "Gizli oturum başlatıldı: ${session.id}")
                    withContext(Dispatchers.Main) {
                        onSuccess(session)
                    }
                } else {
                    throw IOException("Gizli oturum başlatılamadı: ${response.code}")
                }
            } catch (e: Exception) {
                Log.e("SilentModeNetwork", "Bağlantı başarısız: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    /**
     * Gizli oturumu sonlandır
     */
    fun endSession(
        onSuccess: () -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                if (currentSessionId == null || deviceId == null) {
                    throw Exception("Oturum bilgisi yok")
                }

                val request = Request.Builder()
                    .url("$baseUrl/api/silentmode/session/end/$currentSessionId")
                    .addHeader("Authorization", "Bearer $authToken")
                    .post(RequestBody.create("{}".toByteArray(), "application/json".toMediaType()))
                    .build()

                val response = httpClient.newCall(request).execute()

                if (response.isSuccessful) {
                    currentSessionId = null
                    Log.d("SilentModeNetwork", "Gizli oturum sonlandırıldı")
                    withContext(Dispatchers.Main) {
                        onSuccess()
                    }
                } else {
                    throw IOException("Oturum sonlandırılamadı: ${response.code}")
                }
            } catch (e: Exception) {
                Log.e("SilentModeNetwork", "Oturum sonlandırma başarısız: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    /**
     * Gizli komut sunucuya gönder
     */
    fun sendHiddenCommand(
        command: HiddenCommand,
        onSuccess: () -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                if (deviceId == null || currentSessionId == null) {
                    throw Exception("Cihaz veya oturum bilgisi yok")
                }

                val commandBody = JSONObject().apply {
                    put("commandType", command.commandType)
                    put("parameters", command.parameters)
                }

                val request = Request.Builder()
                    .url("$baseUrl/api/silentmode/command/execute/$deviceId/$currentSessionId")
                    .addHeader("Authorization", "Bearer $authToken")
                    .post(RequestBody.create(
                        commandBody.toString().toByteArray(),
                        "application/json".toMediaType()
                    ))
                    .build()

                val response = httpClient.newCall(request).execute()

                if (response.isSuccessful) {
                    Log.d("SilentModeNetwork", "Gizli komut gönderildi: ${command.commandType}")
                    withContext(Dispatchers.Main) {
                        onSuccess()
                    }
                } else {
                    throw IOException("Komut gönderilemedi: ${response.code}")
                }
            } catch (e: Exception) {
                Log.e("SilentModeNetwork", "Komut gönderme başarısız: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    /**
     * Sunucudan gizli komut al (polling)
     */
    fun pollHiddenCommands(
        onCommandReceived: (HiddenCommand) -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                if (deviceId == null || currentSessionId == null) {
                    return@launch
                }

                while (isActive) {
                    try {
                        val request = Request.Builder()
                            .url("$baseUrl/api/silentmode/session/$currentSessionId/commands")
                            .addHeader("Authorization", "Bearer $authToken")
                            .get()
                            .build()

                        val response = httpClient.newCall(request).execute()

                        if (response.isSuccessful) {
                            val responseBody = response.body?.string() ?: "[]"
                            // JSON parsing ve komut işleme
                            Log.d("SilentModeNetwork", "Komutlar alındı")
                        }

                        // 5 saniyede bir kontrol et
                        delay(5000)
                    } catch (e: Exception) {
                        Log.e("SilentModeNetwork", "Komut polling hatası: ${e.message}")
                        delay(5000)
                    }
                }
            } catch (e: Exception) {
                Log.e("SilentModeNetwork", "Polling başarısız: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    /**
     * Gizli mod ayarlarını sunucudan al
     */
    fun getSilentModeSettings(
        onSuccess: (SilentModeSettings) -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                if (deviceId == null) {
                    throw Exception("Cihaz bilgisi yok")
                }

                val request = Request.Builder()
                    .url("$baseUrl/api/silentmode/status/$deviceId")
                    .addHeader("Authorization", "Bearer $authToken")
                    .get()
                    .build()

                val response = httpClient.newCall(request).execute()

                if (response.isSuccessful) {
                    val responseBody = response.body?.string() ?: "{}"
                    val json = JSONObject(responseBody)

                    val settings = SilentModeSettings(
                        deviceId = json.optString("deviceId", ""),
                        isEnabled = json.optBoolean("isEnabled", false),
                        keepDisplayOff = json.optBoolean("keepDisplayOff", true),
                        muteAudio = json.optBoolean("muteAudio", true),
                        disableVibration = json.optBoolean("disableVibration", true),
                        disableNotificationLED = json.optBoolean("disableNotificationLED", true),
                        hideSystemNotifications = json.optBoolean("hideSystemNotifications", true),
                        disableLogs = json.optBoolean("disableLogs", true)
                    )

                    withContext(Dispatchers.Main) {
                        onSuccess(settings)
                    }
                } else {
                    throw IOException("Ayarlar alınamadı: ${response.code}")
                }
            } catch (e: Exception) {
                Log.e("SilentModeNetwork", "Ayarlar alınamadı: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    fun destroy() {
        scope.cancel()
        httpClient.dispatcher.executorService.shutdown()
    }
}
