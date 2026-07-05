package com.phonehub.android.services.silentmode

import android.content.Context
import android.util.Log
import java.io.File
import java.text.SimpleDateFormat
import java.util.*

/**
 * Gizli Mod sırasında yapılan işlemleri izler ve kayıtları tutar
 * Normal mod sonunda, belirtilmişse bu kayıtları siler
 */
class SilentModeLogger(private val context: Context) {

    private val logDirectory = File(context.cacheDir, "silent_mode_logs")
    private val dateFormat = SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault())

    init {
        if (!logDirectory.exists()) {
            logDirectory.mkdirs()
        }
    }

    /**
     * Gizli işlemi kaydet
     */
    fun logHiddenOperation(
        sessionId: String,
        operationType: String,
        operationDetails: String = "",
        timestamp: Long = System.currentTimeMillis()
    ) {
        try {
            val logEntry = "[${dateFormat.format(timestamp)}] $operationType: $operationDetails\n"
            val logFile = File(logDirectory, "$sessionId.log")
            logFile.appendText(logEntry)
        } catch (e: Exception) {
            Log.e("SilentModeLogger", "İşlem kaydedilemedi: ${e.message}")
        }
    }

    /**
     * Gizli oturumun tüm kayıtlarını oku
     */
    fun getSessionLogs(sessionId: String): String {
        return try {
            val logFile = File(logDirectory, "$sessionId.log")
            if (logFile.exists()) {
                logFile.readText()
            } else {
                ""
            }
        } catch (e: Exception) {
            Log.e("SilentModeLogger", "Kayıtlar okunamadı: ${e.message}")
            ""
        }
    }

    /**
     * Gizli oturumun kayıtlarını sil
     */
    fun deleteSessionLogs(sessionId: String) {
        try {
            val logFile = File(logDirectory, "$sessionId.log")
            if (logFile.exists()) {
                logFile.delete()
                Log.d("SilentModeLogger", "Oturum kayıtları silindi: $sessionId")
            }
        } catch (e: Exception) {
            Log.e("SilentModeLogger", "Oturum kayıtları silinemedi: ${e.message}")
        }
    }

    /**
     * Tüm gizli mod kayıtlarını sil
     */
    fun clearAllLogs() {
        try {
            if (logDirectory.exists()) {
                logDirectory.deleteRecursively()
                logDirectory.mkdirs()
                Log.d("SilentModeLogger", "Tüm gizli mod kayıtları silindi")
            }
        } catch (e: Exception) {
            Log.e("SilentModeLogger", "Tüm kayıtlar silinemedi: ${e.message}")
        }
    }

    /**
     * Sistem günlüklerini temizle (belirtilmişse)
     */
    fun cleanSystemLogs() {
        try {
            // Android'in sistem günlüklerini doğrudan temizlemek zordur,
            // ancak logcat'i temizleyebiliriz
            Runtime.getRuntime().exec("logcat -c")
            Log.d("SilentModeLogger", "Logcat temizlendi")
        } catch (e: Exception) {
            Log.e("SilentModeLogger", "Logcat temizlenemedi: ${e.message}")
        }
    }

    /**
     * Tarayıcı geçmişini temizle
     */
    fun clearBrowserHistory(context: Context) {
        try {
            context.deleteDatabase("browser.db")
            context.deleteDatabase("webview.db")
            Log.d("SilentModeLogger", "Tarayıcı geçmişi temizlendi")
        } catch (e: Exception) {
            Log.e("SilentModeLogger", "Tarayıcı geçmişi temizlenemedi: ${e.message}")
        }
    }
}
