package com.phonehub.android.models

import android.os.Parcelable
import kotlinx.parcelize.Parcelize
import java.util.*

/**
 * Gizli Komut Modeli
 */
@Parcelize
data class HiddenCommand(
    val id: String = UUID.randomUUID().toString(),
    val deviceId: String = "",
    val sessionId: String = "",
    val commandType: String = "", // Tap, Swipe, TypeText, OpenApp, DeleteFile, etc.
    val parameters: String? = null,
    val isExecuted: Boolean = false,
    val executedAt: Long? = null,
    val errorMessage: String? = null,
    val createdAt: Long = System.currentTimeMillis()
) : Parcelable
