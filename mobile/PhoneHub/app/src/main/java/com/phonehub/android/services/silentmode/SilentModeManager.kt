package com.phonehub.android.services.silentmode

import android.content.Context
import android.content.Intent
import android.content.ServiceConnection
import android.util.Log
import com.phonehub.android.models.HiddenCommand
import com.phonehub.android.models.SilentModeSession
import com.phonehub.android.models.SilentModeSettings
import kotlinx.coroutines.*
import java.util.*

/**
 * Gizli Mod Yönetim Servisi
 * - Gizli mod ayarlarını yönet
 * - Gizli oturumları başlat ve bitir
 * - Gizli komutları işle
 */
class SilentModeManager(private val context: Context) {

    private val localStorage = SilentModeLocalStorage(context)
    private val systemManager = SilentModeSystemManager(context)
    private val logger = SilentModeLogger(context)
    private val scope = CoroutineScope(Dispatchers.Default + Job())

    private var currentSession: SilentModeSession? = null
    private var isInSilentMode = false

    fun enableSilentMode(
        deviceId: String,
        userId: String,
        settings: SilentModeSettings,
        onSuccess: () -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                val updatedSettings = settings.copy(
                    isEnabled = true,
                    deviceId = deviceId,
                    enabledAt = System.currentTimeMillis(),
                    updatedAt = System.currentTimeMillis()
                )

                localStorage.saveSilentModeSettings(updatedSettings)
                systemManager.applyGlobalSilentMode()
                isInSilentMode = true

                Log.d("SilentModeManager", "Gizli mod etkinleştirildi")
                withContext(Dispatchers.Main) {
                    onSuccess()
                }
            } catch (e: Exception) {
                Log.e("SilentModeManager", "Gizli mod etkinleştirilemedi: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    fun disableSilentMode(
        pin: String? = null,
        onSuccess: () -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                val settings = localStorage.getSilentModeSettings()
                
                // PIN doğrula
                if (settings.pinHash != null && !localStorage.verifyPIN(pin ?: "", settings.pinHash)) {
                    throw Exception("Gizli mod PIN'i yanlış")
                }

                systemManager.removeGlobalSilentMode()
                
                // Oturum bitir
                if (currentSession != null) {
                    endSilentSession(onSuccess = {}, onError = {})
                }

                isInSilentMode = false
                Log.d("SilentModeManager", "Gizli mod devre dışı bırakıldı")
                
                withContext(Dispatchers.Main) {
                    onSuccess()
                }
            } catch (e: Exception) {
                Log.e("SilentModeManager", "Gizli mod devre dışı bırakılamadı: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    fun startSilentSession(
        deviceId: String,
        userId: String,
        onSuccess: (SilentModeSession) -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                val settings = localStorage.getSilentModeSettings()
                if (!settings.isEnabled) {
                    throw Exception("Gizli mod etkinleştirilmemiş")
                }

                val session = SilentModeSession(
                    deviceId = deviceId,
                    userId = userId,
                    startedAt = System.currentTimeMillis()
                )

                currentSession = session
                logger.logHiddenOperation(session.id, "SESSION_START", "Gizli oturum başlatıldı")

                Log.d("SilentModeManager", "Gizli oturum başlatıldı: ${session.id}")
                withContext(Dispatchers.Main) {
                    onSuccess(session)
                }
            } catch (e: Exception) {
                Log.e("SilentModeManager", "Gizli oturum başlatılamadı: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    fun endSilentSession(
        clearHistory: Boolean = true,
        onSuccess: () -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                if (currentSession == null) {
                    throw Exception("Aktif gizli oturum yok")
                }

                currentSession = currentSession?.copy(
                    endedAt = System.currentTimeMillis()
                )

                if (clearHistory) {
                    logger.deleteSessionLogs(currentSession!!.id)
                    logger.cleanSystemLogs()
                    logger.clearBrowserHistory(context)
                }

                logger.logHiddenOperation(currentSession!!.id, "SESSION_END", "Gizli oturum sonlandırıldı")

                Log.d("SilentModeManager", "Gizli oturum sonlandırıldı")
                withContext(Dispatchers.Main) {
                    onSuccess()
                }
            } catch (e: Exception) {
                Log.e("SilentModeManager", "Gizli oturum sonlandırılamadı: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    fun executeHiddenCommand(
        command: HiddenCommand,
        onSuccess: (HiddenCommand) -> Unit = {},
        onError: (String) -> Unit = {}
    ) {
        scope.launch {
            try {
                if (currentSession == null) {
                    throw Exception("Aktif gizli oturum yok")
                }

                val settings = localStorage.getSilentModeSettings()

                // Komutu işle
                val executedCommand = command.copy(
                    isExecuted = true,
                    executedAt = System.currentTimeMillis()
                )

                currentSession = currentSession?.copy(
                    operationCount = currentSession!!.operationCount + 1,
                    operations = (currentSession!!.operations + "[${Date()}] ${command.commandType}").toMutableList()
                )

                if (!settings.disableLogs) {
                    logger.logHiddenOperation(
                        currentSession!!.id,
                        command.commandType,
                        command.parameters ?: "Parametresiz"
                    )
                }

                Log.d("SilentModeManager", "Gizli komut yürütüldü: ${command.commandType}")
                withContext(Dispatchers.Main) {
                    onSuccess(executedCommand)
                }
            } catch (e: Exception) {
                Log.e("SilentModeManager", "Gizli komut yürütülemedi: ${e.message}")
                withContext(Dispatchers.Main) {
                    onError(e.message ?: "Bilinmeyen hata")
                }
            }
        }
    }

    fun isSilentModeEnabled(): Boolean {
        return isInSilentMode && localStorage.isEnabled()
    }

    fun getCurrentSession(): SilentModeSession? {
        return currentSession
    }

    fun getSilentModeSettings(): SilentModeSettings {
        return localStorage.getSilentModeSettings()
    }

    fun destroy() {
        scope.cancel()
    }
}
