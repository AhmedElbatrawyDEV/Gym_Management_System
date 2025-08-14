# دليل النشر - Enhanced Workout API

## نظرة عامة
هذا الدليل يوضح كيفية نشر نظام إدارة الجيم الشامل المطور بـ C#/.NET على بيئات مختلفة.

## متطلبات النظام

### الحد الأدنى
- **نظام التشغيل**: Windows Server 2019+ أو Ubuntu 20.04+
- **المعالج**: 2 CPU cores
- **الذاكرة**: 4 GB RAM
- **التخزين**: 20 GB مساحة فارغة
- **.NET Runtime**: .NET 8.0 أو أحدث
- **قاعدة البيانات**: SQL Server 2019+ أو SQL Server Express

### الموصى به
- **نظام التشغيل**: Windows Server 2022 أو Ubuntu 22.04 LTS
- **المعالج**: 4+ CPU cores
- **الذاكرة**: 8+ GB RAM
- **التخزين**: 50+ GB SSD
- **قاعدة البيانات**: SQL Server 2022

## خطوات النشر

### 1. إعداد البيئة

#### على Windows Server
```powershell
# تحميل وتثبيت .NET 8.0 Runtime
Invoke-WebRequest -Uri "https://download.microsoft.com/download/dotnet/8.0/dotnet-hosting-8.0-win.exe" -OutFile "dotnet-hosting.exe"
.\dotnet-hosting.exe /quiet

# تثبيت IIS (إذا لم يكن مثبتاً)
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole, IIS-WebServer, IIS-CommonHttpFeatures, IIS-HttpErrors, IIS-HttpLogging, IIS-RequestFiltering, IIS-StaticContent, IIS-DefaultDocument, IIS-DirectoryBrowsing, IIS-ASPNET45
```

#### على Ubuntu/Linux
```bash
# تحديث النظام
sudo apt update && sudo apt upgrade -y

# تثبيت .NET 8.0
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-8.0

# تثبيت Nginx (اختياري للـ reverse proxy)
sudo apt install nginx -y
```

### 2. إعداد قاعدة البيانات

#### SQL Server
```sql
-- إنشاء قاعدة بيانات جديدة
CREATE DATABASE WorkoutApiDb;

-- إنشاء مستخدم للتطبيق
CREATE LOGIN WorkoutApiUser WITH PASSWORD = 'YourStrongPassword123!';
USE WorkoutApiDb;
CREATE USER WorkoutApiUser FOR LOGIN WorkoutApiUser;
ALTER ROLE db_owner ADD MEMBER WorkoutApiUser;
```

#### تحديث Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=WorkoutApiDb;User Id=WorkoutApiUser;Password=YourStrongPassword123!;TrustServerCertificate=true;"
  }
}
```

### 3. نشر التطبيق

#### الطريقة الأولى: نشر مباشر
```bash
# استخراج الملفات
unzip Enhanced_Workout_API_Final.zip
cd final_workout_api

# بناء التطبيق للإنتاج
dotnet publish src/WorkoutAPI.Api/WorkoutAPI.Api.csproj -c Release -o /var/www/workoutapi

