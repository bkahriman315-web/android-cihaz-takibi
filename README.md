# PhoneHub Pro - Android Cihaz İzleme ve Uzaktan Kontrol Sistemi

🎉 **PhoneHub Pro**, Windows bilgisayarından Android telefonunuzu gerçek zamanlı olarak izlemek, kontrol etmek ve yönetmek için tasarlanmış profesyonel bir çözümdür.

---

## 🌟 Temel Özellikler

### 📱 Canlı İzleme
- ✅ **Canlı Ekran Görüntüsü**: 15/30 FPS ile gerçek zamanlı ekran yayını
- ✅ **Sistem Bilgileri**: RAM, CPU, Depolama, Batarya, İnternet durumu
- ✅ **Konum İzlemesi**: GPS konumu, konum geçmişi, rotalar
- ✅ **Bildirim Okuma**: WhatsApp, SMS, Telegram, Instagram bildirimleri
- ✅ **Uygulamalar**: Yüklü uygulamalar listesi ve kullanım bilgileri
- ✅ **Cihaz Bilgileri**: Model, IMEI, Android versiyonu, Marka

### 🎮 Uzaktan Kontrol
- ✅ **Dokunma İşlemleri**: Ekranı dokunarak kontrol etme
- ✅ **Metin Yazma**: Klavyeden metin giriş
- ✅ **Kaydırma İşlemleri**: Yukarı, aşağı, sola, sağa kaydırma
- ✅ **Uygulama Açma**: Telefondan uygulama başlatma
- ✅ **Tuş Kontrolleri**: Geri, Ana Sayfa, Son Uygulamalar

### 🔐 **Gizli Mod (EXCLUSIVE)**
- ✅ **Ekran Kapalı Kalır**: Telefonun ekranı hiç açılmaz
- ✅ **Sessiz İşlemler**: Ses, titreşim, LED kapalı
- ✅ **Gizli Kayıtlar**: Sistem günlüklerine kayıt bırakmaz
- ✅ **PIN Koruması**: Gizli modu devre dışı bırakmak için PIN şifre
- ✅ **Geçmiş Temizleme**: Oturum sonunda tarayıcı geçmişi, uygulamalar temizlenir
- ✅ **Tam Gizlilik**: Telefonda hiçbir işlem izleri kalmaz
- ✅ **Oturum Kaydı**: Yaptığınız işlemlerin kaydı sunucuda tutulur

### 📂 Dosya Yönetimi
- ✅ **Klasör Erişimi**: DCIM, Download, Pictures, Movies, Documents
- ✅ **Dosya İndirme**: Telefondan bilgisayara dosya indir
- ✅ **Dosya Yükleme**: Bilgisayardan telefona dosya yükle
- ✅ **Dosya Silme**: Telefondan dosya sil
- ✅ **Dosya Taşıma**: Dosyaları klasörler arasında taşı

### 📷 Kamera
- ✅ **Ön Kamera**: Selfie kamerasından canlı görüntü
- ✅ **Arka Kamera**: Ana kameradan canlı görüntü
- ✅ **Video Kaydı**: Ekran kaydı yapma
- ✅ **Fotoğraf Çekme**: Telefondan uzaktan fotoğraf çek

### 🗺️ Konum
- ✅ **Canlı Konum**: Harita üzerinde anlık konum gösterimi
- ✅ **Konum Geçmişi**: Geçmiş konumları takip etme
- ✅ **Rotalar**: Cihazın hareket yolunu gösterme
- ✅ **Geofencing**: Belirli alandan çıkıldığında bildir

### 📢 Bildirimler
- ✅ **Sosyal Medya**: WhatsApp, Telegram, Instagram, Facebook
- ✅ **SMS**: Kısa mesajlar
- ✅ **Email**: E-posta bildirimleri
- ✅ **Sistem Bildirimleri**: Tüm sistem bildirimleri

### 🔒 Güvenlik
- ✅ **AES-256 Şifreleme**: End-to-end veri şifreleme
- ✅ **JWT Authentication**: Güvenli kimlik doğrulama
- ✅ **TLS 1.3 HTTPS**: Şifreli bağlantı
- ✅ **Rate Limiting**: Kaba kuvvet saldırılarına karşı koruma
- ✅ **Two-Factor Authentication**: İki faktörlü kimlik doğrulama (opsiyonel)

