
using global::WorkoutAPI.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;


namespace WorkoutAPI.Infrastructure.Data;


public static class DatabaseSeeder
{
    public static async Task SeedAsync(WorkoutDbContext context)
    {
        try
        {
            await SeedSubscriptionPlansAsync(context);
            await SeedExercisesAsync(context);
            await SeedUsersAsync(context);
            await SeedTrainersAsync(context);
            await SeedGymClassesAsync(context);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to seed database", ex);
        }
    }

    private static async Task SeedSubscriptionPlansAsync(WorkoutDbContext context)
    {
        if (await context.SubscriptionPlans.AnyAsync()) return;

        var subscriptionPlans = new[]
        {
            SubscriptionPlan.CreateNew(
                "Basic Monthly",
                "Access to gym equipment and basic facilities",
                new Money(199, "SAR"),
                30,
                new List<string> { "Gym Access", "Basic Equipment", "Locker Room" }),

            SubscriptionPlan.CreateNew(
                "Standard Monthly",
                "Gym access plus group classes and swimming pool",
                new Money(299, "SAR"),
                30,
                new List<string> { "Gym Access", "Group Classes", "Swimming Pool", "Locker Room", "Towel Service" }),

            SubscriptionPlan.CreateNew(
                "Premium Monthly",
                "Full access including personal training sessions",
                new Money(599, "SAR"),
                30,
                new List<string> { "Gym Access", "Group Classes", "Swimming Pool", "Personal Training", "Nutritionist Consultation", "Sauna & Steam Room" }),

            SubscriptionPlan.CreateNew(
                "Annual Basic",
                "Basic annual membership with discount",
                new Money(1999, "SAR"),
                365,
                new List<string> { "Gym Access", "Basic Equipment", "Locker Room", "Annual Discount" }),

            SubscriptionPlan.CreateNew(
                "Student Monthly",
                "Special rate for students with valid ID",
                new Money(149, "SAR"),
                30,
                new List<string> { "Gym Access", "Basic Equipment", "Study Area", "Student Discount" }),

            SubscriptionPlan.CreateNew(
                "Corporate Package",
                "Special corporate rates for companies",
                new Money(4999, "SAR"),
                30,
                new List<string> { "Multiple Memberships", "Corporate Wellness Programs", "Flexible Hours", "Health Assessments" })
        };

        await context.SubscriptionPlans.AddRangeAsync(subscriptionPlans);
    }

