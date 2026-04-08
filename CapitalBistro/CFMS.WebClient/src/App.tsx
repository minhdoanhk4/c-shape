import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import ProtectedRoute from './components/ProtectedRoute';
import Login from './pages/Login';

const Dashboard = () => (
  <div className="space-y-6">
    <div className="flex flex-col space-y-2">
      <h1 className="text-3xl font-black text-slate-800 dark:text-white tracking-tight">Overview Dashboard</h1>
      <p className="text-slate-500 font-medium">Welcome back to the Capital Franchise Supply Chain Management Portal.</p>
    </div>
    
    {/* Placeholder for Dashboard Widgets */}
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <StatsCard title="Total Orders" value="1,248" change="+12.5%" />
      <StatsCard title="Active Shipments" value="42" change="+3.2%" />
      <StatsCard title="Inventory Alerts" value="8" change="-2.1%" isNegative />
      <StatsCard title="Revenue (MTD)" value="$42,850" change="+8.4%" />
    </div>
  </div>
);

const StatsCard = ({ title, value, change, isNegative }: { title: string, value: string, change: string, isNegative?: boolean }) => (
  <div className="bg-white dark:bg-slate-900 p-6 rounded-3xl border border-slate-100 dark:border-slate-800 shadow-sm hover:shadow-md transition-shadow duration-300">
    <p className="text-sm font-bold text-slate-500 uppercase tracking-wider">{title}</p>
    <div className="mt-4 flex items-end justify-between">
      <h3 className="text-2xl font-black text-slate-800 dark:text-white">{value}</h3>
      <span className={`text-xs font-bold px-2 py-1 rounded-lg ${isNegative ? 'bg-red-50 text-red-600' : 'bg-green-50 text-green-600'}`}>
        {change}
      </span>
    </div>
  </div>
);

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<Login />} />

        {/* Protected Routes */}
        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/" element={<Dashboard />} />
            <Route path="/orders" element={<div className="text-2xl font-bold">Orders Management</div>} />
            <Route path="/inventory" element={<div className="text-2xl font-bold">Inventory Control</div>} />
            <Route path="/staff" element={<div className="text-2xl font-bold">Staff Directory</div>} />
          </Route>
        </Route>

        {/* Fallback */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
