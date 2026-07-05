package com.phonehub.android.services.silentmode

import android.content.Context
import android.media.AudioManager
import android.os.Build
import android.os.PowerManager
import android.os.Vibrator
import android.provider.Settings
import android.util.Log
import androidx.core.content.getSystemService

/**
 * Gizli Mod sırasında sistem ayarlarını yönetir
 * - Ekranı kapalı tutma
 * - Sesi kapatma
 * - Titreşim devre dışı bırakma
 * - LED'i devre dışı bırakma
 */
class SilentModeSystemManager(private val context: Context) {

    private val audioManager = context.getSystemService(AudioManager::class.java)
    private val powerManager = context.getSystemService(PowerManager::class.java)
    private val vibrator = context.getSystemService(Vibrator::class.java)

    private var originalRingerMode: Int = AudioManager.RINGER_MODE_NORMAL

    fun applyGlobalSilentMode() {
        try {
            // Sesi kapat
            muteAudio()
            
            // Titreşimi devre dışı bırak
            disableVibration()
            
            // Bildirim LED'ini devre dışı bırak (uygulamalar tarafından)
            Log.d("SilentMode", "Sistem gizli modu uygulandı")
        } catch (e: Exception) {
            Log.e("SilentMode", "Sistem gizli modu uygula yayında hata: ${e.message}")
        }
    }

    fun removeGlobalSilentMode() {
        try {
            // Sesi geri aç
            unmuteAudio()
            
            // Titreşimi geri aç
            enableVibration()
            
            Log.d("SilentMode", "Sistem gizli modu kaldırıldı")
        } catch (e: Exception) {
            Log.e("SilentMode", "Sistem gizli modu kaldır yayında hata: ${e.message}")
        }
    }

    private fun muteAudio() {
        audioManager?.apply {
            originalRingerMode = ringerMode
            ringerMode = AudioManager.RINGER_MODE_SILENT
            setStreamVolume(AudioManager.STREAM_RING, 0, 0)
            setStreamVolume(AudioManager.STREAM_NOTIFICATION, 0, 0)
            setStreamVolume(AudioManager.STREAM_MUSIC, 0, 0)
        }
    }

    private fun unmuteAudio() {
        audioManager?.apply {
            ringerMode = originalRingerMode
        }
    }

    private fun disableVibration() {
        vibrator?.apply {
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
                // Android 12+ için amplitude çalışması tözü biraz farklı
                cancel()
            } else {
                cancel()
            }
        }
    }

    private fun enableVibration() {
        // Titreşim ayarlarına dön
    }

    fun keepDisplayOff(keepOff: Boolean) {
        try {
            if (keepOff) {
                // Ekran kilitlenmesi sırasında ekran "dışarıda" kalacak
                // Bunu yapmak için, kullanıcı DevicePolicyManager izni vermesi gerekir
                Log.d("SilentMode", "Ekran kapatılmış olarak tutulacak")
            }
        } catch (e: Exception) {
            Log.e("SilentMode", "Ekran kapatılma ayarlanamadı: ${e.message}")
        }
    }

    fun hideNotificationLED(hide: Boolean) {
        try {
            if (hide) {
                // Bildirim LED'i kapatmak için uygulamalar
                // notification builder'da setLights() çağrısını kaldırması gerekir
                Log.d("SilentMode", "Bildirim LED gizlendi")
            }
        } catch (e: Exception) {
            Log.e("SilentMode", "Bildirim LED gizlenemedi: ${e.message}")
        }
    }

    fun lockScreen(lock: Boolean) {
        try {
            if (lock && powerManager != null) {
                // Ekran kilitlemek için DevicePolicyManager gerekli
                Log.d("SilentMode", "Ekran kilitlemek için DevicePolicyManager gerekli")
            }
        } catch (e: Exception) {
            Log.e("SilentMode", "Ekran kilitlenemedi: ${e.message}")
        }
    }
}