    private static async Task SeedExercisesAsync(WorkoutDbContext context)
    {
        if (await context.Exercises.AnyAsync()) return;

        var exercises = new[]
        {
            // Chest Exercises
            CreateExercise("BENCH_PRESS", ExerciseType.Strength, MuscleGroup.Chest, DifficultyLevel.Intermediate, MuscleGroup.Shoulders, "bench"),
            CreateExercise("INCLINE_PRESS", ExerciseType.Strength, MuscleGroup.Chest, DifficultyLevel.Intermediate, MuscleGroup.Shoulders, "incline"),
            CreateExercise("PUSHUP", ExerciseType.Strength, MuscleGroup.Chest, DifficultyLevel.Beginner, MuscleGroup.Triceps, "pushup"),
            CreateExercise("CHEST_FLY", ExerciseType.Strength, MuscleGroup.Chest, DifficultyLevel.Beginner, null, "fly"),
            CreateExercise("DIPS", ExerciseType.Strength, MuscleGroup.Chest, DifficultyLevel.Intermediate, MuscleGroup.Triceps, "dips"),

            // Back Exercises
            CreateExercise("PULL_UP", ExerciseType.Strength, MuscleGroup.Back, DifficultyLevel.Advanced, MuscleGroup.Biceps, "pullup"),
            CreateExercise("LAT_PULLDOWN", ExerciseType.Strength, MuscleGroup.Back, DifficultyLevel.Beginner, MuscleGroup.Biceps, "lat"),
            CreateExercise("BENT_ROW", ExerciseType.Strength, MuscleGroup.Back, DifficultyLevel.Intermediate, MuscleGroup.Biceps, "row"),
            CreateExercise("DEADLIFT", ExerciseType.Strength, MuscleGroup.Back, DifficultyLevel.Advanced, MuscleGroup.Glutes, "deadlift"),
            CreateExercise("T_BAR_ROW", ExerciseType.Strength, MuscleGroup.Back, DifficultyLevel.Intermediate, MuscleGroup.Biceps, "tbar"),

            // Shoulder Exercises
            CreateExercise("SHOULDER_PRESS", ExerciseType.Strength, MuscleGroup.Shoulders, DifficultyLevel.Beginner, MuscleGroup.Triceps, "press"),
            CreateExercise("LATERAL_RAISE", ExerciseType.Strength, MuscleGroup.Shoulders, DifficultyLevel.Beginner, null, "lateral"),
            CreateExercise("REAR_DELT_FLY", ExerciseType.Strength, MuscleGroup.Shoulders, DifficultyLevel.Beginner, null, "reardelt"),
            CreateExercise("UPRIGHT_ROW", ExerciseType.Strength, MuscleGroup.Shoulders, DifficultyLevel.Intermediate, MuscleGroup.Biceps, "upright"),

            // Arm Exercises
            CreateExercise("BICEP_CURL", ExerciseType.Strength, MuscleGroup.Biceps, DifficultyLevel.Beginner, null, "curl"),
            CreateExercise("HAMMER_CURL", ExerciseType.Strength, MuscleGroup.Biceps, DifficultyLevel.Beginner, null, "hammer"),
            CreateExercise("TRICEP_EXTENSION", ExerciseType.Strength, MuscleGroup.Triceps, DifficultyLevel.Beginner, null, "extension"),
            CreateExercise("TRICEP_PUSHDOWN", ExerciseType.Strength, MuscleGroup.Triceps, DifficultyLevel.Beginner, null, "pushdown"),

            // Leg Exercises
            CreateExercise("SQUAT", ExerciseType.Strength, MuscleGroup.Quadriceps, DifficultyLevel.Intermediate, MuscleGroup.Glutes, "squat"),
            CreateExercise("LEG_PRESS", ExerciseType.Strength, MuscleGroup.Quadriceps, DifficultyLevel.Beginner, MuscleGroup.Glutes, "legpress"),
            CreateExercise("LEG_CURL", ExerciseType.Strength, MuscleGroup.Hamstrings, DifficultyLevel.Beginner, null, "legcurl"),
            CreateExercise("LEG_EXTENSION", ExerciseType.Strength, MuscleGroup.Quadriceps, DifficultyLevel.Beginner, null, "legext"),
            CreateExercise("CALF_RAISE", ExerciseType.Strength, MuscleGroup.Calves, DifficultyLevel.Beginner, null, "calf"),
            CreateExercise("LUNGES", ExerciseType.Strength, MuscleGroup.Quadriceps, DifficultyLevel.Beginner, MuscleGroup.Glutes, "lunge"),

            // Core Exercises
            CreateExercise("PLANK", ExerciseType.Strength, MuscleGroup.Abs, DifficultyLevel.Beginner, MuscleGroup.Core, "plank"),
            CreateExercise("CRUNCHES", ExerciseType.Strength, MuscleGroup.Abs, DifficultyLevel.Beginner, null, "crunch"),
            CreateExercise("RUSSIAN_TWIST", ExerciseType.Strength, MuscleGroup.Obliques, DifficultyLevel.Intermediate, null, "twist"),
            CreateExercise("MOUNTAIN_CLIMBER", ExerciseType.Cardio, MuscleGroup.Core, DifficultyLevel.Intermediate, MuscleGroup.FullBody, "mountain"),

            // Cardio Exercises
            CreateExercise("TREADMILL_RUN", ExerciseType.Cardio, MuscleGroup.LowerBody, DifficultyLevel.Beginner, null, "treadmill"),
            CreateExercise("CYCLING", ExerciseType.Cardio, MuscleGroup.LowerBody, DifficultyLevel.Beginner, null, "bike"),
            CreateExercise("ELLIPTICAL", ExerciseType.Cardio, MuscleGroup.FullBody, DifficultyLevel.Beginner, null, "elliptical"),
            CreateExercise("ROWING", ExerciseType.Cardio, MuscleGroup.FullBody, DifficultyLevel.Intermediate, null, "rowing"),
            CreateExercise("STAIR_CLIMBER", ExerciseType.Cardio, MuscleGroup.LowerBody, DifficultyLevel.Intermediate, null, "stairs"),

            // Functional Exercises
            CreateExercise("BURPEES", ExerciseType.Functional, MuscleGroup.FullBody, DifficultyLevel.Advanced, null, "burpee"),
            CreateExercise("KETTLEBELL_SWING", ExerciseType.Functional, MuscleGroup.FullBody, DifficultyLevel.Intermediate, null, "kettlebell"),
            CreateExercise("BATTLE_ROPES", ExerciseType.Cardio, MuscleGroup.UpperBody, DifficultyLevel.Intermediate, null, "ropes"),
            CreateExercise("BOX_JUMP", ExerciseType.Plyometric, MuscleGroup.LowerBody, DifficultyLevel.Intermediate, null, "boxjump")
        };

        await context.Exercises.AddRangeAsync(exercises);

        // Add translations for exercises
        foreach (var exercise in exercises)
        {
            AddExerciseTranslations(exercise);
        }
    }

