using AppCar.MinhDA.GUI.Views;
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
    /// Interaction logic for ManagementWindow.xaml
    /// </summary>
    public partial class ManagementWindow : Window
    {
        public ManagementWindow()
        {
            InitializeComponent();
            LoadView("CarView");
        }

        private void MenuButton_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            if (button != null)
            {
                // Lấy tên View từ thuộc tính Tag (ví dụ: "CarView", "BrandView")
                string viewName = button.Tag.ToString();
                LoadView(viewName);
            }
        }

        private void LoadView(string viewName)
        {
            object newView = null;

            // Dùng switch case để tạo thể hiện của UserControl tương ứng
            switch (viewName)
            {
                case "CarView":
                    // Tạo một instance mới của CarView
                    newView = new CarView();
                    break;
                case "BrandView":
                    newView = new BrandView();
                    break;
                case "CarTypeView":
                    newView = new CarTypeView();
                    break;
                case "AccountView":
                    newView = new AccountView();
                    break;
                default:
                    // Có thể đặt một View Lỗi hoặc View Trang Chủ mặc định
                    newView = new TextBlock { Text = $"Không tìm thấy View: {viewName}", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                    break;
            }

            // Gán UserControl mới vào ContentArea
            if (newView != null)
            {
                ContentArea.Content = newView;
            }
        }

        

        private void LogoutButton_Click_1(object sender, RoutedEventArgs e)
        {
            var result = NotificationWindow.ShowYesNo("Xác nhận", "Bạn chắc chắn muốn đăng xuất?", NotificationType.Information);
            if (result == NotificationResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            
        }
    }
}
