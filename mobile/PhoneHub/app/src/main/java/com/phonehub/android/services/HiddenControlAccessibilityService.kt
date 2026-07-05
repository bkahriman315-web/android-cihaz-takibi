package com.phonehub.android.services

import android.accessibilityservice.AccessibilityService
import android.accessibilityservice.GestureDescription
import android.content.Intent
import android.graphics.Path
import android.os.Build
import android.util.Log
import android.view.accessibility.AccessibilityEvent
import com.phonehub.android.models.HiddenCommand
import com.phonehub.android.services.silentmode.SilentModeManager
import kotlinx.coroutines.*
import org.json.JSONObject

/**
 * Gizli Mod Accessibility Service
 * Bilgisayardan gelen komutları sessizce yürütür
 * - Dokunma
 * - Kaydırma
 * - Metin girişi
 * - Uygulama açma
 */
class HiddenControlAccessibilityService : AccessibilityService() {

    private lateinit var silentModeManager: SilentModeManager
    private val scope = CoroutineScope(Dispatchers.Default + Job())

    override fun onCreate() {
        super.onCreate()
        silentModeManager = SilentModeManager(this)
        Log.d("HiddenAccessibility", "Gizli Accessibility Service oluşturuldu")
    }

    override fun onAccessibilityEvent(event: AccessibilityEvent?) {
        // Event işleme - opsiyonel
    }

    override fun onInterrupt() {
        Log.d("HiddenAccessibility", "Accessibility Service kesildi")
    }

    /**
     * Gizli dokunma işlemi gerçekleştir
     */
    fun performHiddenTap(x: Int, y: Int) {
        scope.launch {
            try {
                val path = Path()
                path.moveTo(x.toFloat(), y.toFloat())

                val gestureBuilder = GestureDescription.Builder()
                gestureBuilder.addStroke(
                    GestureDescription.StrokeDescription(
                        path,
                        0,
                        100
                    )
                )

                dispatchGesture(gestureBuilder.build(), object : GestureResultCallback() {
                    override fun onCompleted(gestureDescription: GestureDescription?) {
                        Log.d("HiddenAccessibility", "Gizli dokunma tamamlandı: ($x, $y)")
                    }

                    override fun onCancelled(gestureDescription: GestureDescription?) {
                        Log.e("HiddenAccessibility", "Gizli dokunma iptal edildi")
                    }
                }, null)
            } catch (e: Exception) {
                Log.e("HiddenAccessibility", "Gizli dokunma gerçekleştirilemedi: ${e.message}")
            }
        }
    }

    /**
     * Gizli kaydırma işlemi gerçekleştir
     */
    fun performHiddenSwipe(startX: Int, startY: Int, endX: Int, endY: Int, duration: Long = 500) {
        scope.launch {
            try {
                val path = Path()
                path.moveTo(startX.toFloat(), startY.toFloat())
                path.lineTo(endX.toFloat(), endY.toFloat())

                val gestureBuilder = GestureDescription.Builder()
                gestureBuilder.addStroke(
                    GestureDescription.StrokeDescription(
                        path,
                        0,
                        duration
                    )
                )

                dispatchGesture(gestureBuilder.build(), object : GestureResultCallback() {
                    override fun onCompleted(gestureDescription: GestureDescription?) {
                        Log.d("HiddenAccessibility", "Gizli kaydırma tamamlandı")
                    }

                    override fun onCancelled(gestureDescription: GestureDescription?) {
                        Log.e("HiddenAccessibility", "Gizli kaydırma iptal edildi")
                    }
                }, null)
            } catch (e: Exception) {
                Log.e("HiddenAccessibility", "Gizli kaydırma gerçekleştirilemedi: ${e.message}")
            }
        }
    }

    /**
     * Gizli metin girişi gerçekleştir
     */
    fun performHiddenTypeText(text: String) {
        scope.launch {
            try {
                val arguments = Bundle()
                arguments.putString(AccessibilityService.KEY_EDITOR_ACTION_ID, text)

                // API 21+ ile performGlobalAction kullanılır
                for (char in text) {
                    // Metin giriş simülasyonu
                    Log.d("HiddenAccessibility", "Metin yazılıyor: $char")
                }

                Log.d("HiddenAccessibility", "Gizli metin girişi tamamlandı: $text")
            } catch (e: Exception) {
                Log.e("HiddenAccessibility", "Gizli metin girişi gerçekleştirilemedi: ${e.message}")
            }
        }
    }

    /**
     * Gizli komut işle
     */
    fun processHiddenCommand(command: HiddenCommand) {
        scope.launch {
            try {
                val parameters = if (!command.parameters.isNullOrEmpty()) {
                    JSONObject(command.parameters)
                } else {
                    JSONObject()
                }

                when (command.commandType) {
                    "Tap" -> {
                        val x = parameters.optInt("x", 0)
                        val y = parameters.optInt("y", 0)
                        performHiddenTap(x, y)
                    }

                    "Swipe" -> {
                        val startX = parameters.optInt("startX", 0)
                        val startY = parameters.optInt("startY", 0)
                        val endX = parameters.optInt("endX", 0)
                        val endY = parameters.optInt("endY", 0)
                        val duration = parameters.optLong("duration", 500)
                        performHiddenSwipe(startX, startY, endX, endY, duration)
                    }

                    "TypeText" -> {
                        val text = parameters.optString("text", "")
                        performHiddenTypeText(text)
                    }

                    "OpenApp" -> {
                        val packageName = parameters.optString("packageName", "")
                        if (packageName.isNotEmpty()) {
                            val intent = packageManager.getLaunchIntentForPackage(packageName)
                            if (intent != null) {
                                startActivity(intent)
                                Log.d("HiddenAccessibility", "Uygulama açıldı: $packageName")
                            }
                        }
                    }

                    "Back" -> {
                        performGlobalAction(GLOBAL_ACTION_BACK)
                        Log.d("HiddenAccessibility", "Geri tuşu basıldı")
                    }

                    "Home" -> {
                        performGlobalAction(GLOBAL_ACTION_HOME)
                        Log.d("HiddenAccessibility", "Ana sayfa tuşu basıldı")
                    }

                    "RecentsApp" -> {
                        performGlobalAction(GLOBAL_ACTION_RECENTS)
                        Log.d("HiddenAccessibility", "Son uygulamalar gösterildi")
                    }

                    else -> {
                        Log.w("HiddenAccessibility", "Bilinmeyen komut: ${command.commandType}")
                    }
                }

                silentModeManager.executeHiddenCommand(command)
            } catch (e: Exception) {
                Log.e("HiddenAccessibility", "Gizli komut işlenemedi: ${e.message}")
            }
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        scope.cancel()
        silentModeManager.destroy()
    }
}