    private static Exercise CreateExercise(string code, ExerciseType type, MuscleGroup primary,
                                         DifficultyLevel difficulty, MuscleGroup? secondary = null, string? icon = null)
    {
        return Exercise.CreateNew(code, type, primary, difficulty, secondary, icon);
    }

    private static void AddExerciseTranslations(Exercise exercise)
    {
        var translations = GetExerciseTranslations(exercise.Code);
        foreach (var (language, name, description, instructions) in translations)
        {
            exercise.AddTranslation(language, name, description, instructions);
        }
    }

    private static IEnumerable<(Language language, string name, string description, string instructions)> GetExerciseTranslations(string code)
    {
        var translationMap = new Dictionary<string, (string enName, string arName, string enDesc, string arDesc, string enInstr, string arInstr)>
        {
            ["BENCH_PRESS"] = ("Bench Press", "ضغط البنش", "Chest exercise using barbell", "تمرين الصدر باستخدام البار", "Lie on bench, press bar up and down", "استلق على البنش واضغط البار لأعلى وأسفل"),
            ["INCLINE_PRESS"] = ("Incline Press", "الضغط المائل", "Upper chest exercise", "تمرين الجزء العلوي من الصدر", "Press at 45-degree angle", "اضغط بزاوية 45 درجة"),
            ["PUSHUP"] = ("Push-up", "تمرين الضغط", "Bodyweight chest exercise", "تمرين الصدر بوزن الجسم", "Lower body to ground, push up", "اخفض الجسم للأرض ثم ادفع لأعلى"),
            ["CHEST_FLY"] = ("Chest Fly", "فتح الصدر", "Isolation exercise for chest", "تمرين عزل للصدر", "Bring weights together in arc motion", "اجمع الأوزان معاً بحركة دائرية"),
            ["DIPS"] = ("Dips", "الغطس", "Compound upper body exercise", "تمرين مركب للجزء العلوي", "Lower and raise body between bars", "اخفض وارفع الجسم بين القضبان"),

            ["PULL_UP"] = ("Pull-up", "العقلة", "Back and bicep exercise", "تمرين الظهر والبايسبس", "Pull body up to bar", "اسحب الجسم لأعلى للبار"),
            ["LAT_PULLDOWN"] = ("Lat Pulldown", "سحب الظهر العلوي", "Upper back exercise", "تمرين الظهر العلوي", "Pull bar down to chest", "اسحب البار لأسفل للصدر"),
            ["BENT_ROW"] = ("Bent-over Row", "التجديف المنحني", "Middle back exercise", "تمرين وسط الظهر", "Pull weight to chest while bent over", "اسحب الوزن للصدر أثناء الانحناء"),
            ["DEADLIFT"] = ("Deadlift", "الرفعة الميتة", "Full body compound exercise", "تمرين مركب للجسم كامل", "Lift weight from ground to standing", "ارفع الوزن من الأرض للوقوف"),
            ["T_BAR_ROW"] = ("T-Bar Row", "تجديف التي بار", "Middle back rowing exercise", "تمرين تجديف وسط الظهر", "Pull T-bar to chest", "اسحب التي بار للصدر"),

            ["SHOULDER_PRESS"] = ("Shoulder Press", "ضغط الكتف", "Shoulder strength exercise", "تمرين قوة الكتف", "Press weight overhead", "اضغط الوزن لأعلى الرأس"),
            ["LATERAL_RAISE"] = ("Lateral Raise", "رفع جانبي", "Side deltoid exercise", "تمرين الدلتويد الجانبي", "Raise weights to sides", "ارفع الأوزان للجانبين"),
            ["REAR_DELT_FLY"] = ("Rear Delt Fly", "فتح الكتف الخلفي", "Rear deltoid exercise", "تمرين الكتف الخلفي", "Fly weights backward", "احرك الأوزان للخلف"),
            ["UPRIGHT_ROW"] = ("Upright Row", "التجديف العمودي", "Shoulder and trap exercise", "تمرين الكتف والترابيس", "Pull weight up along body", "اسحب الوزن لأعلى بمحاذاة الجسم"),

            ["BICEP_CURL"] = ("Bicep Curl", "عضلة الباي", "Bicep isolation exercise", "تمرين عزل الباي", "Curl weight up to shoulder", "اثن الوزن لأعلى للكتف"),
            ["HAMMER_CURL"] = ("Hammer Curl", "الكيرل المطرقي", "Bicep and forearm exercise", "تمرين الباي والساعد", "Curl with neutral grip", "اثن بقبضة محايدة"),
            ["TRICEP_EXTENSION"] = ("Tricep Extension", "تمديد الترايسبس", "Tricep isolation exercise", "تمرين عزل الترايسبس", "Extend arm overhead", "مد الذراع لأعلى الرأس"),
            ["TRICEP_PUSHDOWN"] = ("Tricep Pushdown", "ضغط الترايسبس", "Tricep cable exercise", "تمرين الترايسبس بالكابل", "Push cable down", "ادفع الكابل لأسفل"),

            ["SQUAT"] = ("Squat", "القرفصاء", "Lower body compound exercise", "تمرين مركب للجزء السفلي", "Squat down and stand up", "اقرفص لأسفل وقف"),
            ["LEG_PRESS"] = ("Leg Press", "ضغط الرجل", "Quadricep exercise", "تمرين عضلة الفخذ الأمامية", "Press weight with legs", "ادفع الوزن بالرجلين"),
            ["LEG_CURL"] = ("Leg Curl", "ثني الرجل", "Hamstring exercise", "تمرين عضلة الفخذ الخلفية", "Curl heels to glutes", "اثن الكعبين للمؤخرة"),
            ["LEG_EXTENSION"] = ("Leg Extension", "تمديد الرجل", "Quadricep isolation", "عزل عضلة الفخذ الأمامية", "Extend legs straight", "مد الرجلين مستقيمة"),
            ["CALF_RAISE"] = ("Calf Raise", "رفع السمانة", "Calf muscle exercise", "تمرين عضلة السمانة", "Raise up on toes", "ارفع على أطراف الأصابع"),
            ["LUNGES"] = ("Lunges", "الطعنات", "Single leg exercise", "تمرين الرجل الواحدة", "Step forward and lunge down", "اخط للأمام واطعن لأسفل"),

            ["PLANK"] = ("Plank", "البلانك", "Core stability exercise", "تمرين ثبات الجذع", "Hold body straight", "حافظ على الجسم مستقيماً"),
            ["CRUNCHES"] = ("Crunches", "البطن", "Abdominal exercise", "تمرين البطن", "Crunch up towards knees", "اثن لأعلى باتجاه الركبتين"),
            ["RUSSIAN_TWIST"] = ("Russian Twist", "اللف الروسي", "Oblique exercise", "تمرين الجانبين", "Twist from side to side", "الف من جانب لآخر"),
            ["MOUNTAIN_CLIMBER"] = ("Mountain Climber", "متسلق الجبال", "Cardio core exercise", "تمرين كارديو للجذع", "Alternate knees to chest", "بدل الركبتين للصدر"),

            ["TREADMILL_RUN"] = ("Treadmill Run", "الجري على الجهاز", "Cardio running exercise", "تمرين جري كارديو", "Run on treadmill", "اجر على جهاز الجري"),
            ["CYCLING"] = ("Cycling", "ركوب الدراجة", "Cardio cycling exercise", "تمرين كارديو بالدراجة", "Pedal on exercise bike", "ادعس على دراجة التمارين"),
            ["ELLIPTICAL"] = ("Elliptical", "الإليبتيكال", "Low impact cardio", "كارديو قليل التأثير", "Move in elliptical motion", "تحرك بحركة بيضاوية"),
            ["ROWING"] = ("Rowing", "التجديف", "Full body cardio", "كارديو للجسم كامل", "Pull rowing handle", "اسحب مقبض التجديف"),
            ["STAIR_CLIMBER"] = ("Stair Climber", "صاعد الدرج", "Lower body cardio", "كارديو للجزء السفلي", "Step up continuously", "اصعد باستمرار"),

            ["BURPEES"] = ("Burpees", "البيربي", "Full body exercise", "تمرين للجسم كامل", "Squat, jump back, push-up, jump up", "اقرفص، اقفز للخلف، ضغط، اقفز لأعلى"),
            ["KETTLEBELL_SWING"] = ("Kettlebell Swing", "أرجحة الكيتل بيل", "Hip hinge exercise", "تمرين مفصل الورك", "Swing kettlebell up", "أرجح الكيتل بيل لأعلى"),
            ["BATTLE_ROPES"] = ("Battle Ropes", "الحبال القتالية", "High intensity exercise", "تمرين عالي الشدة", "Wave ropes up and down", "حرك الحبال لأعلى وأسفل"),
            ["BOX_JUMP"] = ("Box Jump", "القفز على الصندوق", "Plyometric exercise", "تمرين بليومتري", "Jump onto box", "اقفز على الصندوق")
        };

        if (translationMap.TryGetValue(code, out var translation))
        {
            yield return (Language.English, translation.enName, translation.enDesc, translation.enInstr);
            yield return (Language.Arabic, translation.arName, translation.arDesc, translation.arInstr);
        }
    }

