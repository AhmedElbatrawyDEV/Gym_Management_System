import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Progress } from '@/components/ui/progress'
import { 
  Activity, 
  Calendar, 
  Target, 
  Trophy,
  Clock,
  Flame,
  TrendingUp,
  Play,
  CheckCircle,
  Star
} from 'lucide-react'
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts'

const UserDashboard = ({ user }) => {
  const [userStats, setUserStats] = useState({
    totalWorkouts: 0,
    weeklyGoal: 0,
    currentStreak: 0,
    caloriesBurned: 0,
    nextClass: null,
    subscription: null
  })

  const [workoutHistory, setWorkoutHistory] = useState([])
  const [upcomingClasses, setUpcomingClasses] = useState([])
  const [loading, setLoading] = useState(true)

  // Mock data for charts
  const weeklyProgressData = [
    { day: 'السبت', workouts: 1, calories: 350 },
    { day: 'الأحد', workouts: 0, calories: 0 },
    { day: 'الاثنين', workouts: 1, calories: 420 },
    { day: 'الثلاثاء', workouts: 1, calories: 380 },
    { day: 'الأربعاء', workouts: 0, calories: 0 },
    { day: 'الخميس', workouts: 1, calories: 450 },
    { day: 'الجمعة', workouts: 1, calories: 400 },
  ]

  const monthlyStats = [
    { month: 'يناير', workouts: 12, weight: 75 },
    { month: 'فبراير', workouts: 15, weight: 74 },
    { month: 'مارس', workouts: 18, weight: 73 },
    { month: 'أبريل', workouts: 20, weight: 72 },
    { month: 'مايو', workouts: 22, weight: 71 },
    { month: 'يونيو', workouts: 25, weight: 70 },
  ]

  useEffect(() => {
    // Simulate API call
    const fetchUserData = async () => {
      setLoading(true)
      
      // Mock API delay
      await new Promise(resolve => setTimeout(resolve, 1000))
      
      setUserStats({
        totalWorkouts: 125,
        weeklyGoal: 5,
        currentStreak: 7,
        caloriesBurned: 2850,
        nextClass: {
          name: 'تدريب القوة',
          time: '6:00 مساءً',
          instructor: 'أحمد محمد'
        },
        subscription: {
          type: 'شهري',
          expiryDate: '2024-07-15',
          status: 'نشط'
        }
      })

      setWorkoutHistory([
        { id: 1, name: 'تدريب الصدر والذراعين', date: 'اليوم', duration: 45, calories: 420, completed: true },
        { id: 2, name: 'كارديو', date: 'أمس', duration: 30, calories: 350, completed: true },
        { id: 3, name: 'تدريب الظهر', date: 'منذ يومين', duration: 50, calories: 480, completed: true },
        { id: 4, name: 'تدريب الأرجل', date: 'منذ 3 أيام', duration: 60, calories: 520, completed: true },
      ])

      setUpcomingClasses([
        { id: 1, name: 'يوغا الصباح', time: '8:00 صباحاً', date: 'غداً', instructor: 'سارة أحمد', spots: 5 },
        { id: 2, name: 'تدريب القوة', time: '6:00 مساءً', date: 'غداً', instructor: 'أحمد محمد', spots: 2 },
        { id: 3, name: 'زومبا', time: '7:00 مساءً', date: 'بعد غد', instructor: 'فاطمة علي', spots: 8 },
      ])

      setLoading(false)
    }

    fetchUserData()
  }, [])

  const StatCard = ({ title, value, subtitle, icon: Icon, color = 'blue', progress }) => (
    <Card className="hover:shadow-lg transition-shadow">
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-sm font-medium text-gray-600">{title}</CardTitle>
        <Icon className={`h-4 w-4 text-${color}-600`} />
      </CardHeader>
      <CardContent>
        <div className="text-2xl font-bold text-gray-900">{value}</div>
        {subtitle && <p className="text-xs text-gray-600 mt-1">{subtitle}</p>}
        {progress !== undefined && (
          <div className="mt-3">
            <Progress value={progress} className="h-2" />
            <p className="text-xs text-gray-500 mt-1">{progress}% من الهدف الأسبوعي</p>
          </div>
        )}
      </CardContent>
    </Card>
  )

  if (loading) {
    return (
      <div className="space-y-6">
        <div className="animate-pulse">
          <div className="h-8 bg-gray-200 rounded w-1/3 mb-6"></div>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            {[...Array(4)].map((_, i) => (
              <div key={i} className="h-32 bg-gray-200 rounded-lg"></div>
            ))}
          </div>
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <div className="h-80 bg-gray-200 rounded-lg"></div>
            <div className="h-80 bg-gray-200 rounded-lg"></div>
          </div>
        </div>
      </div>
    )
  }

  const weeklyWorkouts = userStats.totalWorkouts % userStats.weeklyGoal
  const weeklyProgressPercent = (weeklyWorkouts / userStats.weeklyGoal) * 100

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">مرحباً، {user?.name}</h1>
          <p className="text-gray-600 mt-1">استمر في التقدم نحو أهدافك الصحية</p>
        </div>
        <div className="flex space-x-3 space-x-reverse">
          <Button variant="outline">
            <Calendar className="ml-2 h-4 w-4" />
            جدول التمارين
          </Button>
          <Button>
            <Play className="ml-2 h-4 w-4" />
            بدء تمرين جديد
          </Button>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard
          title="التمارين هذا الأسبوع"
          value={`${weeklyWorkouts}/${userStats.weeklyGoal}`}
          subtitle="تمارين"
          icon={Activity}
          color="blue"
          progress={weeklyProgressPercent}
        />
        <StatCard
          title="السلسلة الحالية"
          value={`${userStats.currentStreak} أيام`}
          subtitle="استمر في التقدم!"
          icon={Flame}
          color="orange"
        />
        <StatCard
          title="السعرات المحروقة"
          value={userStats.caloriesBurned.toLocaleString()}
          subtitle="هذا الأسبوع"
          icon={Target}
          color="red"
        />
        <StatCard
          title="إجمالي التمارين"
          value={userStats.totalWorkouts}
          subtitle="منذ الانضمام"
          icon={Trophy}
          color="yellow"
        />
      </div>

      {/* Subscription Status */}
      {userStats.subscription && (
        <Card className="bg-gradient-to-r from-indigo-50 to-purple-50 border-indigo-200">
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle className="text-indigo-900">اشتراكك {userStats.subscription.type}</CardTitle>
                <CardDescription className="text-indigo-700">
                  ينتهي في {new Date(userStats.subscription.expiryDate).toLocaleDateString('ar-SA')}
                </CardDescription>
              </div>
              <Badge variant="default" className="bg-green-100 text-green-800">
                {userStats.subscription.status}
              </Badge>
            </div>
          </CardHeader>
        </Card>
      )}

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Weekly Progress */}
        <Card>
          <CardHeader>
            <CardTitle>التقدم الأسبوعي</CardTitle>
            <CardDescription>التمارين والسعرات المحروقة هذا الأسبوع</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={weeklyProgressData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="day" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="workouts" fill="#8884d8" name="التمارين" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Monthly Progress */}
        <Card>
          <CardHeader>
            <CardTitle>التقدم الشهري</CardTitle>
            <CardDescription>عدد التمارين الشهرية</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={monthlyStats}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip />
                <Line type="monotone" dataKey="workouts" stroke="#82ca9d" strokeWidth={2} name="التمارين" />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* Bottom Section */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Recent Workouts */}
        <Card>
          <CardHeader>
            <CardTitle>التمارين الأخيرة</CardTitle>
            <CardDescription>آخر التمارين التي أكملتها</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {workoutHistory.map((workout) => (
                <div key={workout.id} className="flex items-center space-x-4 space-x-reverse p-3 bg-gray-50 rounded-lg">
                  <div className="flex-shrink-0">
                    <CheckCircle className="h-5 w-5 text-green-600" />
                  </div>
                  <div className="flex-1 min-w-0">
                    <p className="text-sm font-medium text-gray-900">{workout.name}</p>
                    <p className="text-xs text-gray-500">{workout.date}</p>
                  </div>
                  <div className="text-left">
                    <p className="text-sm font-medium text-gray-900">{workout.duration} دقيقة</p>
                    <p className="text-xs text-gray-500">{workout.calories} سعرة</p>
                  </div>
                </div>
              ))}
            </div>
            <Button variant="outline" className="w-full mt-4">
              عرض جميع التمارين
            </Button>
          </CardContent>
        </Card>

        {/* Upcoming Classes */}
        <Card>
          <CardHeader>
            <CardTitle>الفصول القادمة</CardTitle>
            <CardDescription>الفصول المتاحة للحجز</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {upcomingClasses.map((classItem) => (
                <div key={classItem.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div className="flex items-center space-x-3 space-x-reverse">
                    <Calendar className="h-5 w-5 text-indigo-600" />
                    <div>
                      <p className="text-sm font-medium text-gray-900">{classItem.name}</p>
                      <p className="text-xs text-gray-500">
                        {classItem.date} - {classItem.time}
                      </p>
                      <p className="text-xs text-gray-500">المدرب: {classItem.instructor}</p>
                    </div>
                  </div>
                  <div className="text-left">
                    <Badge variant="secondary" className="mb-2">
                      {classItem.spots} مقاعد متاحة
                    </Badge>
                    <Button size="sm" className="block">
                      احجز
                    </Button>
                  </div>
                </div>
              ))}
            </div>
            <Button variant="outline" className="w-full mt-4">
              عرض جميع الفصول
            </Button>
          </CardContent>
        </Card>
      </div>

      {/* Next Workout Suggestion */}
      {userStats.nextClass && (
        <Card className="bg-gradient-to-r from-green-50 to-blue-50 border-green-200">
          <CardHeader>
            <CardTitle className="text-green-900 flex items-center">
              <Star className="ml-2 h-5 w-5" />
              التمرين التالي المقترح
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-lg font-semibold text-green-900">{userStats.nextClass.name}</p>
                <p className="text-green-700">
                  اليوم - {userStats.nextClass.time} مع المدرب {userStats.nextClass.instructor}
                </p>
              </div>
              <Button className="bg-green-600 hover:bg-green-700">
                <Play className="ml-2 h-4 w-4" />
                ابدأ الآن
              </Button>
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  )
}

export default UserDashboard

