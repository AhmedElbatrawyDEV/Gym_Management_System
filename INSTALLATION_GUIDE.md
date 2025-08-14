# دليل التثبيت - نظام إدارة الجيم الشامل

## متطلبات النظام

### Backend (C#/.NET)
- **.NET 8.0 SDK** أو أحدث
- **SQL Server 2019+** أو **SQL Server Express**
- **Visual Studio 2022** أو **VS Code** مع C# extension

### Frontend (React)
- **Node.js 18+** 
- **npm** أو **pnpm** (مفضل)
- متصفح حديث (Chrome, Firefox, Safari, Edge)

## خطوات التثبيت

### 1. تحضير البيئة

#### تثبيت .NET 8
```bash
# على Windows
# تحميل من: https://dotnet.microsoft.com/download/dotnet/8.0

# على Ubuntu/Linux
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0

# التحقق من التثبيت
dotnet --version
```

#### تثبيت Node.js
```bash
# على Windows
# تحميل من: https://nodejs.org/

# على Ubuntu/Linux
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# تثبيت pnpm
npm install -g pnpm

# التحقق من التثبيت
node --version
pnpm --version
```

### 2. إعداد قاعدة البيانات

#### SQL Server Express (مجاني)
```bash
# تحميل وتثبيت SQL Server Express
# https://www.microsoft.com/en-us/sql-server/sql-server-downloads

# أو استخدام Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongPassword123!" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

#### إنشاء قاعدة البيانات
```sql
-- الاتصال بـ SQL Server Management Studio أو Azure Data Studio
-- تشغيل الأوامر التالية:

CREATE DATABASE WorkoutApiDb;
GO

-- إنشاء مستخدم للتطبيق
CREATE LOGIN WorkoutApiUser WITH PASSWORD = 'YourStrongPassword123!';
GO

USE WorkoutApiDb;
GO

CREATE USER WorkoutApiUser FOR LOGIN WorkoutApiUser;
GO

ALTER ROLE db_owner ADD MEMBER WorkoutApiUser;
GO
```

### 3. تشغيل Backend

```bash
# استخراج الملفات
unzip Complete_Gym_Management_System_Final.zip
cd complete_gym_system/backend

# استعادة الحزم
dotnet restore

# تحديث Connection String
# تعديل ملف appsettings.json
```

#### تحديث appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WorkoutApiDb;User Id=WorkoutApiUser;Password=YourStrongPassword123!;TrustServerCertificate=true;"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "WorkoutAPI",
    "Audience": "WorkoutAPIUsers",
    "ExpirationInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

```bash
# بناء المشروع
dotnet build

# تشغيل Migrations (إنشاء الجداول)
cd src/WorkoutAPI.Api
dotnet ef database update

# تشغيل API
dotnet run
```

سيعمل API على: `https://localhost:5001` أو `http://localhost:5000`

### 4. تشغيل Frontend

```bash
# في terminal جديد
cd complete_gym_system/frontend

# تثبيت التبعيات
pnpm install

# تشغيل التطبيق
pnpm run dev
```

سيعمل التطبيق على: `http://localhost:5173`

## التحقق من التثبيت

### 1. اختبار Backend
- افتح المتصفح على: `https://localhost:5001/swagger`
- يجب أن تظهر واجهة Swagger API

### 2. اختبار Frontend
- افتح المتصفح على: `http://localhost:5173`
- يجب أن تظهر صفحة تسجيل الدخول

### 3. اختبار التكامل
- استخدم بيانات الاختبار:
  - **Admin**: `admin@gym.com` / `password123`
  - **User**: `user@gym.com` / `password123`

## حل المشاكل الشائعة

### مشكلة الاتصال بقاعدة البيانات
```bash
# التحقق من تشغيل SQL Server
# Windows
services.msc # البحث عن SQL Server

# Linux/Docker
docker ps # التحقق من container
```

### مشكلة CORS
إذا ظهرت أخطاء CORS، تأكد من:
- تشغيل Backend على المنفذ الصحيح
- إعدادات CORS في `Program.cs`

### مشكلة Entity Framework
```bash
# إعادة إنشاء قاعدة البيانات
dotnet ef database drop
dotnet ef database update
```

### مشكلة Node.js Dependencies
```bash
# حذف node_modules وإعادة التثبيت
rm -rf node_modules
rm pnpm-lock.yaml
pnpm install
```

## الإعدادات المتقدمة

### تغيير المنافذ
#### Backend
في `Properties/launchSettings.json`:
```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

#### Frontend
في `vite.config.js`:
```javascript
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    host: true
  }
})
```

### إعداد HTTPS للتطوير
```bash
# إنشاء شهادة تطوير
dotnet dev-certs https --trust
```

## النشر للإنتاج

### Backend
```bash
# بناء للإنتاج
dotnet publish -c Release -o ./publish

# نشر على IIS أو Azure
```

### Frontend
```bash
# بناء للإنتاج
pnpm run build

# نشر على Netlify, Vercel, أو أي خدمة استضافة
```

## الدعم

إذا واجهت أي مشاكل:
1. تحقق من logs في Backend
2. تحقق من Developer Console في المتصفح
3. تأكد من تشغيل جميع الخدمات
4. راجع التوثيق في `README.md`

---

**ملاحظة**: هذا النظام مصمم للتطوير والاختبار. للاستخدام في الإنتاج، يرجى مراجعة إعدادات الأمان وقاعدة البيانات.

