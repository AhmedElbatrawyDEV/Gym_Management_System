# نظام إدارة الجيم الشامل - Complete Gym Management System

## نظرة عامة
نظام إدارة جيم شامل ومتطور يتكون من:
- **Backend API**: مطور بـ C#/.NET 8 مع Entity Framework Core
- **Frontend**: مطور بـ React 18 مع Tailwind CSS و shadcn/ui

## الميزات الرئيسية

### 🔐 نظام المصادقة والتفويض
- JWT Authentication مع أدوار متعددة (Admin, User)
- حماية كلمات المرور بـ SHA256
- إدارة جلسات المستخدمين

### 👥 إدارة المستخدمين الشاملة
- تسجيل وتسجيل دخول المستخدمين
- ملفات شخصية مفصلة
- إدارة حالة المستخدمين (Active, Inactive, Suspended)
- أرقام عضوية فريدة

### 🏋️ إدارة التمارين وبرامج التدريب
- مكتبة شاملة للتمارين مع دعم متعدد اللغات
- إنشاء وإدارة برامج التدريب المخصصة
- تخصيص برامج التدريب للمستخدمين
- تتبع جلسات التدريب والتقدم

### 💳 نظام الدفع والاشتراكات
- إدارة خطط الاشتراك المتعددة
- معالجة المدفوعات
- إنشاء الفواتير تلقائياً
- تتبع تاريخ المدفوعات

### 📊 لوحة الإدارة
- إدارة شاملة للمستخدمين
- إحصائيات مفصلة
- إدارة الاشتراكات والمدفوعات
- إدارة التمارين وبرامج التدريب

### 📱 نظام الحضور
- تسجيل دخول وخروج المستخدمين
- إدارة فصول الجيم والجداول الزمنية
- حجز الفصول وإلغاء الحجوزات
- تتبع الحضور والإحصائيات

### 🔔 نظام الإشعارات
- إرسال إشعارات للمستخدمين
- قوالب إشعارات قابلة للتخصيص
- دعم متعدد اللغات

## هيكل المشروع

```
complete_gym_system/
├── backend/                    # C#/.NET API
│   ├── src/
│   │   ├── WorkoutAPI.Api/     # API Controllers
│   │   ├── WorkoutAPI.Application/  # Business Logic
│   │   ├── WorkoutAPI.Domain/  # Domain Models
│   │   └── WorkoutAPI.Infrastructure/  # Data Access
│   ├── README.md
│   ├── ENHANCED_README.md
│   ├── DEPLOYMENT_GUIDE.md
│   └── WorkoutAPI.sln
├── frontend/                   # React Application
│   ├── src/
│   │   ├── components/         # React Components
│   │   ├── assets/            # Static Assets
│   │   └── App.jsx            # Main App
│   ├── package.json
│   └── vite.config.js
└── README.md                   # هذا الملف
```

## متطلبات النظام

### Backend (C#/.NET)
- .NET 8.0 SDK أو أحدث
- SQL Server 2019+ أو SQL Server Express
- Visual Studio 2022 أو VS Code

### Frontend (React)
- Node.js 18+ 
- npm أو pnpm
- متصفح حديث

## التثبيت والتشغيل

### 1. تشغيل Backend

```bash
# الانتقال إلى مجلد Backend
cd backend

# استعادة الحزم
dotnet restore

# بناء المشروع
dotnet build

# تشغيل API
cd src/WorkoutAPI.Api
dotnet run
```

سيعمل API على: `https://localhost:5001` أو `http://localhost:5000`

### 2. تشغيل Frontend

```bash
# الانتقال إلى مجلد Frontend
cd frontend

# تثبيت التبعيات
pnpm install

# تشغيل التطبيق
pnpm run dev
```

سيعمل التطبيق على: `http://localhost:5173`

## إعداد قاعدة البيانات

### SQL Server
```sql
-- إنشاء قاعدة بيانات جديدة
CREATE DATABASE WorkoutApiDb;

-- إنشاء مستخدم للتطبيق
CREATE LOGIN WorkoutApiUser WITH PASSWORD = 'YourStrongPassword123!';
USE WorkoutApiDb;
CREATE USER WorkoutApiUser FOR LOGIN WorkoutApiUser;
ALTER ROLE db_owner ADD MEMBER WorkoutApiUser;
```

### تحديث Connection String
في ملف `backend/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WorkoutApiDb;User Id=WorkoutApiUser;Password=YourStrongPassword123!;TrustServerCertificate=true;"
  }
}
```

## اختبار النظام

### تسجيل الدخول للاختبار
يمكنك استخدام أي بريد إلكتروني وكلمة مرور للاختبار:

**للمدير:**
- البريد الإلكتروني: `admin@gym.com`
- كلمة المرور: `password123`

**للمستخدم:**
- البريد الإلكتروني: `user@gym.com`
- كلمة المرور: `password123`

## API Documentation

عند تشغيل Backend، يمكن الوصول إلى Swagger UI على:
`https://localhost:5001/swagger`

## الميزات المتقدمة

### 1. نظام الأدوار
- **Admin**: وصول كامل لجميع الميزات
- **User**: وصول محدود للميزات الشخصية

### 2. الأمان
- تشفير كلمات المرور
- JWT Tokens للمصادقة
- حماية من CORS

### 3. قاعدة البيانات
- Entity Framework Core
- Code First Migrations
- علاقات معقدة بين الجداول

### 4. واجهة المستخدم
- تصميم متجاوب (Responsive)
- دعم كامل للعربية (RTL)
- رسوم بيانية تفاعلية
- تجربة مستخدم حديثة

## النشر (Deployment)

### Backend
- يمكن نشره على IIS، Azure، أو Docker
- راجع `DEPLOYMENT_GUIDE.md` للتفاصيل

### Frontend
- يمكن نشره على Netlify، Vercel، أو أي خدمة استضافة
- بناء للإنتاج: `pnpm run build`

## المساهمة

1. Fork المشروع
2. إنشاء branch جديد (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push إلى Branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

## الترخيص

هذا المشروع مرخص تحت رخصة MIT - راجع ملف `LICENSE` للتفاصيل.

## الدعم

للحصول على الدعم، يرجى فتح issue في GitHub أو التواصل عبر البريد الإلكتروني.

---

**تم تطوير هذا النظام بعناية ليكون حلاً شاملاً لإدارة الأندية الرياضية والجيمات.**

