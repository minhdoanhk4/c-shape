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
using System.Windows.Shapes;

namespace CarManagement.GUI
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            NavigateToLogin();

        }
        // Hàm điều hướng chung
        private void NavigateToLogin()
        {
            var wantToQuit = CustomMessageBox.Show("Mọi thông tin của bạn sẽ bị HỦY! Bạn chắc chắn đóng?", "Xác nhận", MessageBoxButton.YesNo);

            if (wantToQuit  == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close(); // Đóng cửa sổ đăng ký hiện tại
            }
            
        }
    }
}
    
