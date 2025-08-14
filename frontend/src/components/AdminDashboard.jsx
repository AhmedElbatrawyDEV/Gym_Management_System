import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { 
  Users, 
  CreditCard, 
  Activity, 
  TrendingUp, 
  Calendar,
  DollarSign,
  UserPlus,
  AlertCircle,
  CheckCircle,
  Clock
} from 'lucide-react'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, LineChart, Line, PieChart, Pie, Cell } from 'recharts'

const AdminDashboard = ({ user }) => {
  const [stats, setStats] = useState({
    totalUsers: 0,
    activeUsers: 0,
    totalRevenue: 0,
    monthlyRevenue: 0,
    totalWorkouts: 0,
    activeSubscriptions: 0
  })

  const [recentActivities, setRecentActivities] = useState([])
  const [loading, setLoading] = useState(true)

  // Mock data for charts
  const monthlyRevenueData = [
    { month: 'يناير', revenue: 15000 },
    { month: 'فبراير', revenue: 18000 },
    { month: 'مارس', revenue: 22000 },
    { month: 'أبريل', revenue: 25000 },
    { month: 'مايو', revenue: 28000 },
    { month: 'يونيو', revenue: 32000 },
  ]

  const userGrowthData = [
    { month: 'يناير', users: 120 },
    { month: 'فبراير', users: 145 },
    { month: 'مارس', users: 180 },
    { month: 'أبريل', users: 220 },
    { month: 'مايو', users: 265 },
    { month: 'يونيو', users: 310 },
  ]

  const subscriptionData = [
    { name: 'شهري', value: 45, color: '#8884d8' },
    { name: 'ربع سنوي', value: 30, color: '#82ca9d' },
    { name: 'سنوي', value: 25, color: '#ffc658' },
  ]

  useEffect(() => {
    // Simulate API call
    const fetchDashboardData = async () => {
      setLoading(true)
      
      // Mock API delay
      await new Promise(resolve => setTimeout(resolve, 1000))
      
      setStats({
        totalUsers: 310,
        activeUsers: 285,
        totalRevenue: 140000,
        monthlyRevenue: 32000,
        totalWorkouts: 1250,
        activeSubscriptions: 285
      })

      setRecentActivities([
        { id: 1, type: 'user_joined', message: 'انضم أحمد محمد إلى الجيم', time: '5 دقائق', status: 'success' },
        { id: 2, type: 'payment', message: 'تم دفع اشتراك شهري - 500 ريال', time: '15 دقيقة', status: 'success' },
        { id: 3, type: 'workout', message: 'تم إكمال 25 جلسة تدريب اليوم', time: '30 دقيقة', status: 'info' },
        { id: 4, type: 'subscription', message: 'انتهاء اشتراك سارة أحمد قريباً', time: '1 ساعة', status: 'warning' },
        { id: 5, type: 'class', message: 'تم حجز فصل اليوغا بالكامل', time: '2 ساعة', status: 'info' },
      ])

      setLoading(false)
    }

    fetchDashboardData()
  }, [])

  const StatCard = ({ title, value, change, icon: Icon, color = 'blue' }) => (
    <Card className="hover:shadow-lg transition-shadow">
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-sm font-medium text-gray-600">{title}</CardTitle>
        <Icon className={`h-4 w-4 text-${color}-600`} />
      </CardHeader>
      <CardContent>
        <div className="text-2xl font-bold text-gray-900">{value}</div>
        {change && (
          <p className="text-xs text-gray-600 mt-1">
            <span className={`text-${change > 0 ? 'green' : 'red'}-600`}>
              {change > 0 ? '+' : ''}{change}%
            </span>
            {' '}من الشهر الماضي
          </p>
        )}
      </CardContent>
    </Card>
  )

  const ActivityIcon = ({ type }) => {
    switch (type) {
      case 'user_joined':
        return <UserPlus className="h-4 w-4 text-green-600" />
      case 'payment':
        return <DollarSign className="h-4 w-4 text-blue-600" />
      case 'workout':
        return <Activity className="h-4 w-4 text-purple-600" />
      case 'subscription':
        return <AlertCircle className="h-4 w-4 text-orange-600" />
      case 'class':
        return <Calendar className="h-4 w-4 text-indigo-600" />
      default:
        return <CheckCircle className="h-4 w-4 text-gray-600" />
    }
  }

  if (loading) {
    return (
      <div className="space-y-6">
        <div className="animate-pulse">
          <div className="h-8 bg-gray-200 rounded w-1/4 mb-6"></div>
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

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">مرحباً، {user?.name}</h1>
          <p className="text-gray-600 mt-1">إليك نظرة عامة على أداء الجيم اليوم</p>
        </div>
        <div className="flex space-x-3 space-x-reverse">
          <Button variant="outline">
            <Calendar className="ml-2 h-4 w-4" />
            تصدير التقرير
          </Button>
          <Button>
            <UserPlus className="ml-2 h-4 w-4" />
            إضافة مستخدم جديد
          </Button>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard
          title="إجمالي المستخدمين"
          value={stats.totalUsers.toLocaleString()}
          change={12}
          icon={Users}
          color="blue"
        />
        <StatCard
          title="المستخدمون النشطون"
          value={stats.activeUsers.toLocaleString()}
          change={8}
          icon={Activity}
          color="green"
        />
        <StatCard
          title="الإيرادات الشهرية"
          value={`${stats.monthlyRevenue.toLocaleString()} ريال`}
          change={15}
          icon={DollarSign}
          color="yellow"
        />
        <StatCard
          title="الاشتراكات النشطة"
          value={stats.activeSubscriptions.toLocaleString()}
          change={5}
          icon={CreditCard}
          color="purple"
        />
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Revenue Chart */}
        <Card>
          <CardHeader>
            <CardTitle>الإيرادات الشهرية</CardTitle>
            <CardDescription>نمو الإيرادات خلال الأشهر الستة الماضية</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={monthlyRevenueData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip formatter={(value) => [`${value.toLocaleString()} ريال`, 'الإيرادات']} />
                <Bar dataKey="revenue" fill="#8884d8" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* User Growth Chart */}
        <Card>
          <CardHeader>
            <CardTitle>نمو المستخدمين</CardTitle>
            <CardDescription>عدد المستخدمين الجدد شهرياً</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={userGrowthData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip formatter={(value) => [value, 'المستخدمون']} />
                <Line type="monotone" dataKey="users" stroke="#82ca9d" strokeWidth={2} />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* Bottom Section */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Recent Activities */}
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>الأنشطة الأخيرة</CardTitle>
            <CardDescription>آخر الأحداث في النظام</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {recentActivities.map((activity) => (
                <div key={activity.id} className="flex items-center space-x-4 space-x-reverse">
                  <ActivityIcon type={activity.type} />
                  <div className="flex-1 min-w-0">
                    <p className="text-sm font-medium text-gray-900">{activity.message}</p>
                    <p className="text-xs text-gray-500 flex items-center">
                      <Clock className="ml-1 h-3 w-3" />
                      منذ {activity.time}
                    </p>
                  </div>
                  <Badge variant={activity.status === 'success' ? 'default' : activity.status === 'warning' ? 'destructive' : 'secondary'}>
                    {activity.status === 'success' ? 'مكتمل' : activity.status === 'warning' ? 'تحذير' : 'معلومات'}
                  </Badge>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Subscription Distribution */}
        <Card>
          <CardHeader>
            <CardTitle>توزيع الاشتراكات</CardTitle>
            <CardDescription>أنواع الاشتراكات النشطة</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={200}>
              <PieChart>
                <Pie
                  data={subscriptionData}
                  cx="50%"
                  cy="50%"
                  innerRadius={40}
                  outerRadius={80}
                  paddingAngle={5}
                  dataKey="value"
                >
                  {subscriptionData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip formatter={(value) => [`${value}%`, 'النسبة']} />
              </PieChart>
            </ResponsiveContainer>
            <div className="mt-4 space-y-2">
              {subscriptionData.map((item, index) => (
                <div key={index} className="flex items-center justify-between">
                  <div className="flex items-center space-x-2 space-x-reverse">
                    <div className={`w-3 h-3 rounded-full`} style={{ backgroundColor: item.color }}></div>
                    <span className="text-sm text-gray-600">{item.name}</span>
                  </div>
                  <span className="text-sm font-medium">{item.value}%</span>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}

export default AdminDashboard