    private static async Task SeedUsersAsync(WorkoutDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var users = new[]
        {
            CreateUser("Ahmed", "Al-Rashid", "ahmed.rashid@gmail.com", "+966501234567", new DateTime(1990, 5, 15), Gender.Male, Language.Arabic),
            CreateUser("Fatima", "Al-Zahra", "fatima.zahra@gmail.com", "+966501234568", new DateTime(1985, 8, 22), Gender.Female, Language.Arabic),
            CreateUser("Mohammed", "Bin-Salem", "mohammed.salem@gmail.com", "+966501234569", new DateTime(1992, 3, 10), Gender.Male, Language.English),
            CreateUser("Aisha", "Al-Mahmoud", "aisha.mahmoud@gmail.com", "+966501234570", new DateTime(1988, 11, 5), Gender.Female, Language.Arabic),
            CreateUser("Omar", "Al-Khaled", "omar.khaled@gmail.com", "+966501234571", new DateTime(1995, 7, 18), Gender.Male, Language.English),
            CreateUser("Norah", "Al-Saud", "norah.saud@gmail.com", "+966501234572", new DateTime(1991, 12, 8), Gender.Female, Language.Arabic),
            CreateUser("Khalid", "Al-Mutairi", "khalid.mutairi@gmail.com", "+966501234573", new DateTime(1987, 4, 25), Gender.Male, Language.Arabic),
            CreateUser("Sarah", "Al-Qahtani", "sarah.qahtani@gmail.com", "+966501234574", new DateTime(1993, 9, 14), Gender.Female, Language.English)
        };

        await context.Users.AddRangeAsync(users);
    }

