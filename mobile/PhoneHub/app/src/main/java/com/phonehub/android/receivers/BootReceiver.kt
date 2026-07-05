package com.phonehub.android.receivers

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.util.Log
import com.phonehub.android.services.SilentModeBackgroundService

/**
 * Boot Completed Alıcısı
 * Cihaz açıldığında gizli mod hizmetini yeniden başlatır
 */
class BootReceiver : BroadcastReceiver() {

    override fun onReceive(context: Context?, intent: Intent?) {
        if (intent?.action == Intent.ACTION_BOOT_COMPLETED) {
            Log.d("BootReceiver", "Cihaz açıldı, gizli mod servisi başlatılıyor")

            if (context != null) {
                val serviceIntent = Intent(context, SilentModeBackgroundService::class.java)
                try {
                    context.startService(serviceIntent)
                    Log.d("BootReceiver", "Gizli mod servisi başlatıldı")
                } catch (e: Exception) {
                    Log.e("BootReceiver", "Gizli mod servisi başlatılamadı: ${e.message}")
                }
            }
        }
    }
}
