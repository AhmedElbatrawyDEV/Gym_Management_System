# دليل التثبيت والتشغيل - Workout API

## متطلبات النظام

### البرامج المطلوبة
- **Visual Studio 2022** (Community أو أعلى) أو **Visual Studio Code**
- **.NET 8 SDK** - [تحميل من هنا](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (LocalDB، Express، أو Developer Edition)
- **Git** (اختياري)

### فحص التثبيت
تأكد من تثبيت .NET 8 بتشغيل الأمر التالي في Command Prompt:
```bash
dotnet --version
```
يجب أن يظهر رقم إصدار 8.0.x

## خطوات التثبيت

### 1. استخراج المشروع
1. استخرج ملف `WorkoutAPI_Solution.zip` إلى مجلد على جهازك
2. افتح مجلد `WorkoutAPI`

### 2. فتح المشروع في Visual Studio

#### باستخدام Visual Studio 2022:
1. افتح Visual Studio 2022
2. اختر "Open a project or solution"
3. انتقل إلى مجلد المشروع واختر `WorkoutAPI.sln`

#### باستخدام Visual Studio Code:
1. افتح Visual Studio Code
2. اختر "File" > "Open Folder"
3. اختر مجلد `WorkoutAPI`
4. ثبت C# Extension إذا لم يكن مثبتاً

### 3. استعادة الحزم (NuGet Packages)

#### في Visual Studio:
1. انقر بزر الماوس الأيمن على Solution في Solution Explorer
2. اختر "Restore NuGet Packages"

#### في Command Line:
```bash
cd WorkoutAPI
dotnet restore
```

### 4. تكوين قاعدة البيانات

#### تحديث Connection String:
1. افتح `src/WorkoutAPI.Api/appsettings.json`
2. عدل Connection String حسب إعداد SQL Server لديك:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkoutDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### خيارات Connection String:

**LocalDB (الافتراضي):**
```
Server=(localdb)\\mssqllocaldb;Database=WorkoutDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

**SQL Server Express:**
```
Server=.\\SQLEXPRESS;Database=WorkoutDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

**SQL Server مع اسم مستخدم وكلمة مرور:**
```
Server=localhost;Database=WorkoutDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true
```

### 5. إنشاء قاعدة البيانات

#### الطريقة الأولى - تلقائي عند التشغيل:
قاعدة البيانات ستُنشأ تلقائياً عند تشغيل التطبيق لأول مرة.

#### الطريقة الثانية - يدوي بـ Entity Framework:
```bash
cd src/WorkoutAPI.Api
dotnet ef database update
```

### 6. تشغيل التطبيق

#### في Visual Studio:
1. اختر `WorkoutAPI.Api` كـ Startup Project
2. اضغط F5 أو انقر "Start"

#### في Command Line:
```bash
cd src/WorkoutAPI.Api
dotnet run
```

### 7. التحقق من التشغيل
1. افتح المتصفح واذهب إلى: `http://localhost:5000`
2. يجب أن تظهر صفحة Swagger UI
3. جرب API endpoints المختلفة

## استكشاف الأخطاء

### خطأ في الاتصال بقاعدة البيانات
```
Microsoft.Data.SqlClient.SqlException: A network-related or instance-specific error occurred
```

**الحلول:**
1. تأكد من تشغيل SQL Server
2. تحقق من Connection String
3. تأكد من صحة اسم الخادم

### خطأ في .NET SDK
```
The specified framework 'Microsoft.NETCore.App', version '8.0.0' was not found
```

**الحل:**
- ثبت .NET 8 SDK من الموقع الرسمي

### خطأ في NuGet Packages
```
Package restore failed
```

**الحلول:**
1. تأكد من الاتصال بالإنترنت
2. شغل `dotnet restore` في Command Line
3. امسح مجلد `bin` و `obj` وأعد البناء

## إعدادات التطوير

### تفعيل Hot Reload
في `Program.cs`، تأكد من وجود:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### تفعيل Logging
في `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## إضافة بيانات تجريبية

### إنشاء مستخدم تجريبي
استخدم Swagger UI أو Postman لإرسال POST request إلى `/api/users`:

```json
{
  "firstName": "أحمد",
  "lastName": "محمد",
  "email": "ahmed@example.com",
  "phoneNumber": "+966501234567",
  "dateOfBirth": "1990-01-01",
  "gender": 1,
  "profileImageUrl": null
}
```

## نشر التطبيق

### النشر المحلي
```bash
dotnet publish -c Release -o ./publish
```

### النشر على IIS
1. انشر التطبيق كما هو موضح أعلاه
2. انسخ مجلد `publish` إلى خادم IIS
3. أنشئ Application Pool جديد بـ .NET 8
4. أنشئ Website جديد يشير إلى مجلد النشر

## الدعم والمساعدة

### الموارد المفيدة
- [وثائق .NET](https://docs.microsoft.com/dotnet/)
- [وثائق Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [وثائق ASP.NET Core](https://docs.microsoft.com/aspnet/core/)

### المشاكل الشائعة
1. **Port مستخدم**: غير المنفذ في `Program.cs`
2. **CORS Error**: تأكد من إعدادات CORS في `Program.cs`
3. **Database Migration**: استخدم `dotnet ef database update`

---

**ملاحظة**: هذا الدليل يغطي التثبيت الأساسي. للاستخدام في الإنتاج، يُنصح بإضافة إعدادات الأمان والأداء المناسبة.

