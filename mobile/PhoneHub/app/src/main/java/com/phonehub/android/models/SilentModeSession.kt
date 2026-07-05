package com.phonehub.android.models

import android.os.Parcelable
import kotlinx.parcelize.Parcelize
import java.util.*

/**
 * Gizli Mod Oturumu Modeli
 */
@Parcelize
data class SilentModeSession(
    val id: String = UUID.randomUUID().toString(),
    val deviceId: String = "",
    val userId: String = "",
    val startedAt: Long = 0,
    val endedAt: Long? = null,
    val operationCount: Int = 0,
    val operations: MutableList<String> = mutableListOf(),
    val ipAddress: String? = null
) : Parcelable
