# 📖 PhoneHub Pro - Kurulum Kılavuzu

## 📋 İçindekiler

1. [Ön Gereksinimler](#ön-gereksinimler)
2. [Adım 1: Backend Kurulumu](#adım-1-backend-kurulumu)
3. [Adım 2: Windows Uygulaması](#adım-2-windows-uygulaması)
4. [Adım 3: Android Uygulaması](#adım-3-android-uygulaması)
5. [Adım 4: Docker Kurulumu (Opsiyonel)](#adım-4-docker-kurulumu-opsiyonel)
6. [Adım 5: İlk Kurulum](#adım-5-ilk-kurulum)
7. [Sorun Giderme](#sorun-giderme)

---

## ✅ Ön Gereksinimler

### Windows Bilgisayar
- Windows 10 20H1+ veya Windows 11
- 4GB+ RAM
- 500MB+ boş disk alanı
- .NET 9 SDK ([İndir](https://dotnet.microsoft.com/download/dotnet/9.0))
- Git ([İndir](https://git-scm.com))
- Visual Studio 2022 veya VS Code (Opsiyonel)

### PostgreSQL Sunucu
```bash
# Windows üzerinde PostgreSQL yükle
# https://www.postgresql.org/download/windows/

# Veya Docker ile:
docker run -d \
  --name postgres-phonehub \
  -e POSTGRES_DB=phonehub_db \
  -e POSTGRES_USER=phonehub_user \
  -e POSTGRES_PASSWORD=PhoneHub@123456 \
  -p 5432:5432 \
  postgres:16-alpine
```

### Redis Cache (Opsiyonel)
```bash
# Docker ile:
docker run -d \
  --name redis-phonehub \
  -p 6379:6379 \
  redis:7-alpine
```

### Android Telefon
- Android 9 (API 28)+
- 2GB+ RAM
- 100MB+ boş disk alanı
- Geliştirici modunun açık olması

---

## 🔧 Adım 1: Backend Kurulumu

### 1.1 Depoyu Klonla
```bash
git clone https://github.com/bkahriman315-web/android-cihaz-takibi.git
cd android-cihaz-takibi
```

### 1.2 Backend Klasörüne Git
```bash
cd backend/PhoneHub.API
```

### 1.3 Bağımlılıkları Yükle
```bash
dotnet restore
```

### 1.4 Ortam Ayarlarını Konfigüre Et

**appsettings.json** dosyasını oluştur:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=phonehub_db;Username=phonehub_user;Password=PhoneHub@123456"
  },
  "Jwt": {
    "Secret": "SuperSecretKeyThatIsAtLeast32CharactersLongForHS256Encryption!",
    "Issuer": "PhoneHub.API",
    "Audience": "PhoneHub.Client",
    "ExpirationMinutes": 1440
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "Redis": {
    "Connection": "localhost:6379"
  }
}
```

### 1.5 Database Migrations
```bash
# Migration'ları uygula
dotnet ef database update

# Başarılı mı kontrol et:
# PostgreSQL'e bağlan ve tabloları gör
```

### 1.6 Backend Uygulamasını Çalıştır
```bash
dotnet run

# Başarılı çalışırsa şu mesajı göreceksin:
# Now listening on: https://localhost:7001
# Now listening on: http://localhost:5000
```

### 1.7 API'yi Test Et
```bash
# Terminal/PowerShell'de:
curl https://localhost:7001/swagger

# Veya tarayıcıda:
# https://localhost:7001/swagger/index.html
```

✅ Backend hazır!

---

## 🪟 Adım 2: Windows Uygulaması

### 2.1 WinUI 3 Uzantılarını Yükle
```bash
# Visual Studio 2022 kurulu mu?
# Şuradan indir: https://visualstudio.microsoft.com/downloads/

# Kurulum sırasında şunları seç:
# - .NET Desktop Development
# - Windows App SDK (WinUI 3)
```

### 2.2 Windows Uygulaması Klasörüne Git
```bash
cd frontend/PhoneHub.Desktop
```

### 2.3 Bağımlılıkları Yükle
```bash
dotnet restore
```

### 2.4 Uygulamayı Çalıştır
```bash
dotnet run

# Veya Visual Studio'da:
# PhoneHub.Desktop.sln dosyasını aç ve Run'a tıkla
```

### 2.5 API Sunucusu Adresi Ayarla

**SilentModeViewModel.cs** dosyasında:
```csharp
private readonly string _baseUrl = "https://localhost:7001"; // Kendi sunucunun adresi
```

✅ Windows uygulaması hazır!

---

## 📱 Adım 3: Android Uygulaması

### 3.1 Android Studio'yu Yükle
[İndir](https://developer.android.com/studio)

### 3.2 Android Klasörüne Git
```bash
cd mobile/PhoneHub
```

### 3.3 Gradle Senkronizasyonu
```bash
# Android Studio'da aç:
# PhoneHub klasörünü aç
# "Sync Now" butonuna tıkla
```

### 3.4 APK Oluştur
```bash
# Terminal'de:
./gradlew assembleDebug

# Veya Android Studio'da:
# Build > Build Bundle(s) / APK(s) > Build APK(s)
```

### 3.5 Telefona Yükle
```bash
# Telefonu USB ile bilgisayara bağla
# Geliştirici modunu aç: Ayarlar > Hakkında > Build numarası (7 kez tıkla)

# Komutu çalıştır:
./gradlew installDebug

# Veya Android Studio'da:
# Run > Select Device > OK
```

### 3.6 Telefonda İzinleri Ver

Uygulama açıldığında şu izinleri iste:
- ✅ Accessibility Service (İçinde: Gizli Mod kontrol)
- ✅ Screen Capture (İçinde: Ekran görüntüsü)
- ✅ Location (İçinde: Konum izlemesi)
- ✅ Storage (İçinde: Dosya yönetimi)
- ✅ Notification Access (İçinde: Bildirim okuma)
- ✅ Camera (İçinde: Kamera erişimi)

### 3.7 API Sunucusu Adresi Ayarla

**SilentModeNetworkService.kt** dosyasında:
```kotlin
private val baseUrl = "https://api.phonehub.local:7001" // Kendi sunucunun adresi
```

✅ Android uygulaması hazır!

---

## 🐳 Adım 4: Docker Kurulumu (Opsiyonel)

Eğer Docker kurulu ise, her şey bir komutla başlatılabilir:

### 4.1 Docker Desktop'u Yükle
[İndir](https://www.docker.com/products/docker-desktop)

### 4.2 Docker Compose Dosyasını Kur
```bash
# Depo köküne git
cd android-cihaz-takibi

# Docker container'larını başlat
docker-compose up -d

# Container'lar çalışıyor mu kontrol et:
docker-compose ps
```

### 4.3 Veritabanı ve Redis'i Kontrol Et
```bash
# PostgreSQL kontrol:
docker exec phonehub-db psql -U phonehub_user -d phonehub_db -c "SELECT VERSION();"

# Redis kontrol:
docker exec phonehub-cache redis-cli ping
# Yanıt: PONG
```

### 4.4 Backend Uygulamasını Kontrol Et
```bash
curl https://localhost:7001/swagger
```

✅ Docker başarıyla kuruldu!

---

## 🎯 Adım 5: İlk Kurulum

### 5.1 Kullanıcı Oluştur
```bash
# Backend API'ye POST iste:
POST /api/auth/register
{
  "email": "test@example.com",
  "password": "Test@1234",
  "firstName": "Test",
  "lastName": "User"
}
```

### 5.2 Giriş Yap
```bash
POST /api/auth/login
{
  "email": "test@example.com",
  "password": "Test@1234"
}

# Yanıt:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 5.3 Windows Uygulamasına Token Ekle
```csharp
// SilentModeViewModel.cs'de:
private string GetAuthToken()
{
    return "<aldığın-token-buraya-koy>";
}
```

### 5.4 Android Uygulamasında Giriş Yap
- Uygulamayı aç
- Email ve şifreyi gir
- Giriş'e tıkla
- İzinleri ver

### 5.5 Gizli Mod Testi
1. Windows uygulamasını aç
2. "Gizli Mod" sekmesine git
3. Telefon seçimini yap
4. "Gizli Modu Etkinleştir" butonuna tıkla
5. Telefonda:
   - Ekran kapanmalı
   - Ses kapalı olmalı
   - Hiç bildirim görülmemeli
6. Bilgisayardan dokunma testi yap
7. "Gizli Modu Devre Dışı Bırak" butonuna tıkla

✅ Tüm kurulum tamamlandı!

---

## 🔧 Sorun Giderme

### Sorun: Backend'e bağlanılamıyor
```bash
# 1. Backend çalışıyor mu kontrol et:
netstat -ano | findstr :7001  # Windows
lsof -i :7001               # Mac/Linux

# 2. Sunucuyu yeniden başlat:
dotnet run

# 3. Firewall kontrol et:
# Firewall ayarlarında 7001 portunu aç
```

### Sorun: Veritabanı bağlantısı hatası
```bash
# 1. PostgreSQL çalışıyor mu kontrol et:
psql -U phonehub_user -d phonehub_db  # Şifre: PhoneHub@123456

# 2. Bağlantı stringini kontrol et:
# appsettings.json'da "DefaultConnection" doğru mu?

# 3. Veritabanı oluştur:
createdb -U postgres phonehub_db
```

### Sorun: Android Accessibility Service etkinleştirilemedi
1. Ayarlar > Erişilebilirlik'e git
2. "PhoneHub" uygulamasını bul
3. Etkinleştir'i aç
4. "İzin Ver" seçeneğini tıkla
5. Uygulamayı yeniden başlat

### Sorun: Ekran görüntüsü alınamıyor
1. MediaProjection izni gerekli
2. Android 5.0+'ta sistem tarafından onay istenir
3. "Başla" veya "İzin Ver" butonuna tıkla
4. Telefonu yeniden başlat

### Sorun: Gizli Mod çalışmıyor
```bash
# Telefon loglarını kontrol et:
adb logcat | grep SilentMode

# Accessibility Service'in çalışıp çalışmadığını kontrol et:
adb shell dumpsys accessibility | grep PhoneHub

# Uygulamayı kapat ve yeniden başlat
adb shell am force-stop com.phonehub.android
adb shell am start -n com.phonehub.android/.ui.MainActivity
```

---

## 📞 Yardım Gerekli?

Sorun çözemediysen:
1. [Issues](https://github.com/bkahriman315-web/android-cihaz-takibi/issues) sekmesine git
2. Yeni issue açıp açıkla
3. Logları ve hata mesajlarını ekle
4. Sistem bilgilerini paylaş

---

**Tebrikler! PhoneHub Pro başarıyla kuruldu! 🎉**

Aşağıdaki adımlar:
- [Kullanım Rehberini Oku](docs/USAGE_GUIDE.md)
- [Gizli Mod Özelliklerini Keşfet](docs/USAGE_GUIDE.md#gizli-mod-rehberi)
- [API Dokümantasyonunu İncele](docs/API.md)
