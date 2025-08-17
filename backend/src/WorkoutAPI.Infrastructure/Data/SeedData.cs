using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(WorkoutDbContext context)
    {
        // Check if data already exists
        if (await context.Users.AnyAsync() || await context.Exercises.AnyAsync())
        {
            return; // Database has been seeded
        }

        // Seed Users
        await SeedUsers(context);
        
        // Seed Exercises
        await SeedExercises(context);
        
        // Seed Workout Plans
        await SeedWorkoutPlans(context);
        
        // Seed User Workout Plans
        await SeedUserWorkoutPlans(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedUsers(WorkoutDbContext context)
    {
        var users = new List<User>
        {
            new User
            {
                FirstName = "أحمد",
                LastName = "محمد",
                Email = "ahmed.mohamed@example.com",
                PhoneNumber = "+201234567890",
                DateOfBirth = new DateTime(1990, 5, 15),
                Gender = Gender.Male,
                IsActive = true
            },
            new User
            {
                FirstName = "فاطمة",
                LastName = "علي",
                Email = "fatima.ali@example.com",
                PhoneNumber = "+201234567891",
                DateOfBirth = new DateTime(1992, 8, 22),
                Gender = Gender.Female,
                IsActive = true
            },
            new User
            {
                FirstName = "محمد",
                LastName = "أحمد",
                Email = "mohamed.ahmed@example.com",
                PhoneNumber = "+201234567892",
                DateOfBirth = new DateTime(1988, 12, 10),
                Gender = Gender.Male,
                IsActive = true
            },
            new User
            {
                FirstName = "عائشة",
                LastName = "حسن",
                Email = "aisha.hassan@example.com",
                PhoneNumber = "+201234567893",
                DateOfBirth = new DateTime(1995, 3, 8),
                Gender = Gender.Female,
                IsActive = true
            },
            new User
            {
                FirstName = "خالد",
                LastName = "عبدالله",
                Email = "khalid.abdullah@example.com",
                PhoneNumber = "+201234567894",
                DateOfBirth = new DateTime(1985, 7, 30),
                Gender = Gender.Male,
                IsActive = true
            }
        };

        await context.Users.AddRangeAsync(users);
    }

    private static async Task SeedExercises(WorkoutDbContext context)
    {
        var exercises = new List<Exercise>();
        var exerciseTranslations = new List<ExerciseTranslation>();

        // Push Exercises (Chest, Shoulders, Triceps)
        var pushExerciseData = new[]
        {
            new { Code = "BENCH_PRESS", PrimaryMuscle = MuscleGroup.Chest, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Triceps, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-bed", NameEn = "Bench Press", NameAr = "ضغط البنش", DescEn = "Lie on your back, lift the bar or dumbbells up until you fully extend your arms, then slowly lower them towards your chest.", DescAr = "استلق على ظهرك، ارفع البار أو الدمبلز لأعلى حتى تمد الذراعين بالكامل، ثم اخفضهما ببطء نحو الصدر." },
            new { Code = "INCLINE_BENCH_PRESS", PrimaryMuscle = MuscleGroup.Chest, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Shoulders, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-chart-line", NameEn = "Incline Bench Press", NameAr = "ضغط البنش المائل", DescEn = "Same as bench press but on an inclined bench at 45 degrees, focuses on the upper chest.", DescAr = "نفس تمرين ضغط البنش لكن على مقعد مائل بزاوية 45 درجة، يركز على الجزء العلوي من الصدر." },
            new { Code = "CABLE_CHEST_FLY", PrimaryMuscle = MuscleGroup.Chest, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-plane", NameEn = "Cable Chest Fly", NameAr = "تمرين الصدر بالكابل", DescEn = "Stand between cable machine, hold handles, and push arms forward while maintaining slight bend in elbows.", DescAr = "قف بين آلة الكابل، أمسك المقابض، وادفع ذراعيك للأمام مع الحفاظ على انحناء بسيط في المرفقين." },
            new { Code = "PUSH_UPS", PrimaryMuscle = MuscleGroup.Chest, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Triceps, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-hand-paper", NameEn = "Push-ups", NameAr = "تمرين الضغط", DescEn = "Start in plank position, lower your body until chest nearly touches floor, then push back up.", DescAr = "ابدأ في وضع اللوح، اخفض جسمك حتى يلامس الصدر الأرض تقريباً، ثم ادفع للأعلى." },
            new { Code = "SHOULDER_PRESS", PrimaryMuscle = MuscleGroup.Shoulders, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Triceps, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-arrow-up", NameEn = "Shoulder Press", NameAr = "ضغط الكتف", DescEn = "Press dumbbells or barbell overhead from shoulder level to full arm extension.", DescAr = "اضغط الدمبلز أو البار فوق الرأس من مستوى الكتف إلى امتداد الذراع الكامل." },
            new { Code = "LATERAL_RAISES", PrimaryMuscle = MuscleGroup.Shoulders, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-expand-arrows-alt", NameEn = "Lateral Raises", NameAr = "رفع جانبي", DescEn = "Lift dumbbells out to sides until arms are parallel to floor, then lower slowly.", DescAr = "ارفع الدمبلز إلى الجانبين حتى تصبح الذراعان موازيتان للأرض، ثم اخفضهما ببطء." },
            new { Code = "TRICEP_DIPS", PrimaryMuscle = MuscleGroup.Triceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Chest, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-angle-down", NameEn = "Tricep Dips", NameAr = "غطسات الترايسبس", DescEn = "Support body weight on parallel bars or bench, lower body by bending elbows, then push back up.", DescAr = "ادعم وزن الجسم على القضبان المتوازية أو المقعد، اخفض الجسم بثني المرفقين، ثم ادفع للأعلى." },
            new { Code = "TRICEP_PUSHDOWN", PrimaryMuscle = MuscleGroup.Triceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrow-down", NameEn = "Tricep Pushdown", NameAr = "دفع الترايسبس لأسفل", DescEn = "Using cable machine, push the bar down while keeping elbows stationary at your sides.", DescAr = "باستخدام آلة الكابل، ادفع البار لأسفل مع إبقاء المرفقين ثابتين على جانبيك." },
            new { Code = "DIAMOND_PUSHUPS", PrimaryMuscle = MuscleGroup.Triceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Chest, Difficulty = DifficultyLevel.Advanced, Icon = "fas fa-gem", NameEn = "Diamond Push-ups", NameAr = "تمرين الضغط الماسي", DescEn = "Push-ups with hands forming diamond shape, targets triceps more than regular push-ups.", DescAr = "تمرين الضغط مع تشكيل اليدين شكل الماس، يستهدف الترايسبس أكثر من الضغط العادي." },
            new { Code = "CHEST_PRESS", PrimaryMuscle = MuscleGroup.Chest, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Shoulders, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-compress", NameEn = "Chest Press Machine", NameAr = "آلة ضغط الصدر", DescEn = "Seated chest press using machine, push handles forward until arms are extended.", DescAr = "ضغط الصدر جالساً باستخدام الآلة، ادفع المقابض للأمام حتى تمتد الذراعان." },
            new { Code = "FRONT_RAISES", PrimaryMuscle = MuscleGroup.Shoulders, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrow-up", NameEn = "Front Raises", NameAr = "رفع أمامي", DescEn = "Lift dumbbells in front of body to shoulder height, then lower slowly.", DescAr = "ارفع الدمبلز أمام الجسم إلى مستوى الكتف، ثم اخفضهما ببطء." },
            new { Code = "OVERHEAD_PRESS", PrimaryMuscle = MuscleGroup.Shoulders, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Triceps, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-arrow-up", NameEn = "Overhead Press", NameAr = "الضغط فوق الرأس", DescEn = "Press weight overhead from shoulder position to full extension above head.", DescAr = "اضغط الوزن فوق الرأس من وضع الكتف إلى الامتداد الكامل فوق الرأس." }
        };

        // Pull Exercises (Back, Biceps)
        var pullExerciseData = new[]
        {
            new { Code = "PULL_UPS", PrimaryMuscle = MuscleGroup.Back, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Biceps, Difficulty = DifficultyLevel.Advanced, Icon = "fas fa-arrow-up", NameEn = "Pull-ups", NameAr = "العقلة", DescEn = "Hang from bar and pull body up until chin clears the bar, then lower slowly.", DescAr = "تعلق من البار واسحب الجسم لأعلى حتى يتجاوز الذقن البار، ثم اخفض ببطء." },
            new { Code = "LAT_PULLDOWN", PrimaryMuscle = MuscleGroup.Back, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Biceps, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrow-down", NameEn = "Lat Pulldown", NameAr = "سحب عالي", DescEn = "Pull the bar down to chest level while seated, focusing on back muscles.", DescAr = "اسحب البار لأسفل إلى مستوى الصدر أثناء الجلوس، مع التركيز على عضلات الظهر." },
            new { Code = "BENT_OVER_ROW", PrimaryMuscle = MuscleGroup.Back, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Biceps, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-arrows-alt-h", NameEn = "Bent Over Row", NameAr = "التجديف المنحني", DescEn = "Bend forward at hips, pull barbell or dumbbells to lower chest, squeeze shoulder blades.", DescAr = "انحن للأمام عند الوركين، اسحب البار أو الدمبلز إلى أسفل الصدر، اضغط لوحي الكتف." },
            new { Code = "SEATED_ROW", PrimaryMuscle = MuscleGroup.Back, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Biceps, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrows-alt-h", NameEn = "Seated Cable Row", NameAr = "التجديف جالساً", DescEn = "Sit and pull cable handles to torso, keeping back straight and squeezing shoulder blades.", DescAr = "اجلس واسحب مقابض الكابل إلى الجذع، مع إبقاء الظهر مستقيماً وضغط لوحي الكتف." },
            new { Code = "BICEP_CURLS", PrimaryMuscle = MuscleGroup.Biceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-dumbbell", NameEn = "Bicep Curls", NameAr = "تمرين البايسبس", DescEn = "Curl dumbbells up to shoulder level, keeping elbows stationary at sides.", DescAr = "اثن الدمبلز لأعلى إلى مستوى الكتف، مع إبقاء المرفقين ثابتين على الجانبين." },
            new { Code = "HAMMER_CURLS", PrimaryMuscle = MuscleGroup.Biceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-hammer", NameEn = "Hammer Curls", NameAr = "تمرين المطرقة", DescEn = "Curl dumbbells with neutral grip (palms facing each other), targets different part of biceps.", DescAr = "اثن الدمبلز بقبضة محايدة (الكفان متقابلان)، يستهدف جزءاً مختلفاً من البايسبس." },
            new { Code = "FACE_PULLS", PrimaryMuscle = MuscleGroup.Shoulders, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Back, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrows-alt-h", NameEn = "Face Pulls", NameAr = "سحب الوجه", DescEn = "Pull cable rope to face level, separating hands at the end, targets rear delts.", DescAr = "اسحب حبل الكابل إلى مستوى الوجه، مع فصل اليدين في النهاية، يستهدف الدلتا الخلفية." },
            new { Code = "REVERSE_FLY", PrimaryMuscle = MuscleGroup.Shoulders, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Back, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-expand", NameEn = "Reverse Fly", NameAr = "الطيران العكسي", DescEn = "Lift dumbbells out to sides while bent forward, targets rear deltoids.", DescAr = "ارفع الدمبلز إلى الجانبين أثناء الانحناء للأمام، يستهدف الدلتا الخلفية." },
            new { Code = "CHIN_UPS", PrimaryMuscle = MuscleGroup.Biceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Back, Difficulty = DifficultyLevel.Advanced, Icon = "fas fa-arrow-up", NameEn = "Chin-ups", NameAr = "العقلة بقبضة عكسية", DescEn = "Pull-ups with underhand grip, emphasizes biceps more than regular pull-ups.", DescAr = "العقلة بقبضة عكسية، تؤكد على البايسبس أكثر من العقلة العادية." },
            new { Code = "PREACHER_CURLS", PrimaryMuscle = MuscleGroup.Biceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-dumbbell", NameEn = "Preacher Curls", NameAr = "تمرين الواعظ", DescEn = "Bicep curls performed on preacher bench for better isolation.", DescAr = "تمرين البايسبس يُؤدى على مقعد الواعظ لعزل أفضل." },
            new { Code = "T_BAR_ROW", PrimaryMuscle = MuscleGroup.Back, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Biceps, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-arrows-alt-h", NameEn = "T-Bar Row", NameAr = "تجديف البار T", DescEn = "Row using T-bar apparatus, excellent for building back thickness.", DescAr = "التجديف باستخدام جهاز البار T، ممتاز لبناء سماكة الظهر." },
            new { Code = "CABLE_CURLS", PrimaryMuscle = MuscleGroup.Biceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-dumbbell", NameEn = "Cable Curls", NameAr = "تمرين البايسبس بالكابل", DescEn = "Bicep curls using cable machine for constant tension throughout the movement.", DescAr = "تمرين البايسبس باستخدام آلة الكابل للحصول على توتر ثابت طوال الحركة." }
        };

        // Leg Exercises (Quadriceps, Hamstrings, Glutes, Calves)
        var legExerciseData = new[]
        {
            new { Code = "SQUATS", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Glutes, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-arrow-down", NameEn = "Squats", NameAr = "القرفصاء", DescEn = "Lower body by bending knees and hips, keep chest up and knees behind toes.", DescAr = "اخفض الجسم بثني الركبتين والوركين، حافظ على الصدر مرفوعاً والركبتين خلف أصابع القدم." },
            new { Code = "DEADLIFTS", PrimaryMuscle = MuscleGroup.Hamstrings, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Back, Difficulty = DifficultyLevel.Advanced, Icon = "fas fa-weight-hanging", NameEn = "Deadlifts", NameAr = "الرفعة المميتة", DescEn = "Lift barbell from floor to hip level, keeping back straight and core engaged.", DescAr = "ارفع البار من الأرض إلى مستوى الورك، مع إبقاء الظهر مستقيماً والجذع مشدوداً." },
            new { Code = "LUNGES", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Glutes, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-walking", NameEn = "Lunges", NameAr = "الاندفاع", DescEn = "Step forward into lunge position, lower back knee toward ground, then return to start.", DescAr = "اخطو للأمام في وضع الاندفاع، اخفض الركبة الخلفية نحو الأرض، ثم عد للبداية." },
            new { Code = "LEG_PRESS", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Glutes, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-compress", NameEn = "Leg Press", NameAr = "ضغط الأرجل", DescEn = "Press weight with legs while seated in leg press machine.", DescAr = "اضغط الوزن بالأرجل أثناء الجلوس في آلة ضغط الأرجل." },
            new { Code = "LEG_CURLS", PrimaryMuscle = MuscleGroup.Hamstrings, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-undo", NameEn = "Leg Curls", NameAr = "ثني الأرجل", DescEn = "Curl legs up while lying face down on leg curl machine.", DescAr = "اثن الأرجل لأعلى أثناء الاستلقاء على البطن في آلة ثني الأرجل." },
            new { Code = "LEG_EXTENSIONS", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-expand", NameEn = "Leg Extensions", NameAr = "مد الأرجل", DescEn = "Extend legs while seated in leg extension machine.", DescAr = "مد الأرجل أثناء الجلوس في آلة مد الأرجل." },
            new { Code = "CALF_RAISES", PrimaryMuscle = MuscleGroup.Calves, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrow-up", NameEn = "Calf Raises", NameAr = "رفع السمانة", DescEn = "Rise up on toes as high as possible, then lower slowly.", DescAr = "ارتفع على أصابع القدم أعلى ما يمكن، ثم اخفض ببطء." },
            new { Code = "BULGARIAN_SPLIT_SQUATS", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Glutes, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-walking", NameEn = "Bulgarian Split Squats", NameAr = "القرفصاء البلغارية", DescEn = "Single leg squat with rear foot elevated on bench or platform.", DescAr = "قرفصاء برجل واحدة مع رفع القدم الخلفية على مقعد أو منصة." },
            new { Code = "HIP_THRUSTS", PrimaryMuscle = MuscleGroup.Glutes, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Hamstrings, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-arrow-up", NameEn = "Hip Thrusts", NameAr = "دفع الورك", DescEn = "Thrust hips up while back is on bench, squeeze glutes at the top.", DescAr = "ادفع الوركين لأعلى أثناء وضع الظهر على المقعد، اضغط المؤخرة في الأعلى." },
            new { Code = "GOBLET_SQUATS", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Glutes, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-wine-glass", NameEn = "Goblet Squats", NameAr = "قرفصاء الكأس", DescEn = "Squats while holding dumbbell or kettlebell at chest level.", DescAr = "القرفصاء أثناء حمل الدمبل أو الكيتل بل على مستوى الصدر." },
            new { Code = "ROMANIAN_DEADLIFTS", PrimaryMuscle = MuscleGroup.Hamstrings, SecondaryMuscle = (MuscleGroup?)MuscleGroup.Glutes, Difficulty = DifficultyLevel.Intermediate, Icon = "fas fa-weight-hanging", NameEn = "Romanian Deadlifts", NameAr = "الرفعة الرومانية", DescEn = "Deadlift variation focusing on hamstrings, lower weight by pushing hips back.", DescAr = "تنويع الرفعة المميتة يركز على الهامسترينغ، اخفض الوزن بدفع الوركين للخلف." },
            new { Code = "WALL_SIT", PrimaryMuscle = MuscleGroup.Quadriceps, SecondaryMuscle = (MuscleGroup?)null, Difficulty = DifficultyLevel.Beginner, Icon = "fas fa-chair", NameEn = "Wall Sit", NameAr = "الجلوس على الحائط", DescEn = "Sit against wall with thighs parallel to floor, hold position.", DescAr = "اجلس على الحائط مع جعل الفخذين موازيين للأرض، احتفظ بالوضعية." }
        };

        // Create Exercise entities for Push exercises
        foreach (var pushEx in pushExerciseData)
        {
            var exercise = new Exercise
            {
                Code = pushEx.Code,
                Type = ExerciseType.Push,
                PrimaryMuscleGroup = pushEx.PrimaryMuscle,
                SecondaryMuscleGroup = pushEx.SecondaryMuscle,
                Difficulty = pushEx.Difficulty,
                IconName = pushEx.Icon,
                IsActive = true
            };
            exercises.Add(exercise);

            // Add translations
            exerciseTranslations.Add(new ExerciseTranslation
            {
                ExerciseId = exercise.Id,
                Language = Language.English,
                Name = pushEx.NameEn,
                Description = pushEx.DescEn
            });

            exerciseTranslations.Add(new ExerciseTranslation
            {
                ExerciseId = exercise.Id,
                Language = Language.Arabic,
                Name = pushEx.NameAr,
                Description = pushEx.DescAr
            });
        }

        // Create Exercise entities for Pull exercises
        foreach (var pullEx in pullExerciseData)
        {
            var exercise = new Exercise
            {
                Code = pullEx.Code,
                Type = ExerciseType.Pull,
                PrimaryMuscleGroup = pullEx.PrimaryMuscle,
                SecondaryMuscleGroup = pullEx.SecondaryMuscle,
                Difficulty = pullEx.Difficulty,
                IconName = pullEx.Icon,
                IsActive = true
            };
            exercises.Add(exercise);

            // Add translations
            exerciseTranslations.Add(new ExerciseTranslation
            {
                ExerciseId = exercise.Id,
                Language = Language.English,
                Name = pullEx.NameEn,
                Description = pullEx.DescEn
            });

            exerciseTranslations.Add(new ExerciseTranslation
            {
                ExerciseId = exercise.Id,
                Language = Language.Arabic,
                Name = pullEx.NameAr,
                Description = pullEx.DescAr
            });
        }

        // Create Exercise entities for Leg exercises
        foreach (var legEx in legExerciseData)
        {
            var exercise = new Exercise
            {
                Code = legEx.Code,
                Type = ExerciseType.Legs,
                PrimaryMuscleGroup = legEx.PrimaryMuscle,
                SecondaryMuscleGroup = legEx.SecondaryMuscle,
                Difficulty = legEx.Difficulty,
                IconName = legEx.Icon,
                IsActive = true
            };
            exercises.Add(exercise);

            // Add translations
            exerciseTranslations.Add(new ExerciseTranslation
            {
                ExerciseId = exercise.Id,
                Language = Language.English,
                Name = legEx.NameEn,
                Description = legEx.DescEn
            });

            exerciseTranslations.Add(new ExerciseTranslation
            {
                ExerciseId = exercise.Id,
                Language = Language.Arabic,
                Name = legEx.NameAr,
                Description = legEx.DescAr
            });
        }

        await context.Exercises.AddRangeAsync(exercises);
        await context.ExerciseTranslations.AddRangeAsync(exerciseTranslations);
    }

    private static async Task SeedWorkoutPlans(WorkoutDbContext context)
    {
        var workoutPlans = new List<WorkoutPlan>();
        var workoutPlanTranslations = new List<WorkoutPlanTranslation>();
        var workoutPlanExercises = new List<WorkoutPlanExercise>();

        // Get exercises
        var pushExercises = await context.Exercises.Where(e => e.Type == ExerciseType.Push).Take(12).ToListAsync();
        var pullExercises = await context.Exercises.Where(e => e.Type == ExerciseType.Pull).Take(12).ToListAsync();
        var legExercises = await context.Exercises.Where(e => e.Type == ExerciseType.Legs).Take(12).ToListAsync();

        // Push Day Plan
        var pushPlan = new WorkoutPlan
        {
            Code = "PUSH_DAY_PLAN",
            Type = ExerciseType.Push,
            IsActive = true
        };
        workoutPlans.Add(pushPlan);

        workoutPlanTranslations.Add(new WorkoutPlanTranslation
        {
            WorkoutPlanId = pushPlan.Id,
            Language = Language.English,
            Name = "Push Day Workout",
            Description = "Complete push workout targeting chest, shoulders, and triceps with 12 exercises"
        });

        workoutPlanTranslations.Add(new WorkoutPlanTranslation
        {
            WorkoutPlanId = pushPlan.Id,
            Language = Language.Arabic,
            Name = "يوم الدفع",
            Description = "تمرين دفع كامل يستهدف الصدر والكتف والترايسبس بـ 12 تمريناً"
        });

        // Add exercises to push plan
        for (int i = 0; i < pushExercises.Count; i++)
        {
            workoutPlanExercises.Add(new WorkoutPlanExercise
            {
                WorkoutPlanId = pushPlan.Id,
                ExerciseId = pushExercises[i].Id,
                Order = i + 1,
                DefaultSets = i < 4 ? 4 : 3, // First 4 exercises have 4 sets, rest have 3
                DefaultReps = i < 8 ? 12 : 15, // First 8 exercises have 12 reps, rest have 15
                DefaultRestTime = TimeSpan.FromSeconds(60)
            });
        }

        // Pull Day Plan
        var pullPlan = new WorkoutPlan
        {
            Code = "PULL_DAY_PLAN",
            Type = ExerciseType.Pull,
            IsActive = true
        };
        workoutPlans.Add(pullPlan);

        workoutPlanTranslations.Add(new WorkoutPlanTranslation
        {
            WorkoutPlanId = pullPlan.Id,
            Language = Language.English,
            Name = "Pull Day Workout",
            Description = "Complete pull workout targeting back, biceps, and rear delts with 12 exercises"
        });

        workoutPlanTranslations.Add(new WorkoutPlanTranslation
        {
            WorkoutPlanId = pullPlan.Id,
            Language = Language.Arabic,
            Name = "يوم السحب",
            Description = "تمرين سحب كامل يستهدف الظهر والبايسبس والدلتا الخلفية بـ 12 تمريناً"
        });

        // Add exercises to pull plan
        for (int i = 0; i < pullExercises.Count; i++)
        {
            workoutPlanExercises.Add(new WorkoutPlanExercise
            {
                WorkoutPlanId = pullPlan.Id,
                ExerciseId = pullExercises[i].Id,
                Order = i + 1,
                DefaultSets = i < 4 ? 4 : 3,
                DefaultReps = i < 8 ? 12 : 15,
                DefaultRestTime = TimeSpan.FromSeconds(60)
            });
        }

        // Legs Day Plan
        var legsPlan = new WorkoutPlan
        {
            Code = "LEGS_DAY_PLAN",
            Type = ExerciseType.Legs,
            IsActive = true
        };
        workoutPlans.Add(legsPlan);

        workoutPlanTranslations.Add(new WorkoutPlanTranslation
        {
            WorkoutPlanId = legsPlan.Id,
            Language = Language.English,
            Name = "Legs Day Workout",
            Description = "Complete legs workout targeting quadriceps, hamstrings, glutes, and calves with 12 exercises"
        });

        workoutPlanTranslations.Add(new WorkoutPlanTranslation
        {
            WorkoutPlanId = legsPlan.Id,
            Language = Language.Arabic,
            Name = "يوم الأرجل",
            Description = "تمرين أرجل كامل يستهدف الكوادريسبس والهامسترينغ والمؤخرة والسمانة بـ 12 تمريناً"
        });

        // Add exercises to legs plan
        for (int i = 0; i < legExercises.Count; i++)
        {
            workoutPlanExercises.Add(new WorkoutPlanExercise
            {
                WorkoutPlanId = legsPlan.Id,
                ExerciseId = legExercises[i].Id,
                Order = i + 1,
                DefaultSets = i < 4 ? 4 : 3,
                DefaultReps = i < 8 ? 12 : 15,
                DefaultRestTime = TimeSpan.FromSeconds(90) // Legs need more rest
            });
        }

        await context.WorkoutPlans.AddRangeAsync(workoutPlans);
        await context.WorkoutPlanTranslations.AddRangeAsync(workoutPlanTranslations);
        await context.WorkoutPlanExercises.AddRangeAsync(workoutPlanExercises);
    }

    private static async Task SeedUserWorkoutPlans(WorkoutDbContext context)
    {
        var users = await context.Users.ToListAsync();
        var workoutPlans = await context.WorkoutPlans.ToListAsync();
        var userWorkoutPlans = new List<UserWorkoutPlan>();

        // Assign workout plans to users
        foreach (var user in users)
        {
            foreach (var plan in workoutPlans)
            {
                userWorkoutPlans.Add(new UserWorkoutPlan
                {
                    UserId = user.Id,
                    WorkoutPlanId = plan.Id,
                    AssignedDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 15)),
                    IsActive = true
                });
            }
        }

        await context.UserWorkoutPlans.AddRangeAsync(userWorkoutPlans);
    }
}