    private static User CreateUser(string firstName, string lastName, string email, string phone,
                                 DateTime dateOfBirth, Gender gender, Language language)
    {
        var personalInfo = new PersonalInfo(firstName, lastName, dateOfBirth, gender);
        var contactInfo = new ContactInfo(email, phone);
        return User.CreateNew(personalInfo, contactInfo, language);
    }

    private static async Task SeedTrainersAsync(WorkoutDbContext context)
    {
        if (await context.Trainers.AnyAsync()) return;

        // Get some users to make them trainers
        var users = await context.Users.Take(4).ToListAsync();

        var trainers = new[]
        {
            CreateTrainer(users[0].Guid, "Strength Training & Bodybuilding", "ACSM Certified Personal Trainer", 150),
            CreateTrainer(users[1].Guid, "Yoga & Pilates", "RYT-500 Yoga Alliance", 120),
            CreateTrainer(users[2].Guid, "CrossFit & Functional Training", "CrossFit Level 2 Trainer", 180),
            CreateTrainer(users[3].Guid, "Cardio & Weight Loss", "NASM Certified Personal Trainer", 130)
        };

        await context.Trainers.AddRangeAsync(trainers);
    }

    private static Trainer CreateTrainer(Guid userId, string specialization, string certification, decimal hourlyRate)
    {
        return Trainer.CreateNew(userId, specialization, certification, new Money(hourlyRate, "SAR"));
    }

