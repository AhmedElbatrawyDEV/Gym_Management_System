namespace WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Domain.Enums;

// User related enums
public enum Gender {
    Male = 1,
    Female = 2,
    Other = 3,
    PreferNotToSay = 4
}

public enum UserStatus {
    Pending = 0,
    Active = 1,
    Inactive = 2,
    Suspended = 3,
    Deleted = 4
}

public enum Language {
    English = 1,
    Arabic = 2,
    French = 3,
    Spanish = 4,
    German = 5
}

// Exercise related enums
public enum ExerciseType {
    Strength = 1,
    Cardio = 2,
    Flexibility = 3,
    Balance = 4,
    Plyometric = 5,
    Functional = 6,
    Rehabilitation = 7,
    Sport = 8
}

public enum MuscleGroup {
    // Upper Body
    Chest = 1,
    Back = 2,
    Shoulders = 3,
    Biceps = 4,
    Triceps = 5,
    Forearms = 6,

    // Core
    Abs = 7,
    Obliques = 8,
    LowerBack = 9,

    // Lower Body
    Quadriceps = 10,
    Hamstrings = 11,
    Glutes = 12,
    Calves = 13,

    // Full Body
    FullBody = 14,

    // Compound movements
    UpperBody = 15,
    LowerBody = 16,
    Core = 17
}

public enum DifficultyLevel {
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}

// Workout related enums
public enum WorkoutSessionStatus {
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4,
    NoShow = 5
}

public enum ActivityType {
    GeneralWorkout = 1,
    Cardio = 2,
    Strength = 3,
    GroupClass = 4,
    PersonalTraining = 5,
    Swimming = 6,
    Yoga = 7,
    Pilates = 8,
    CrossFit = 9,
    Martial = 10
}

// Subscription related enums
public enum MembershipType {
    Basic = 1,
    Standard = 2,
    Premium = 3,
    VIP = 4,
    Student = 5,
    Senior = 6,
    Corporate = 7,
    Family = 8
}

public enum SubscriptionStatus {
    Active = 1,
    Expired = 2,
    Cancelled = 3,
    Suspended = 4,
    PendingPayment = 5,
    PendingActivation = 6
}

// Payment related enums
public enum PaymentMethod {
    Cash = 1,
    CreditCard = 2,
    DebitCard = 3,
    BankTransfer = 4,
    OnlinePayment = 5,
    ApplePay = 6,
    GooglePay = 7,
    SamsungPay = 8,
    PayPal = 9,
    SADAD = 10,      // Saudi payment gateway
    Mada = 11,       // Saudi debit card network
    STCPay = 12,     // Saudi Telecom payment
    Tabby = 13,      // Buy now, pay later (popular in Saudi)
    Tamara = 14      // Buy now, pay later (popular in Saudi)
}

public enum PaymentStatus {
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5,
    Refunded = 6,
    PartiallyRefunded = 7,
    Expired = 8,
    OnHold = 9
}

public enum InvoiceStatus {
    Draft = 1,
    Sent = 2,
    Viewed = 3,
    Paid = 4,
    Overdue = 5,
    Cancelled = 6,
    Refunded = 7,
    PartiallyPaid = 8
}

// Class and booking related enums
public enum BookingStatus {
    Confirmed = 1,
    Pending = 2,
    Cancelled = 3,
    NoShow = 4,
    Completed = 5,
    Waitlisted = 6,
    CheckedIn = 7
}

public enum ClassStatus {
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4,
    Postponed = 5,
    Full = 6
}

// Days of the week for scheduling
public enum DayOfWeek {
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6
}

// Workout plan related enums
public enum WorkoutPlanType {
    WeightLoss = 1,
    MuscleGain = 2,
    Strength = 3,
    Endurance = 4,
    Flexibility = 5,
    Rehabilitation = 6,
    Sport = 7,
    General = 8,
    Beginner = 9,
    Intermediate = 10,
    Advanced = 11
}

public enum WorkoutPlanStatus {
    Draft = 1,
    Active = 2,
    Inactive = 3,
    Archived = 4,
    UnderReview = 5
}

// Trainer related enums
public enum TrainerStatus {
    Active = 1,
    Inactive = 2,
    OnLeave = 3,
    Suspended = 4,
    Terminated = 5
}

public enum CertificationLevel {
    Basic = 1,
    Intermediate = 2,
    Advanced = 3,
    Master = 4,
    Specialist = 5
}

