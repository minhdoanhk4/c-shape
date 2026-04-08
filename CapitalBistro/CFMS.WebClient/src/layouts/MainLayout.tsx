import { Outlet, Link, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../store/authStore';
import { LayoutDashboard, Users, LogOut, Package, ClipboardList } from 'lucide-react';

const MainLayout = () => {
  const { logout, user } = useAuthStore();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="flex min-h-screen bg-slate-50 dark:bg-slate-950 font-sans">
      {/* Sidebar */}
      <aside className="w-64 bg-white dark:bg-slate-900 border-r border-slate-200 dark:border-slate-800 flex flex-col transition-all duration-300">
        <div className="p-6">
          <div className="flex items-center space-x-2">
            <div className="w-8 h-8 bg-primary-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold">CF</span>
            </div>
            <span className="text-xl font-bold text-slate-800 dark:text-white tracking-tight">CFMS Portal</span>
          </div>
        </div>

        <nav className="flex-1 px-4 space-y-1">
          <SidebarLink to="/" icon={<LayoutDashboard size={20} />} label="Dashboard" />
          <SidebarLink to="/orders" icon={<ClipboardList size={20} />} label="Orders" />
          <SidebarLink to="/inventory" icon={<Package size={20} />} label="Inventory" />
          <SidebarLink to="/staff" icon={<Users size={20} />} label="Staff Management" />
        </nav>

        <div className="p-4 border-t border-slate-200 dark:border-slate-800">
          <button 
            onClick={handleLogout}
            className="flex items-center space-x-3 w-full px-4 py-2.5 text-slate-600 dark:text-slate-400 hover:bg-red-50 dark:hover:bg-red-900/20 hover:text-red-600 dark:hover:text-red-500 rounded-xl transition-all duration-200"
          >
            <LogOut size={20} />
            <span className="font-medium">Logout</span>
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {/* Header */}
        <header className="h-16 bg-white dark:bg-slate-900 border-b border-slate-200 dark:border-slate-800 flex items-center justify-between px-8 sticky top-0 z-10">
          <h2 className="text-lg font-semibold text-slate-700 dark:text-slate-200">
            Overview
          </h2>
          <div className="flex items-center space-x-4">
            <div className="flex items-center space-x-3 px-3 py-1.5 rounded-full bg-slate-100 dark:bg-slate-800">
              <div className="w-8 h-8 rounded-full bg-primary-100 dark:bg-primary-900/30 flex items-center justify-center text-primary-700 dark:text-primary-400 font-bold text-xs">
                {user?.username?.[0]?.toUpperCase() || 'A'}
              </div>
              <div className="flex flex-col">
                <span className="text-sm font-bold text-slate-700 dark:text-slate-200 leading-tight">{user?.username || 'Admin User'}</span>
                <span className="text-[10px] text-slate-500 font-medium uppercase tracking-wider">{user?.role || 'Administrator'}</span>
              </div>
            </div>
          </div>
        </header>

        <main className="flex-1 p-8 overflow-auto">
          <div className="max-w-7xl mx-auto">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
};

const SidebarLink = ({ to, icon, label }: { to: string, icon: React.ReactNode, label: string }) => (
  <Link 
    to={to} 
    className="flex items-center space-x-3 px-4 py-3 text-slate-600 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800 hover:text-primary-600 rounded-xl transition-all duration-200 group"
  >
    <span className="group-hover:scale-110 transition-transform duration-200">{icon}</span>
    <span className="font-medium">{label}</span>
  </Link>
);

export default MainLayout;
