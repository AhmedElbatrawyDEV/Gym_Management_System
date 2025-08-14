# Enhanced Workout API - نظام إدارة الجيم الشامل المحدث

## التحديثات الجديدة 🆕

تم تطوير النظام الأصلي ليصبح نظام إدارة شامل للجيم مع إضافة الميزات التالية:

### 🔐 نظام المصادقة والتفويض المتقدم
- **JWT Authentication** مع أدوار متعددة
- **Admin Panel** كامل لإدارة النظام
- **أدوار المستخدمين**: Admin, SuperAdmin, User
- **حماية كلمات المرور** بـ SHA256
- **إدارة جلسات المستخدمين** وقفل الحسابات

### 💳 نظام الدفع والاشتراكات الشامل
- **خطط اشتراك متعددة** قابلة للتخصيص
- **معالجة المدفوعات** التلقائية
- **إنشاء الفواتير** تلقائياً
- **تتبع تاريخ المدفوعات** والاشتراكات
- **تجديد تلقائي** للاشتراكات

### 📊 لوحة إدارة متقدمة
- **إدارة شاملة للمستخدمين** مع تحديث الحالات
- **إحصائيات مفصلة** للجيم
- **إدارة الاشتراكات والمدفوعات**
- **إدارة التمارين وبرامج التدريب**
- **تقارير مالية** وإحصائيات الحضور

### 📱 نظام الحضور والفصول
- **تسجيل دخول وخروج** المستخدمين
- **إدارة فصول الجيم** والجداول الزمنية
- **حجز الفصول** وإلغاء الحجوزات
- **تتبع الحضور** والإحصائيات
- **إدارة المدربين** والفصول

### 🔔 نظام الإشعارات المتطور
- **إرسال إشعارات** للمستخدمين والمديرين
- **قوالب إشعارات** قابلة للتخصيص
- **دعم متعدد اللغات** للإشعارات
- **إشعارات تلقائية** للاشتراكات والمدفوعات

### 👥 إدارة المستخدمين المحسنة
- **ملفات شخصية شاملة** مع الصور
- **إدارة حالة المستخدمين** (Active, Inactive, Suspended)
- **تتبع محاولات تسجيل الدخول**
- **أرقام عضوية** فريدة
- **تفضيلات اللغة** للمستخدمين

## الكيانات الجديدة

### Admin (المديرين)
```csharp
public class Admin : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public AdminRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
```

### Subscription (الاشتراكات)
```csharp
public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public bool IsActive { get; set; }
}

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionStatus Status { get; set; }
    public bool AutoRenew { get; set; }
}
```

### Payment (المدفوعات)
```csharp
public class Payment : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? TransactionId { get; set; }
}

public class Invoice : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid PaymentId { get; set; }
    public string InvoiceNumber { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public InvoiceStatus Status { get; set; }
}
```

### Attendance (الحضور)
```csharp
public class AttendanceRecord : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public int? DurationMinutes { get; set; }
    public ActivityType ActivityType { get; set; }
}

public class GymClass : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? InstructorId { get; set; }
    public int MaxCapacity { get; set; }
    public bool IsActive { get; set; }
}
```

## API Endpoints الجديدة

### Admin Management
```
POST /api/admin/register          - تسجيل مدير جديد
POST /api/admin/login             - تسجيل دخول المدير
GET  /api/admin                   - جميع المديرين
PUT  /api/admin/{id}              - تحديث مدير
DELETE /api/admin/{id}            - حذف مدير
PUT  /api/admin/{id}/change-password - تغيير كلمة المرور
```

### Subscription Management
```
GET  /api/subscriptions/plans     - جميع خطط الاشتراك
POST /api/subscriptions/plans     - إنشاء خطة اشتراك
PUT  /api/subscriptions/plans/{id} - تحديث خطة اشتراك
POST /api/subscriptions/users/{userId}/assign - تخصيص اشتراك
GET  /api/subscriptions/users/{userId} - اشتراكات المستخدم
PUT  /api/subscriptions/users/{id}/extend - تمديد اشتراك
PUT  /api/subscriptions/users/{id}/cancel - إلغاء اشتراك
```

### Payment Management
```
POST /api/payments/process        - معالجة دفعة
GET  /api/payments/{id}           - تفاصيل دفعة
GET  /api/payments/user/{userId}  - مدفوعات المستخدم
GET  /api/payments/{paymentId}/invoice - فاتورة الدفعة
GET  /api/payments/user/{userId}/invoices - فواتير المستخدم
```

### Attendance Management
```
POST /api/attendance/check-in     - تسجيل دخول
PUT  /api/attendance/check-out/{recordId} - تسجيل خروج
GET  /api/attendance/user/{userId} - سجلات حضور المستخدم
GET  /api/attendance/classes      - جميع فصول الجيم
POST /api/attendance/classes      - إنشاء فصل جديد
POST /api/attendance/classes/book/{scheduleId} - حجز فصل
DELETE /api/attendance/classes/book/{bookingId} - إلغاء حجز
```

### Notification Management
```
POST /api/notifications           - إرسال إشعار
GET  /api/notifications/user/{userId} - إشعارات المستخدم
PUT  /api/notifications/{id}/read - تحديد الإشعار كمقروء
POST /api/notifications/templates - إنشاء قالب إشعار
GET  /api/notifications/templates - جميع قوالب الإشعارات
```

## الأمان المحسن

### JWT Configuration
```json
{
  "Jwt": {
    "Key": "ThisIsASecretKeyForJWTTokenGenerationThatShouldBeAtLeast32Characters",
    "Issuer": "WorkoutAPI",
    "Audience": "WorkoutAPIUsers"
  }
}
```

### Authorization Policies
```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "SuperAdmin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});
```

## ميزات الأمان الجديدة

- **تشفير كلمات المرور** بـ SHA256
- **JWT Tokens** مع انتهاء صلاحية
- **تفويض قائم على الأدوار**
- **حماية من محاولات تسجيل الدخول المتكررة**
- **قفل الحسابات** بعد محاولات فاشلة
- **CORS مُفعل** للتطوير

## التشغيل السريع

1. **استنساخ المشروع**
```bash
git clone <repository-url>
cd enhanced_workout_api
```

2. **استعادة الحزم**
```bash
dotnet restore
```

3. **تحديث قاعدة البيانات**
```bash
cd src/WorkoutAPI.Api
dotnet ef database update
```

4. **تشغيل التطبيق**
```bash
dotnet run
```

5. **الوصول للتطبيق**
- API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000`

## بيانات تجريبية

سيتم إنشاء بيانات تجريبية تلقائياً تتضمن:
- مدير افتراضي: `admin@gym.com` / `admin123`
- خطط اشتراك متنوعة
- تمارين باللغتين العربية والإنجليزية
- برامج تدريب جاهزة

## الخطوات التالية

1. **تطوير Frontend** بـ React
2. **إضافة تقارير متقدمة**
3. **تكامل مع بوابات دفع حقيقية**
4. **تطبيق موبايل**
5. **نظام إشعارات push**

## المساهمة والدعم

للمساهمة في تطوير النظام أو الحصول على الدعم:
1. إنشاء Issue للمشاكل أو الاقتراحات
2. Fork المشروع للمساهمة
3. التواصل مع فريق التطوير

---

**ملاحظة**: هذا النظام المحدث يوفر حلاً شاملاً لإدارة الجيم مع جميع الميزات المطلوبة للإدارة الفعالة والمربحة للجيم.

