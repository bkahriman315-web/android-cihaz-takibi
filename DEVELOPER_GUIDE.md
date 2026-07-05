# Geliştirici Kılavuzu / Developer Guide

## İçindekiler / Table of Contents
1. [Türkçe Kılavuz](#türkçe-kılavuz)
2. [English Guide](#english-guide)

---

## Türkçe Kılavuz

### Ortam Kurulumu

#### Gerekli Yazılımlar
- **Java**: OpenJDK 11 veya üzeri
- **Kotlin**: 1.6+
- **Android Studio**: 2021.1 veya üzeri
- **Node.js**: 14+ (Backend API geliştirmesi için)
- **MongoDB**: 4.4+ (Database)
- **Git**: 2.30+

#### Adım Adım Kurulum

1. **Repository'yi clone edin**
```bash
git clone https://github.com/bkahriman315-web/android-cihaz-takibi.git
cd android-cihaz-takibi
```

2. **Backend ortamını kurun**
```bash
cd backend
npm install
cp .env.example .env
# .env dosyasını düzenleyin
```

3. **Frontend ortamını kurun**
```bash
cd ../android
./gradlew build
```

4. **Database'i başlatın**
```bash
# MongoDB yerel veya Docker'da çalıştırın
docker run -d -p 27017:27017 --name mongodb mongo:4.4
```

### Geliştirme Iş Akışı

#### 1. Feature Branch Oluşturma
```bash
git checkout -b feature/yeni-ozellik
# veya
git checkout -b fix/hata-adi
# veya
git checkout -b docs/dokumantasyon-adi
```

#### 2. Kod Yazma Kuralları

**Kotlin (Android)**
```kotlin
// Naming conventions
class UserDeviceActivity : AppCompatActivity() {
    private lateinit var viewModel: DeviceViewModel
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_user_device)
        initializeComponents()
    }
    
    private fun initializeComponents() {
        // Implementation
    }
}
```

**JavaScript/TypeScript (Backend)**
```javascript
// Naming conventions
class UserController {
    async getUser(req, res) {
        try {
            const user = await User.findById(req.params.id);
            res.json(user);
        } catch (error) {
            res.status(500).json({ error: error.message });
        }
    }
}
```

#### 3. Commit İlkeleleri
- **Açıklayıcı mesajlar yazın**: `git commit -m "Add user authentication feature"`
- **Sık commit edin**: Günde en az 2-3 kez
- **Conventional Commits kullanın**:
  - `feat:` Yeni özellik
  - `fix:` Hata düzeltmesi
  - `docs:` Dokümantasyon
  - `style:` Kod stili
  - `refactor:` Kod yeniden düzenleme
  - `test:` Test ekleme
  - `chore:` Diğer

```bash
git commit -m "feat: add biometric authentication"
git commit -m "fix: resolve null pointer exception in device list"
```

### Testing

#### Unit Tests (Android)
```kotlin
class UserDeviceViewModelTest {
    private lateinit var viewModel: DeviceViewModel
    
    @Before
    fun setup() {
        viewModel = DeviceViewModel()
    }
    
    @Test
    fun `test get devices returns list`() {
        // Arrange
        val expected = listOf(Device("123", "Samsung"))
        
        // Act
        val result = viewModel.getDevices()
        
        // Assert
        assertEquals(expected, result)
    }
}
```

#### Integration Tests (Backend)
```javascript
describe('User API', () => {
    it('should return user by id', async () => {
        const res = await request(app)
            .get('/api/v1/users/123')
            .expect(200);
        
        expect(res.body).toHaveProperty('id');
    });
});
```

#### Test Çalıştırma
```bash
# Android
./gradlew test
./gradlew connectedAndroidTest

# Backend
npm test

# Coverage raporu
npm run test:coverage
```

### API Geliştirmesi

#### Yeni Endpoint Ekleme
```javascript
// routes/users.js
router.post('/api/v1/users/register', async (req, res) => {
    try {
        // Validation
        const { email, password } = req.body;
        if (!email || !password) {
            return res.status(400).json({ error: 'Missing fields' });
        }
        
        // Implementation
        const user = new User({ email, password });
        await user.save();
        
        res.status(201).json(user);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});
```

#### API Dokümantasyonu (Swagger/OpenAPI)
```yaml
/api/v1/users/{id}:
  get:
    summary: Get user by ID
    parameters:
      - name: id
        in: path
        required: true
        schema:
          type: string
    responses:
      200:
        description: User found
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/User'
      404:
        description: User not found
```

### Debug ve Logging

#### Android Debug
```kotlin
// Build.gradle
buildTypes {
    debug {
        debuggable true
        minifyEnabled false
    }
}

// Log kullanımı
Log.d("DeviceActivity", "Device list size: ${devices.size}")
Log.e("DeviceActivity", "Error occurred", exception)
```

#### Backend Debug
```javascript
// Winston logger
const logger = require('winston');

logger.info('User created', { userId: user.id });
logger.error('Database error', { error: err.message });
```

### Performance Optimization

#### Android Optimization
```kotlin
// RecyclerView Optimization
class DeviceAdapter : RecyclerView.Adapter<DeviceViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DeviceViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_device, parent, false)
        return DeviceViewHolder(view)
    }
    
    override fun onBindViewHolder(holder: DeviceViewHolder, position: Int) {
        holder.bind(devices[position])
    }
    
    override fun getItemCount(): Int = devices.size
}
```

#### Backend Optimization
```javascript
// Caching örneği
const cache = new Map();

async function getDevices(userId) {
    const cacheKey = `devices:${userId}`;
    if (cache.has(cacheKey)) {
        return cache.get(cacheKey);
    }
    
    const devices = await Device.find({ userId });
    cache.set(cacheKey, devices);
    
    return devices;
}
```

### Pull Request Süreci

1. **PR Açın**: Açıklayıcı başlık ve açıklama yazın
2. **Checks Bekleyin**: CI/CD pipeline başarılı olmalı
3. **Review Alın**: En az 2 reviewer onaylı olmalı
4. **Merge Yapın**: Squash merge'i tercih edin

### Yararlı Komutlar

```bash
# Branch güncelleme
git fetch origin
git rebase origin/main

# Değişiklikleri stash'le
git stash
git stash pop

# Son commit'i düzelt
git commit --amend

# Force push (dikkat!)
git push origin feature/branch --force-with-lease

# Log görüntüleme
git log --oneline --graph --all
```

---

## English Guide

### Environment Setup

#### Required Tools
- **Java**: OpenJDK 11 or later
- **Kotlin**: 1.6+
- **Android Studio**: 2021.1 or later
- **Node.js**: 14+ (for Backend API development)
- **MongoDB**: 4.4+ (Database)
- **Git**: 2.30+

#### Step-by-Step Setup

1. **Clone the repository**
```bash
git clone https://github.com/bkahriman315-web/android-cihaz-takibi.git
cd android-cihaz-takibi
```

2. **Setup Backend**
```bash
cd backend
npm install
cp .env.example .env
# Edit .env file with your configuration
```

3. **Setup Frontend**
```bash
cd ../android
./gradlew build
```

4. **Start Database**
```bash
docker run -d -p 27017:27017 --name mongodb mongo:4.4
```

### Development Workflow

#### Code Style Guide
- Follow Google's Kotlin style guide
- Use 4 spaces for indentation
- Max line length: 100 characters
- Use meaningful variable names

#### Commit Message Format
```
<type>(<scope>): <subject>

<body>

<footer>
```

Example:
```
feat(auth): add JWT token validation

- Implement token verification
- Add error handling for expired tokens
- Update API routes

Closes #123
```

### Running Tests

```bash
# Android tests
./gradlew test

# Backend tests
npm test

# All tests with coverage
npm run test:coverage
```

### Debugging Tips

1. **Android Studio Debugger**: F9 to toggle breakpoints
2. **Logcat filtering**: Filter by tag or process
3. **Backend logging**: Use structured logging with Winston
4. **Network inspection**: Use Android Network Monitor

### Version Control Best Practices

- Always create feature branches
- Never commit to main directly
- Delete merged branches
- Keep commit history clean
- Review before pushing

### Resources

- [Kotlin Documentation](https://kotlinlang.org/docs/)
- [Android Documentation](https://developer.android.com/)
- [Node.js Best Practices](https://nodejs.org/en/docs/guides/)
- [Git Documentation](https://git-scm.com/doc)

---

**Last Updated**: 2026-07-05  
**Version**: 1.0