    private static async Task SeedGymClassesAsync(WorkoutDbContext context)
    {
        if (await context.GymClasses.AnyAsync()) return;

        var trainers = await context.Trainers.ToListAsync();

        var gymClasses = new[]
        {
            CreateGymClass("Morning Yoga", "Gentle yoga to start your day", 20, TimeSpan.FromMinutes(60), DifficultyLevel.Beginner, trainers.FirstOrDefault()?.UserId),
            CreateGymClass("HIIT Cardio", "High-intensity interval training", 15, TimeSpan.FromMinutes(45), DifficultyLevel.Intermediate, trainers.Skip(1).FirstOrDefault()?.UserId),
            CreateGymClass("Strength & Conditioning", "Build strength and muscle", 12, TimeSpan.FromMinutes(75), DifficultyLevel.Advanced, trainers.Skip(2).FirstOrDefault()?.UserId),
            CreateGymClass("Pilates Core", "Core strengthening through Pilates", 18, TimeSpan.FromMinutes(50), DifficultyLevel.Beginner, trainers.Skip(1).FirstOrDefault()?.UserId),
            CreateGymClass("CrossFit WOD", "Workout of the Day - CrossFit style", 10, TimeSpan.FromMinutes(60), DifficultyLevel.Advanced, trainers.Skip(2).FirstOrDefault()?.UserId),
            CreateGymClass("Zumba Dance", "Fun dance fitness class", 25, TimeSpan.FromMinutes(55), DifficultyLevel.Beginner, null),
            CreateGymClass("Boxing Fitness", "Non-contact boxing workout", 16, TimeSpan.FromMinutes(60), DifficultyLevel.Intermediate, null),
            CreateGymClass("Spinning", "Indoor cycling workout", 20, TimeSpan.FromMinutes(45), DifficultyLevel.Intermediate, null)
        };

        await context.GymClasses.AddRangeAsync(gymClasses);
    }

    private static GymClass CreateGymClass(string name, string description, int maxCapacity,
                                         TimeSpan duration, DifficultyLevel difficulty, Guid? instructorId = null)
    {
        return GymClass.CreateNew(name, description, maxCapacity, duration, difficulty, instructorId);
    }
}

// ===== SEEDER EXTENSION METHOD =====

public static class DatabaseSeederExtensions
{
    public static async Task<IServiceProvider> SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();

        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Run migrations if any
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            // Seed data
            await DatabaseSeeder.SeedAsync(context);
        }
        catch (Exception ex)
        {
             throw;
        }

        return serviceProvider;
    }
}