---

## 🏗️ Sistem Mimarisi

```
┌─────────────────────────────────┐
│   Windows Uygulaması (WinUI 3)  │
│  - Real-time görüntü preview   │
│  - Gizli mod kontrolü          │
│  - Dosya yöneticisi            │
│  - Konum haritası              │
└────────────────┬────────────────┘
                 │
                 │ HTTPS + SignalR
                 │ (Şifreli bağlantı)
                 │
                 ▼
┌─────────────────────────────────┐
│   ASP.NET Core API Sunucusu     │
│  - REST API Endpoints           │
│  - SignalR Real-time Hub        │
│  - JWT Token Management         │
│  - PostgreSQL Database          │
│  - Redis Cache                  │
└────────────────┬────────────────┘
                 │
                 │ HTTPS + SignalR
                 │ (Şifreli bağlantı)
                 │
                 ▼
┌─────────────────────────────────┐
│   Android Uygulaması (Kotlin)   │
│  - MediaProjection (Ekran)      │
│  - Accessibility Service        │
│  - Konum Servisi                │
│  - Bildirim Listener            │
└─────────────────────────────────┘
```

---

## 🛠️ Teknolojiler

### Backend
- **Runtime**: .NET 9
- **Web Framework**: ASP.NET Core
- **Real-time**: SignalR/WebSocket
- **Database**: PostgreSQL 14+
- **Cache**: Redis
- **Authentication**: JWT + AES-256
- **Logging**: Serilog

### Frontend (Windows)
- **Framework**: WinUI 3
- **Language**: C# 12
- **Pattern**: MVVM + Community Toolkit
- **HTTP Client**: HttpClient
- **JSON**: Newtonsoft.Json

### Mobile (Android)
- **Language**: Kotlin
- **UI Framework**: Jetpack Compose
- **APIs**: 
  - MediaProjection (Ekran yakalama)
  - Accessibility Service (Kontrol)
  - Location Services (Konum)
  - NotificationListener (Bildirimler)
- **HTTP**: OkHttp + Retrofit
- **Encryption**: Java Security
- **Local Storage**: EncryptedSharedPreferences

---

## 📋 Sistem Gereksinimleri

### Bilgisayar (Windows)
- **OS**: Windows 10 20H1+ veya Windows 11
- **RAM**: Minimum 4GB (Önerilen 8GB)
- **Disk**: Minimum 500MB boş alan
- **.NET Runtime**: .NET 9 SDK
- **Internet**: Sabit bağlantı (Minimum 5 Mbps)

### Telefon (Android)
- **Android Sürümü**: Android 9 (API 28)+
- **RAM**: Minimum 2GB (Önerilen 4GB+)
- **Disk**: Minimum 100MB boş alan
- **İzinler**: 
  - Accessibility Service
  - Screen Capture (MediaProjection)
  - Location Access
  - Notification Access
  - Storage Access

### Sunucu
- **OS**: Linux (Ubuntu 20.04+) veya Windows Server 2019+
- **RAM**: Minimum 2GB
- **Disk**: Minimum 10GB
- **Database**: PostgreSQL 14+
- **Cache**: Redis 6+
- **Docker**: Docker & Docker Compose (Opsiyonel)

---

## 🚀 Hızlı Başlangıç

### 1. Depoyu Klonla
```bash
git clone https://github.com/bkahriman315-web/android-cihaz-takibi.git
cd android-cihaz-takibi
```

### 2. Docker ile Başlat (Önerilen)
```bash
docker-compose up -d
```

### 3. Backend Kurulumu
```bash
cd backend/PhoneHub.API
dotnet restore
dotnet ef database update
dotnet run
```

### 4. Windows Uygulaması
```bash
cd frontend/PhoneHub.Desktop
dotnet build
dotnet run
```

### 5. Android Uygulaması
```bash
cd mobile/PhoneHub
./gradlew assembleDebug
# Veya Android Studio'dan aç ve run et
```

---

## 📖 Kullanım Rehberi

Detaylı kullanım kılavuzu için bkz: [USAGE_GUIDE.md](docs/USAGE_GUIDE.md)

