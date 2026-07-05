package com.phonehub.android.receivers

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.net.ConnectivityManager
import android.util.Log

/**
 * Ağ Durumu Değişim Alıcısı
 * İnternet bağlantısı değiştiğinde gizli mode uygun şekilde yanıt verir
 */
class NetworkChangeReceiver : BroadcastReceiver() {

    override fun onReceive(context: Context?, intent: Intent?) {
        if (context == null) return

        val connectivityManager = context.getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        val activeNetwork = connectivityManager.activeNetwork

        if (activeNetwork != null) {
            Log.d("NetworkChange", "Ağ bağlantısı sağlandı")
            // Gizli mod servisi ile senkronize et
        } else {
            Log.d("NetworkChange", "Ağ bağlantısı kaybedildi")
            // Offline mod'a geç, çevrimdışı komutları kuyruğa al
        }
    }
}
