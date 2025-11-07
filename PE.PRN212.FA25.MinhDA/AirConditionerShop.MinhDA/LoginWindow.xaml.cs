using AirConditionerShop.BLL.Services;
using AirConditionerShop.DAL.Models;
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

namespace AirConditionerShop.MinhDA
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private StaffMemberService _service = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // lấy email và pass đã nhập gửi cho service -> repo -> dbcontext
            string email = txtEmail.Text;
            string pass = txtPassword.Password;
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("both email and password are required", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            StaffMember? acc = _service.Authenticate(email);
            //StaffMember? acc = _service.Authenticate(email,pass);
            if (acc == null)
            {
                MessageBox.Show("Email doesn't exited. Sign Up now!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //email tồn tài r thì check pass
            if (acc.Password != pass)
            {
                MessageBox.Show("Password isn't correct. Reset password now!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //phân quyền
            if (acc.Role == 3)
            {
                MessageBox.Show("Bạn không phận sự miễn vào", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }




            MainWindow main = new();

            //gửi role trc khi show
            main.Role = acc.Role;

            main.Show();
            this.Hide();
        }
    }
}
