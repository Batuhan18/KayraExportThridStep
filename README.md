🎯 Mikroservisler
1. Auth Microservice - Kimlik Doğrulama Servisi

✅ Microsoft Identity + JWT Token entegrasyonu
✅ Custom Authorization Handlers (Approval, Delete, BusinessHours)
✅ Token üretimi ve doğrulama
✅ Kullanıcı yönetimi (Register, Login)
🔐 Port: 5001 (Docker), 7161 (Local)

Teknolojiler:

ASP.NET Core 8.0
Microsoft.AspNetCore.Identity
JWT Bearer Authentication
SQL Server

2. Product Microservice - Ürün Yönetim Servisi

✅ Onion Architecture implementasyonu
✅ CQRS Pattern (Command/Query ayrımı)
✅ MediatR ile command/query handling
✅ Redis Cache ile sorgu optimizasyonu
✅ Cache Invalidation stratejisi
✅ Stored Procedures ile veritabanı işlemleri
✅ JWT ile korumalı endpoint'ler
🔐 Port: 5002 (Docker), 7005 (Local)

Özellikler:

Ürün ekleme (JWT gerekli)
Ürün güncelleme
Ürün silme
Ürün listeleme
Redis cache kullanımı

Teknolojiler:

CQRS Pattern
MediatR
Redis (StackExchange.Redis)
ADO.NET (Stored Procedures)
SQL Server

3. Log Microservice - Merkezi Log Yönetimi

✅ Serilog ile structured logging
✅ Seq ile log visualization
✅ SQL Server ile log persistence
✅ Log seviyelendirme (INFO, WARNING, ERROR, CRITICAL)
✅ Merkezi log toplama API
✅ Log sorgulama ve istatistik endpoint'leri
🔐 Port: 5003 (Docker), 7200 (Local)

Log Seviyeleri:

INFO: Bilgilendirme logları
WARNING: Uyarı logları
ERROR: Hata logları
CRITICAL: Kritik sistem hataları

Teknolojiler:

Serilog
Serilog.Sinks.Console
Serilog.Sinks.File
Serilog.Sinks.MSSqlServer
Serilog.Sinks.Seq
SQL Server

4. API Gateway - YARP Reverse Proxy

✅ YARP (Yet Another Reverse Proxy) ile routing
✅ Merkezi JWT doğrulama
✅ Policy-based authorization
✅ CORS desteği
✅ Rate limiting hazır (isteğe bağlı)
🔐 Port: 5000 (Docker), 7000 (Local)

Özellikler:

Tek giriş noktası (Single Entry Point)
Mikroservis routing
JWT token validation

🛠️ SOLID Prensipleri Uygulaması
Single Responsibility Principle (SRP)

Her servis tek bir sorumluluğa sahip (Auth, Product, Log)
Command ve Query handler'ları ayrı sınıflar
Repository pattern ile veri erişimi ayrıştırıldı

Open/Closed Principle (OCP)

Interface'ler üzerinden extension
Yeni özellikler mevcut kodu bozmadan eklenebilir
Policy-based authorization ile genişletilebilir yetkilendirme

Liskov Substitution Principle (LSP)

IRepository<T> implementasyonları birbirinin yerine kullanılabilir
IAuthorizationHandler implementasyonları değiştirilebilir

Interface Segregation Principle (ISP)

IRepository, IProductCommandRepository gibi özel interface'ler
Gereksiz method'lar zorlanmadı

Dependency Inversion Principle (DIP)

Tüm bağımlılıklar interface'ler üzerinden
Constructor Injection ile Dependency Injection
High-level modüller low-level detaylara bağlı değil

🐳 Docker Kurulumu
Gereksinimler

Docker Desktop 4.0+
Docker Compose 2.0+

Hızlı Başlangıç
bash# Repository'yi klonla
git clone <repository-url>
cd KayraExportThridStep

# Tüm servisleri başlat
docker-compose up -d

# Logları izle
docker-compose logs -f

# Servisleri durdur
docker-compose down

# Volume'leri de temizle
docker-compose down -v

Health Checks
bash# Gateway
curl http://localhost:5000/

# Auth Service
curl http://localhost:5001/health

# Product Service
curl http://localhost:5002/health

# Log Service
curl http://localhost:5003/health

🚀 Yerel Geliştirme Kurulumu
Gereksinimler

.NET 8.0 SDK
Visual Studio 2022 veya Rider
SQL Server 2022 / LocalDB
Redis (Docker veya yerel)
Git

Adımlar
1. Repository Klonla
bashgit clone <repository-url>
cd KayraExportThridStep
2. SQL Server'ı Hazırla
bash# SQL Server Management Studio (SSMS) ile bağlan
# DatabaseScripts klasöründeki scriptleri çalıştır
3. Redis'i Başlat
bashdocker run -d --name redis -p 6379:6379 redis:latest
4. Servisleri Başlat
Terminal 1 - Auth Service:
bashcd AuthMicroservice/KayraExportThirdStep.Auth.API
dotnet run
Terminal 2 - Product Service:
bashcd ProductMicroservice/API
dotnet run
Terminal 3 - Log Service:
bashcd LogMicroservice/KayraExportThirdStep.Log.API
dotnet run
Terminal 4 - API Gateway:
bashcd APIGateway/KayraExportThirdStep.APIGateway
dotnet run

5. Test Et

Gateway: https://localhost:7000
Swagger (Auth): https://localhost:7161/swagger
Swagger (Product): https://localhost:7005/swagger
Swagger (Log): https://localhost:7200/swagger