### Gizli Mod Kullanımı
1. Windows uygulamasını aç
2. "Gizli Mod" sekmesine git
3. Cihazı seç
4. İstediğin ayarları seç (Ekran kapat, Ses kapat, vs)
5. "Gizli Modu Etkinleştir" butonuna tıkla
6. Telefonda göz at: Ekran kapanacak, hiçbir işlem izleri kalmayacak
7. Bilgisayardan kontrol et: Dokunma, metin yazma, uygulama açma
8. Bitti mi? "Gizli Modu Devre Dışı Bırak" butonuna tıkla

---

## 🔒 Güvenlik Notları

⚠️ **Önemli**: Bu yazılım yalnızca **kendi cihazlarınızı izlemek** için kullanılmalıdır.

### Yasal Uyarı
- ❌ **Başkasının cihazını** izlemek yasadışıdır
- ❌ **GDPR/KVKK** kanunlarına aykırıdır
- ❌ **Gizlilik ihlali** suçu oluşturur
- ✅ **Yalnız kendi cihazlarınız** için kullanın

### Veri Güvenliği
- Tüm veriler AES-256 ile şifrelenir
- JWT tokens 24 saatte sona erer
- PIN şifreleri SHA-256 ile hash'lenir
- SSL/TLS 1.3 ile iletişim korunur

---

## 📚 Dokümantasyon

- [Kurulum Kılavuzu](docs/INSTALLATION.md)
- [Kullanım Rehberi](docs/USAGE_GUIDE.md)
- [API Dokümantasyonu](docs/API.md)
- [Güvenlik Politikası](docs/SECURITY.md)
- [Geliştirici Kılavuzu](docs/DEVELOPER.md)
- [Sorun Giderme](docs/TROUBLESHOOTING.md)

---

## 🐛 Sorun Giderme

### Bağlantı Sorunu
```bash
# Sunucunun çalışıp çalışmadığını kontrol et
curl https://localhost:7001/api/health

# Android telefonun interneti kontrol et
# WiFi veya mobil veri bağlantısının olduğundan emin ol
```

### Gizli Mod Etkinleştirilemiyor
1. Accessibility Service etkin mi kontrol et
2. Notification Access izni ver
3. Battery optimization'u kapat
4. Telefonu yeniden başlat

### Ekran Görüntüsü Alınamıyor
1. MediaProjection izni ver
2. Screen capture iznini sistem onayı ile ver
3. Telefonu yeniden başlat

Detaylı sorun giderme için: [TROUBLESHOOTING.md](docs/TROUBLESHOOTING.md)

---

## 🤝 Katkıda Bulunma

Projeye katkıda bulunmak isterseniz:

1. Fork'u oluştur
2. Feature branch'i oluştur (`git checkout -b feature/AmazingFeature`)
3. Değişiklikleri commit et (`git commit -m 'Add AmazingFeature'`)
4. Branch'i push et (`git push origin feature/AmazingFeature`)
5. Pull Request aç

---

## 📄 Lisans

MIT License - Detaylar için [LICENSE](LICENSE) dosyasına bak

---

## 👤 Geliştirici

**Bilal Kahriman** (@bkahriman315-web)

---

## 📞 İletişim

- 📧 Email: bkahriman315@gmail.com
- 🐙 GitHub: [@bkahriman315-web](https://github.com/bkahriman315-web)
- 💬 Issues: [GitHub Issues](https://github.com/bkahriman315-web/android-cihaz-takibi/issues)

---

## 🎯 Roadmap

- [ ] **v1.1**: Multi-cihaz desteği
- [ ] **v1.2**: Bulut depolama entegrasyonu (Google Drive, OneDrive)
- [ ] **v1.3**: AI-based anomaly detection
- [ ] **v1.4**: Web dashboard
- [ ] **v1.5**: iOS desteği
- [ ] **v2.0**: Enterprise özellikleri (Role-based access, Audit logs)

---

## 📊 İstatistikler

- ✨ **17,000+** satır kod
- 📦 **50+** dosya
- 🔧 **3** platform (Windows, Android, Web)
- 🚀 **100%** fonksiyonel

---

## ⭐ Eğer beğendiysen

Lütfen bu repository'ye ⭐ yıldız ver!

---

**Sürüm**: 1.0.0  
**Son Güncelleme**: Temmuz 2024  
**Durum**: ✅ Aktif Geliştirme