// Equipment related enums
public enum EquipmentType {
    Cardio = 1,
    Strength = 2,
    FreeWeights = 3,
    Functional = 4,
    Flexibility = 5,
    Accessories = 6,
    Machines = 7,
    Cables = 8
}

public enum EquipmentStatus {
    Available = 1,
    InUse = 2,
    Maintenance = 3,
    OutOfOrder = 4,
    Reserved = 5,
    Cleaning = 6
}

// Attendance related enums
public enum AttendanceStatus {
    CheckedIn = 1,
    CheckedOut = 2,
    Present = 3,
    Absent = 4,
    Late = 5,
    ExcusedAbsence = 6
}

// Notification related enums
public enum NotificationType {
    General = 1,
    ClassReminder = 2,
    PaymentDue = 3,
    PaymentConfirmed = 4,
    MembershipExpiring = 5,
    ClassCancelled = 6,
    NewClass = 7,
    PromotionalOffer = 8,
    WorkoutReminder = 9,
    Achievement = 10
}

public enum NotificationStatus {
    Pending = 1,
    Sent = 2,
    Delivered = 3,
    Read = 4,
    Failed = 5,
    Cancelled = 6
}

public enum NotificationChannel {
    InApp = 1,
    Email = 2,
    SMS = 3,
    Push = 4,
    WhatsApp = 5
}

// Goal and progress related enums
public enum GoalType {
    WeightLoss = 1,
    WeightGain = 2,
    MuscleGain = 3,
    StrengthIncrease = 4,
    EnduranceImprovement = 5,
    FlexibilityImprovement = 6,
    BodyFatReduction = 7,
    GeneralFitness = 8,
    SportPerformance = 9,
    Rehabilitation = 10
}

public enum GoalStatus {
    Active = 1,
    Completed = 2,
    Paused = 3,
    Cancelled = 4,
    Overdue = 5
}

// Time period enums
public enum TimePeriod {
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Quarterly = 4,
    Yearly = 5
}

// Measurement units
public enum WeightUnit {
    Kilograms = 1,
    Pounds = 2,
    Stones = 3
}

public enum DistanceUnit {
    Kilometers = 1,
    Miles = 2,
    Meters = 3,
    Feet = 4
}

public enum TemperatureUnit {
    Celsius = 1,
    Fahrenheit = 2,
    Kelvin = 3
}

// System and audit enums
public enum AuditAction {
    Create = 1,
    Update = 2,
    Delete = 3,
    View = 4,
    Login = 5,
    Logout = 6,
    Export = 7,
    Import = 8,
    Approve = 9,
    Reject = 10
}

public enum SystemRole {
    SuperAdmin = 1,
    Admin = 2,
    Manager = 3,
    Trainer = 4,
    Receptionist = 5,
    Member = 6,
    Guest = 7,
    Maintenance = 8
}

// Priority levels
public enum Priority {
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4,
    Urgent = 5
}

// Status for general use
public enum GeneralStatus {
    Active = 1,
    Inactive = 2,
    Pending = 3,
    Approved = 4,
    Rejected = 5,
    Suspended = 6,
    Deleted = 7,
    Draft = 8,
    Published = 9,
    Archived = 10
}

// Communication preferences
public enum CommunicationPreference {
    Email = 1,
    SMS = 2,
    Phone = 3,
    InApp = 4,
    WhatsApp = 5,
    None = 6
}

// Age groups
public enum AgeGroup {
    Child = 1,        // Under 13
    Teen = 2,         // 13-17
    YoungAdult = 3,   // 18-25
    Adult = 4,        // 26-59
    Senior = 5        // 60+
}

// Fitness levels
public enum FitnessLevel {
    Sedentary = 1,
    LightlyActive = 2,
    ModeratelyActive = 3,
    VeryActive = 4,
    ExtremelyActive = 5
}

// Medical conditions (for workout restrictions)
public enum MedicalCondition {
    None = 0,
    HeartDisease = 1,
    Diabetes = 2,
    Hypertension = 3,
    Arthritis = 4,
    BackPain = 5,
    KneeProblems = 6,
    ShoulderProblems = 7,
    Asthma = 8,
    Pregnancy = 9,
    Other = 10
}

// Body composition metrics
public enum BodyMetric {
    Weight = 1,
    Height = 2,
    BodyFatPercentage = 3,
    MuscleMass = 4,
    BoneMass = 5,
    WaterPercentage = 6,
    BMI = 7,
    VisceralFat = 8,
    MetabolicAge = 9,
    BasalMetabolicRate = 10
}