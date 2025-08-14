# Workout API - نظام إدارة التدريب المتكامل

## نظرة عامة

نظام إدارة التدريب المتكامل هو Web API مبني بـ C# و .NET 8 باستخدام مبادئ Domain-Driven Design (DDD). يدعم النظام اللغتين العربية والإنجليزية ويوفر إدارة شاملة للمستخدمين، التمارين، خطط التدريب، والجلسات التدريبية.

## الميزات الرئيسية

- ✅ **إدارة المستخدمين**: إنشاء وتحديث وإدارة ملفات المستخدمين
- ✅ **نظام التمارين**: مكتبة شاملة من التمارين مع الترجمات
- ✅ **خطط التدريب**: Push-Pull-Legs وخطط تدريب مخصصة
- ✅ **تتبع الجلسات**: تسجيل وتتبع جلسات التدريب
- ✅ **دعم اللغات**: العربية والإنجليزية
- ✅ **Domain-Driven Design**: هيكل مشروع متقدم ومنظم
- ✅ **Entity Framework Core**: إدارة قاعدة البيانات
- ✅ **FluentValidation**: تحقق متقدم من البيانات
- ✅ **Mapster**: تحويل البيانات السريع
- ✅ **Swagger**: توثيق API تفاعلي

## هيكل المشروع

```
WorkoutAPI/
├── src/
│   ├── WorkoutAPI.Domain/          # طبقة المجال - الكيانات والقواعد التجارية
│   │   ├── Common/                 # الفئات الأساسية
│   │   ├── Entities/               # كيانات المجال
│   │   ├── Enums/                  # التعدادات
│   │   ├── Events/                 # أحداث المجال
│   │   ├── Interfaces/             # واجهات المستودعات
│   │   └── ValueObjects/           # كائنات القيمة
│   │
│   ├── WorkoutAPI.Infrastructure/  # طبقة البنية التحتية
│   │   ├── Data/                   # DbContext والتكوينات
│   │   └── Repositories/           # تنفيذ المستودعات
│   │
│   ├── WorkoutAPI.Application/     # طبقة التطبيق
│   │   ├── DTOs/                   # كائنات نقل البيانات
│   │   ├── Services/               # خدمات التطبيق
│   │   ├── Validators/             # مدققات البيانات
│   │   └── Mappings/               # تكوينات Mapster
│   │
│   └── WorkoutAPI.Api/             # طبقة API
│       ├── Controllers/            # وحدات التحكم
│       └── Program.cs              # نقطة البداية
│
├── WorkoutAPI.sln                  # ملف الحل
└── README.md                       # هذا الملف
```

## التقنيات المستخدمة

- **Framework**: .NET 8
- **Database**: SQL Server with Entity Framework Core 8
- **Validation**: FluentValidation
- **Mapping**: Mapster
- **Documentation**: Swagger/OpenAPI
- **Architecture**: Domain-Driven Design (DDD)
- **Patterns**: Repository Pattern, Unit of Work

## متطلبات النظام

- .NET 8 SDK
- SQL Server (LocalDB أو SQL Server Express)
- Visual Studio 2022 أو VS Code

## التثبيت والتشغيل

### 1. استنساخ المشروع
```bash
git clone <repository-url>
cd WorkoutAPI
```

### 2. استعادة الحزم
```bash
dotnet restore
```

### 3. تحديث قاعدة البيانات
```bash
cd src/WorkoutAPI.Api
dotnet ef database update
```

### 4. تشغيل التطبيق
```bash
dotnet run
```

التطبيق سيعمل على: `http://localhost:5000`
Swagger UI متاح على: `http://localhost:5000`

## تكوين قاعدة البيانات

### Connection String
قم بتحديث `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkoutDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### إنشاء Migration جديد
```bash
cd src/WorkoutAPI.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../WorkoutAPI.Api
```

## API Endpoints

### Users (المستخدمون)
- `GET /api/users` - جلب جميع المستخدمين النشطين
- `GET /api/users/{id}` - جلب مستخدم بالمعرف
- `GET /api/users/{id}/profile` - جلب ملف المستخدم الشخصي
- `GET /api/users/by-email/{email}` - جلب مستخدم بالإيميل
- `POST /api/users` - إنشاء مستخدم جديد
- `PUT /api/users/{id}` - تحديث مستخدم
- `DELETE /api/users/{id}` - حذف مستخدم (حذف ناعم)
- `GET /api/users/exists/{email}` - فحص وجود مستخدم

### Exercises (التمارين)
- `GET /api/exercises` - جلب جميع التمارين
- `GET /api/exercises/{id}` - جلب تمرين بالمعرف
- `GET /api/exercises/by-type/{type}` - جلب التمارين حسب النوع
- `GET /api/exercises/by-muscle-group/{muscleGroup}` - جلب التمارين حسب المجموعة العضلية
- `GET /api/exercises/by-code/{code}` - جلب تمرين بالكود

## نماذج البيانات الرئيسية

### User (المستخدم)
```csharp
public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    Gender Gender,
    string? ProfileImageUrl
);
```

### Exercise (التمرين)
```csharp
public record ExerciseResponse(
    Guid Id,
    string Code,
    ExerciseType Type,
    MuscleGroup PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    DifficultyLevel Difficulty,
    string? IconName,
    bool IsActive,
    string Name,
    string? Description,
    string? Instructions
);
```

## التحقق من البيانات

يستخدم النظام FluentValidation للتحقق من صحة البيانات:

```csharp
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100);
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
```

## دعم اللغات

النظام يدعم اللغتين العربية والإنجليزية من خلال:
- جداول الترجمة للتمارين وخطط التدريب
- معامل `language` في API endpoints
- دعم RTL في الواجهة الأمامية

## Domain Events

النظام يستخدم Domain Events لفصل الاهتمامات:

```csharp
public class WorkoutSessionStartedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WorkoutSessionId { get; }
    public Guid WorkoutPlanId { get; }
}
```

## الأمان والأداء

- **CORS**: مكون لجميع المصادر للتطوير
- **Validation**: تحقق شامل من البيانات
- **Logging**: تسجيل مفصل للأخطاء والعمليات
- **Error Handling**: معالجة شاملة للأخطاء

## المساهمة

1. Fork المشروع
2. إنشاء branch للميزة الجديدة
3. Commit التغييرات
4. Push إلى Branch
5. إنشاء Pull Request

## الترخيص

هذا المشروع مرخص تحت رخصة MIT.

## الدعم

للدعم والاستفسارات، يرجى إنشاء Issue في المستودع.

---

**ملاحظة**: هذا المشروع تم تطويره باستخدام أفضل الممارسات في .NET وDomain-Driven Design لضمان قابلية الصيانة والتوسع.

