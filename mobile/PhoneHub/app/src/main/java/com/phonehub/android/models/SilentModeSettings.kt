package com.phonehub.android.models

import android.os.Parcelable
import kotlinx.parcelize.Parcelize
import java.util.*

/**
 * Gizli Mod Ayarları Modeli
 */
@Parcelize
data class SilentModeSettings(
    val id: String = UUID.randomUUID().toString(),
    val deviceId: String = "",
    val isEnabled: Boolean = false,
    val keepDisplayOff: Boolean = true,
    val muteAudio: Boolean = true,
    val disableVibration: Boolean = true,
    val disableNotificationLED: Boolean = true,
    val hideSystemNotifications: Boolean = true,
    val disableLogs: Boolean = true,
    val clearHistoryAfterSession: Boolean = true,
    val pinHash: String? = null,
    val enabledAt: Long = 0,
    val updatedAt: Long = 0
) : Parcelable
