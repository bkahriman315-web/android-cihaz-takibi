package com.phonehub.android.services.silentmode

import android.content.Context
import android.content.SharedPreferences
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import com.phonehub.android.models.SilentModeSettings
import java.security.MessageDigest
import java.util.*

/**
 * Gizli Mod Ayarlarını yerel olarak yönetmek için SharedPreferences kullanır
 */
class SilentModeLocalStorage(context: Context) {

    private val masterKey = MasterKey.Builder(context)
        .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
        .build()

    private val sharedPreferences: SharedPreferences = EncryptedSharedPreferences.create(
        context,
        "silent_mode_settings",
        masterKey,
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )

    fun saveSilentModeSettings(settings: SilentModeSettings) {
        sharedPreferences.edit().apply {
            putString("device_id", settings.deviceId)
            putBoolean("is_enabled", settings.isEnabled)
            putBoolean("keep_display_off", settings.keepDisplayOff)
            putBoolean("mute_audio", settings.muteAudio)
            putBoolean("disable_vibration", settings.disableVibration)
            putBoolean("disable_notification_led", settings.disableNotificationLED)
            putBoolean("hide_system_notifications", settings.hideSystemNotifications)
            putBoolean("disable_logs", settings.disableLogs)
            putBoolean("clear_history_after_session", settings.clearHistoryAfterSession)
            putString("pin_hash", settings.pinHash)
            putLong("enabled_at", settings.enabledAt)
            putLong("updated_at", settings.updatedAt)
            apply()
        }
    }

    fun getSilentModeSettings(): SilentModeSettings {
        return SilentModeSettings(
            deviceId = sharedPreferences.getString("device_id", "") ?: "",
            isEnabled = sharedPreferences.getBoolean("is_enabled", false),
            keepDisplayOff = sharedPreferences.getBoolean("keep_display_off", true),
            muteAudio = sharedPreferences.getBoolean("mute_audio", true),
            disableVibration = sharedPreferences.getBoolean("disable_vibration", true),
            disableNotificationLED = sharedPreferences.getBoolean("disable_notification_led", true),
            hideSystemNotifications = sharedPreferences.getBoolean("hide_system_notifications", true),
            disableLogs = sharedPreferences.getBoolean("disable_logs", true),
            clearHistoryAfterSession = sharedPreferences.getBoolean("clear_history_after_session", true),
            pinHash = sharedPreferences.getString("pin_hash", null),
            enabledAt = sharedPreferences.getLong("enabled_at", 0),
            updatedAt = sharedPreferences.getLong("updated_at", 0)
        )
    }

    fun isEnabled(): Boolean {
        return sharedPreferences.getBoolean("is_enabled", false)
    }

    fun setEnabled(enabled: Boolean) {
        sharedPreferences.edit().putBoolean("is_enabled", enabled).apply()
    }

    fun setPINHash(pinHash: String) {
        sharedPreferences.edit().putString("pin_hash", pinHash).apply()
    }

    fun getPINHash(): String? {
        return sharedPreferences.getString("pin_hash", null)
    }

    fun clearAllSettings() {
        sharedPreferences.edit().clear().apply()
    }

    fun hashPIN(pin: String): String {
        return MessageDigest.getInstance("SHA-256")
            .digest(pin.toByteArray())
            .fold("") { str, it -> str + "%02x".format(it) }
    }

    fun verifyPIN(inputPin: String, storedHash: String?): Boolean {
        if (storedHash == null || inputPin.isEmpty()) return false
        return hashPIN(inputPin) == storedHash
    }
}
