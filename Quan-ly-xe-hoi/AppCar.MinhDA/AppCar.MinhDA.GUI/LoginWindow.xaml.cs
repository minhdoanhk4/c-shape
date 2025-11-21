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

namespace AppCar.MinhDA.GUI
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = NotificationWindow.ShowYesNo("Xác nhận", "Bạn chắc chắn muốn đóng ứng dụng?", NotificationType.Information);
            if (result == NotificationResult.Yes)
            {
                Application.Current.Shutdown();
            }
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationWindow.Show("Cảnh báo", "Email hoặc password chưa đúng!? Vui lòng thử lại", NotificationType.Error);
            txtPassword.Clear();
        }
    }
}
