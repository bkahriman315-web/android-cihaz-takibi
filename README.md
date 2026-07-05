# PhoneHub Pro - Android Device Monitoring & Remote Control

🎯 **Türkçe Açıklama**

Windows bilgisayarından Android telefonunuzu izleyin, kontrol edin ve sistem bilgilerine erişin.

## 🌟 Özellikler

### 📱 Canlı İzleme
- ✅ Canlı ekran görüntüsü (15/30 FPS)
- ✅ Batarya durumu
- ✅ Konum bilgisi (GPS)
- ✅ İnternet ve WiFi durumu
- ✅ Sistem bilgileri (RAM, CPU, depolama)

### 🎮 Uzaktan Kontrol
- ✅ Dokunma işlemleri
- ✅ Metin giriş
- ✅ Kaydırma
- ✅ Uygulama açma

### 📂 Dosya Yönetimi
- ✅ DCIM, Download, Pictures klasörleri erişimi
- ✅ Dosya indirme ve yükleme
- ✅ Dosya silme ve taşıma

### 📷 Kamera
- ✅ Ön ve arka kamera canlı görüntüsü
- ✅ Ekran kaydı

### 📍 Konum
- ✅ Canlı konum haritada gösterim
- ✅ Geçmiş rotalar
- ✅ Geofencing

### 📬 Bildirimler
- ✅ WhatsApp, SMS, Telegram, Instagram bildirimleri
- ✅ Bildirim okuması

## 🏗️ Sistem Mimarisi

```
┌─────────────────────────┐
│  Windows App (WinUI 3)  │
│  - Real-time preview    │
│  - Remote control       │
│  - File manager         │
└────────────┬────────────┘
             │ HTTPS + SignalR/WebSocket
             ▼
┌─────────────────────────┐
│  ASP.NET Core Server    │
│  - REST API             │
│  - SignalR Hubs         │
│  - PostgreSQL DB        │
│  - Redis Cache          │
│  - JWT Authentication   │
└────────────┬────────────┘
             │ HTTPS + SignalR/WebSocket
             ▼
┌─────────────────────────┐
│  Android App (Kotlin)   │
│  - MediaProjection      │
│  - Accessibility Svc    │
│  - Location Service     │
│  - Notification Svc     │
└─────────────────────────┘
```

## 🚀 Hızlı Başlangıç

### Ön Koşullar
- .NET 9 SDK
- Node.js 18+
- Android Studio
- Docker & Docker Compose
- PostgreSQL 14+

### 1. Backend Kurulumu

```bash
cd backend/PhoneHub.API
dotnet restore
dotnet ef database update
dotnet run
```

**API Endpoint:** `https://localhost:7001`
**Swagger:** `https://localhost:7001/swagger`

### 2. Windows Uygulaması

```bash
cd frontend/PhoneHub.Desktop
de msbuild PhoneHub.Desktop.sln
```

### 3. Android Uygulaması

```bash
cd mobile/PhoneHub
gradlew assembleDebug
```

### 4. Docker Ortamı

```bash
docker-compose up -d
```

## 📋 Proje Yapısı

```
android-cihaz-takibi/
├── backend/
│   ├── PhoneHub.API/              # ASP.NET Core API
│   ├── PhoneHub.Core/             # Business Logic
│   ├── PhoneHub.Infrastructure/   # Data Access
│   ├── PhoneHub.Tests/            # Unit Tests
│   └── Dockerfile
├── frontend/
│   ├── PhoneHub.Desktop/          # WinUI 3 App
│   └── PhoneHub.Shared/           # Shared Models
├── mobile/
│   └── PhoneHub/                  # Android App (Kotlin)
├── docker-compose.yml
├── .github/
│   └── workflows/                 # CI/CD Pipeline
└── docs/
    ├── API.md
    ├── SECURITY.md
    └── DEPLOYMENT.md
```

## 🔐 Güvenlik

- **AES-256** end-to-end şifreleme
- **JWT** token authentication
- **TLS 1.3** HTTPS
- **CORS** policy
- **Rate limiting**

## 📖 Dokümantasyon

- [API Documentation](docs/API.md)
- [Security Guide](docs/SECURITY.md)
- [Deployment Guide](docs/DEPLOYMENT.md)
- [Developer Guide](docs/DEVELOPER.md)

## 📝 Lisans

MIT License - [LICENSE](LICENSE) dosyasına bakın.

## 👨‍💻 Geliştirici

Bilal Kahriman (@bkahriman315-web)

## ⚖️ Yasal Uyarı

Bu yazılım yalnızca kendi cihazlarınızı izlemek için kullanılmalıdır. Başkasının cihazını izlemek yasadışıdır ve GDPR, KVKK gibi kanunlara aykırıdır.
