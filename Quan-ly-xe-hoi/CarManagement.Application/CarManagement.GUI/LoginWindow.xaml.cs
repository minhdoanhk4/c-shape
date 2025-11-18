using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarManagement.GUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // 1. Ngăn không cho link mở trình duyệt web mặc định
            e.Handled = true;

            // 2. Logic mở màn hình đăng ký (RegisterWindow)
            // Vì bạn có thể chưa tạo file RegisterWindow, tôi sẽ để tạm MessageBox
            CustomMessageBox.Show("Chuyển sang màn hình Đăng ký tài khoản.");

            
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close(); 
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            // Hỏi người dùng có chắc chắn muốn thoát không
            var wantToQuit = CustomMessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận",MessageBoxButton.YesNo);

            if (wantToQuit == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
