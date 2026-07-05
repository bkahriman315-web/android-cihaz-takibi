# 📖 PhoneHub Pro - Kullanım Rehberi

## 📚 İçindekiler

1. [Başlangıç](#başlangıç)
2. [Cihaz Bağlantısı](#cihaz-bağlantısı)
3. [Gizli Mod Rehberi](#gizli-mod-rehberi)
4. [Ekran Görüntüsü](#ekran-görüntüsü)
5. [Dosya Yönetimi](#dosya-yönetimi)
6. [Konum İzlemesi](#konum-izlemesi)
7. [Bildirim Okuma](#bildirim-okuma)
8. [SSS](#sss)

---

## 🚀 Başlangıç

### Windows Uygulamasını Aç
```bash
cd frontend/PhoneHub.Desktop
dotnet run
```

### Giriş Yap
1. Email ve şifre gir
2. "Giriş Yap" butonuna tıkla
3. Ana ekrana yönlendirileceksin

---

## 📱 Cihaz Bağlantısı

### Adım 1: Android Uygulamasını Yükle
```bash
cd mobile/PhoneHub
./gradlew installDebug
```

### Adım 2: Telefonda Uygulamayı Aç
- Uygulama ikonuna tıkla
- Geri tuşu ile izinleri etkinleştir

### Adım 3: İzinleri Ver
- ✅ **Accessibility Service**: Kontrol için gerekli
- ✅ **Screen Capture**: Ekran görüntüsü için gerekli  
- ✅ **Location**: Konum izlemesi için gerekli
- ✅ **Notifications**: Bildirim okuma için gerekli
- ✅ **Storage**: Dosya yönetimi için gerekli
- ✅ **Camera**: Kamera erişimi için gerekli

### Adım 4: Cihazı Eşleştir
1. Windows uygulamasında "Cihazlar" sekmesi git
2. "Yeni Cihaz Ekle" butonuna tıkla
3. Telefonda gösterilen kodu gir
4. "Bağlan" butonuna tıkla

✅ Cihaz bağlandı!

---

## 🔐 Gizli Mod Rehberi

### Gizli Mod Nedir?

Gizli Mod, telefonda hiçbir izleme kaydı bırakmadan bilgisayardan kontrol etme özelliğidir.

| Özellik | Normal Mod | Gizli Mod |
|---------|-----------|----------|
| Ekran | Açık | Kapalı |
| Ses | Açık | Kapalı |
| Titreşim | Açık | Kapalı |
| LED | Yanıyor | Kapalı |
| Sistem Bildirimleri | Görülüyor | Gizli |
| Loglar | Tutulur | Silinir |
| Geçmiş | Kalır | Temizlenir |

### Gizli Modu Etkinleştir

#### 1. Windows Uygulamasında
```
Sol Menu > Gizli Mod > Cihaz Seç
```

#### 2. Ayarları Konfigüre Et

**Temel Ayarlar:**
- ☑️ Ekranı kapalı tut (KeepDisplayOff)
- ☑️ Sesi kapat (MuteAudio)
- ☑️ Titreşimi devre dışı bırak
- ☑️ Bildirim LED'ini kapat
- ☑️ Sistem bildirimlerini gizle

**Gelişmiş Ayarlar:**
- ☑️ Logları devre dışı bırak
- ☑️ Oturumdan sonra geçmişi temizle

#### 3. PIN Koruma (Opsiyonel)
```
PIN Koruma > PIN Kodu Gir > Gizli Modu Etkinleştir
```

#### 4. "Gizli Modu Etkinleştir" Butonuna Tıkla

✅ Gizli Mod aktif! Telefonda:
- Ekran kapanacak
- Ses susturulacak
- Hiçbir bildirim görülmeyecek

### Gizli Modda Kontrol Et

#### Dokunma
```
Ekran Simülasyonu > Dokunma > İstediğin Konumu Seç
```

#### Metin Yazma
```
Ekran Simülasyonu > Metin Yazma > Metni Gir > Yazı
```

#### Uygulama Açma
```
Uygulamalar > Uygulamayı Seç > Aç
```

#### Kaydırma
```
Ekran Simülasyonu > Kaydırma > Yönü Seç (Yukarı/Aşağı)
```

### Gizli Modu Kapat

#### 1. "Gizli Modu Devre Dışı Bırak" Butonuna Tıkla

#### 2. PIN Girerse İste
```
PIN: <Ayarlarken girdiğin PIN>
```

#### 3. Telefon Normal Moduna Dön
- Ekran açılacak
- Ses açılacak
- Geçmiş temizlenecek (Ayarlanmışsa)

✅ Gizli Mod devre dışı!

### Gizli Mod Oturum Geçmişi

1. "Oturum Geçmişi" sekmesine git
2. Tarih aralığı seç
3. İşlemleri görüntüle:
   - Başlama Tarihi
   - Bitiş Tarihi
   - Yapılan İşlem Sayısı
   - Süre

---

## 📷 Ekran Görüntüsü

### Canlı Ekran İzleme
```
Ekran Görüntüsü > Cihaz Seç > Başlat
```

**Özellikler:**
- 15 FPS (Varsayılan)
- 30 FPS (Yüksek Kalite)
- Otomatik sıkıştırma
- Şifreleme

### Ekran Kaydı
```
Kamera > Ekran Kaydı > Başlat > ... > Durdur
```

**Kaydetme Seçenekleri:**
- 720p (Varsayılan)
- 1080p (Yüksek Kalite)
- 4K (Ultra Kalite)

### Kamera Erişimi

#### Ön Kamera
```
Kamera > Ön Kamera > Canlı Görüntü Al
```

#### Arka Kamera
```
Kamera > Arka Kamera > Canlı Görüntü Al
```

---

## 📂 Dosya Yönetimi

### Klasörler
- 📁 DCIM (Fotoğraflar)
- 📁 Download (İndirilenler)
- 📁 Pictures (Resimler)
- 📁 Movies (Videolar)
- 📁 Documents (Belgeler)

### Dosya İndirme
```
Dosya Yöneticisi > Klasörü Seç > Dosya > İndir
```

### Dosya Yükleme
```
Dosya Yöneticisi > Klasörü Seç > Dosya Yükle > Dosyayı Seç
```

### Dosya Silme
```
Dosya Yöneticisi > Dosya > Sağ Tıkla > Sil
```

---

## 🗺️ Konum İzlemesi

### Canlı Konum
```
Konum > Cihaz Seç > Canlı Konum
```

**Görülecekler:**
- 📍 Anlık konum (GPS)
- 🛰️ Uydu görüntüsü
- 🗺️ Harita gösterimi
- ⚙️ Doğruluk: ~5-10m

### Konum Geçmişi
```
Konum > Geçmiş > Tarih Aralığı Seç
```

**İçerik:**
- Zaman: Saat, dakika
- Konum: Enlem, Boylam
- Adres: Sokak, Şehir
- Hız: km/h

### Rotalar
```
Konum > Rotalar > Tarih Seç
```

**Gösterilecekler:**
- Hareket yolunu
- Durakları
- Toplam mesafe
- Ortalama hız

### Geofencing (Coğrafi Çit)
```
Konum > Geofencing > Yeni Çit > Merkez Seç > Yarıçap Gir > Kaydet
```

**Bildirimleri:**
- Alan dışına çıkıldı
- Alan içine girildi
- Zaman: Ne zaman

---

## 📢 Bildirim Okuma

### Desteklenen Uygulamalar
- 💬 WhatsApp
- 📱 Telegram
- 📷 Instagram
- 💌 Gmail
- 📧 Outlook
- 📲 SMS
- 🔔 Sistem Bildirimleri

### Bildiriminleri Görüntüle
```
Bildirimler > Uygulamayı Seç > Bildirimleri Listele
```

### Bildirim Detayları
- 👤 Gönderen
- 📝 İçerik
- ⏰ Alış Tarihi
- 📱 Uygulama

### Bildirim Filtreleme
```
Bildirimler > Filtrele > Uygulama > Tarih > Ara
```

---

## ❓ SSS

### S1: Gizli Mod gerçekten gizli mi?
**C:** Evet! Ekran kapanır, ses kapalı kalır, hiçbir bildirim gösterilmez, loglar silinir.

### S2: Gizli Modu Kapatırsam izleri kalacak mı?
**C:** Hayır. "Geçmişi temizle" seçeneğini etkinleştirmişsen, tüm izler silinir.

### S3: PIN'i unuttum ne yapayım?
**C:** Telefonda Ayarlar > PhoneHub > Gizli Mod > PIN Sıfırla

### S4: İnternet kesintisi olursa ne olur?
**C:** Gizli Mod otomatik olarak offline mode'a geçer. Komutlar kuyruğa alınır.

### S5: Birden fazla telefon bağlayabilir miyim?
**C:** Evet! Her telefon ayrı cihaz olarak kaydedilir.

### S6: Gizli Mod'u PIN olmadan kullanabilir miyim?
**C:** Evet. PIN isteğe bağlıdır. Boş bırakabilirsin.

### S7: Telefon kapalıyken Gizli Mod çalışır mı?
**C:** Hayır. Telefon açık olması gerekir.

### S8: Sunucu down olursa ne olur?
**C:** Bağlantı kopacak. Sunucu aktif olunca yeniden bağlanacak.

### S9: Verilerim güvenli mi?
**C:** Evet! Tüm veriler AES-256 ile şifrelenmiştir.

### S10: Başka birisi Gizli Modu etkinleştirebilir mi?
**C:** Hayır. JWT token ile korumalı. Başka biri PIN'i bilemezse etkinleştiremez.

---

## 📞 Yardım Gerekli?

- ❓ [Issues Sekmesi](https://github.com/bkahriman315-web/android-cihaz-takibi/issues)
- 📧 Email: bkahriman315@gmail.com
- 💬 GitHub Discussions

---

**İyi Kullanımlar! 🎉**