# تعيين الصلاحيات (Linux)
sudo chown -R www-data:www-data /var/www/workoutapi
sudo chmod -R 755 /var/www/workoutapi
```

#### الطريقة الثانية: استخدام Docker
```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/WorkoutAPI.Api/WorkoutAPI.Api.csproj", "src/WorkoutAPI.Api/"]
COPY ["src/WorkoutAPI.Application/WorkoutAPI.Application.csproj", "src/WorkoutAPI.Application/"]
COPY ["src/WorkoutAPI.Domain/WorkoutAPI.Domain.csproj", "src/WorkoutAPI.Domain/"]
COPY ["src/WorkoutAPI.Infrastructure/WorkoutAPI.Infrastructure.csproj", "src/WorkoutAPI.Infrastructure/"]
RUN dotnet restore "src/WorkoutAPI.Api/WorkoutAPI.Api.csproj"
COPY . .
WORKDIR "/src/src/WorkoutAPI.Api"
RUN dotnet build "WorkoutAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkoutAPI.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkoutAPI.Api.dll"]
```

```bash
# بناء وتشغيل Docker container
docker build -t workout-api .
docker run -d -p 8080:80 --name workout-api-container workout-api
```

### 4. إعداد Reverse Proxy (Nginx)

```nginx
# /etc/nginx/sites-available/workout-api
server {
    listen 80;
    server_name your-domain.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

```bash
# تفعيل الموقع
sudo ln -s /etc/nginx/sites-available/workout-api /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

### 5. إعداد SSL/HTTPS

```bash
# تثبيت Certbot
sudo apt install certbot python3-certbot-nginx -y

# الحصول على شهادة SSL
sudo certbot --nginx -d your-domain.com

# تجديد تلقائي للشهادة
sudo crontab -e
# إضافة السطر التالي:
0 12 * * * /usr/bin/certbot renew --quiet
```

### 6. إعداد خدمة النظام (Systemd)

```ini
# /etc/systemd/system/workout-api.service
[Unit]
Description=Workout API
After=network.target

[Service]
Type=notify
ExecStart=/usr/bin/dotnet /var/www/workoutapi/WorkoutAPI.Api.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=workout-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

```bash
# تفعيل وتشغيل الخدمة
sudo systemctl enable workout-api.service
sudo systemctl start workout-api.service
sudo systemctl status workout-api.service
```

## إعدادات الإنتاج

### appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=production-server;Database=WorkoutApiDb;User Id=WorkoutApiUser;Password=YourStrongPassword123!;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Key": "YourProductionSecretKeyThatShouldBeAtLeast32CharactersLong",
    "Issuer": "WorkoutAPI",
    "Audience": "WorkoutAPIUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "your-domain.com"
}
```

## الأمان والصيانة

### 1. النسخ الاحتياطي
```bash
# نسخ احتياطي لقاعدة البيانات
sqlcmd -S server -d WorkoutApiDb -Q "BACKUP DATABASE WorkoutApiDb TO DISK = '/backup/WorkoutApiDb_$(date +%Y%m%d).bak'"

# نسخ احتياطي للملفات
tar -czf /backup/workout-api-files-$(date +%Y%m%d).tar.gz /var/www/workoutapi
```

### 2. المراقبة
```bash
# مراقبة الخدمة
sudo systemctl status workout-api.service

# مراقبة السجلات
sudo journalctl -u workout-api.service -f

# مراقبة استخدام الموارد
htop
```

### 3. التحديثات
```bash
# إيقاف الخدمة
sudo systemctl stop workout-api.service

# نسخ احتياطي
cp -r /var/www/workoutapi /backup/workoutapi-backup-$(date +%Y%m%d)

# نشر النسخة الجديدة
dotnet publish src/WorkoutAPI.Api/WorkoutAPI.Api.csproj -c Release -o /var/www/workoutapi

# تشغيل الخدمة
sudo systemctl start workout-api.service
```

## استكشاف الأخطاء

### مشاكل شائعة وحلولها

#### 1. خطأ في الاتصال بقاعدة البيانات
```bash
# فحص الاتصال
sqlcmd -S server -U username -P password -Q "SELECT 1"

# فحص Connection String
grep -r "ConnectionStrings" /var/www/workoutapi/appsettings*.json
```

#### 2. مشاكل الصلاحيات
```bash
# إعادة تعيين الصلاحيات
sudo chown -R www-data:www-data /var/www/workoutapi
sudo chmod -R 755 /var/www/workoutapi
```

#### 3. مشاكل الذاكرة
```bash
# زيادة حد الذاكرة
echo 'Environment=DOTNET_GCHeapHardLimit=2000000000' >> /etc/systemd/system/workout-api.service
sudo systemctl daemon-reload
sudo systemctl restart workout-api.service
```

## الدعم والصيانة

### جهات الاتصال
- **الدعم التقني**: support@gym-system.com
- **الطوارئ**: +1-xxx-xxx-xxxx
- **التوثيق**: https://docs.gym-system.com

### جدولة الصيانة
- **النسخ الاحتياطي**: يومياً في الساعة 2:00 صباحاً
- **التحديثات**: أسبوعياً يوم الأحد في الساعة 3:00 صباحاً
- **مراجعة الأمان**: شهرياً

---

**ملاحظة**: تأكد من اختبار جميع الخطوات في بيئة تطوير قبل تطبيقها في الإنتاج.

