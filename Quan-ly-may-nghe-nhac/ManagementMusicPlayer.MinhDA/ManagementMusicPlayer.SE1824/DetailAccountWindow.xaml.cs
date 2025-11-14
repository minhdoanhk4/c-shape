using ManagementMusicPlayer.BLL.Services;
using ManagementMusicPlayer.DAL.Entities;
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

namespace ManagementMusicPlayer.SE1824
{

    public class RoleItem
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
    /// <summary>
    /// Interaction logic for DetailAccountWindow.xaml
    /// </summary>
    public partial class DetailAccountWindow : Window
    {
        private AccountService _accountService = new();
        public Account? EditedOne { get; set; }
        public bool IsUpdated { get; set; } = false;

        // Định nghĩa các vai trò quản lý được phép và RoleId tương ứng
        private readonly List<RoleItem> AllowedRoles = new List<RoleItem>
        {
            new RoleItem { RoleId = 1, RoleName = "Admin" },
            new RoleItem { RoleId = 2, RoleName = "Manager" },
            new RoleItem { RoleId = 3, RoleName = "Staff" }
            // RoleId = 4 (Member) được loại trừ theo yêu cầu
        };

        public DetailAccountWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Load danh sách vai trò vào ComboBox
            cmbRole.ItemsSource = AllowedRoles;

            if (EditedOne != null)
            {
                // Chế độ CẬP NHẬT
                txtWindowTitle.Text = "CẬP NHẬT ACCOUNT";
                txtEmail.Text = EditedOne.Email;
                txtFullName.Text = EditedOne.FullName;

                txtPassword.Tag = "Nhập mật khẩu mới nếu muốn đổi...";
                txtPassword.Password = "";

                // 2. Kiểm tra quyền hạn UPDATE (Chỉ cho phép sửa Account Quản lý)
                var currentRole = AllowedRoles.FirstOrDefault(r => r.RoleId == EditedOne.RoleId);

                if (currentRole == null) // RoleId không nằm trong danh sách Quản lý (Giả định là Member)
                {
                    txtWindowTitle.Text = $"XEM CHI TIẾT ACCOUNT (Vai trò: {EditedOne.RoleId})";

                    // Vô hiệu hóa form và nút Save
                    txtFullName.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtPassword.IsEnabled = false;
                    cmbRole.IsEnabled = false;
                    btnSave.IsEnabled = false;

                    MessageBox.Show($"Không thể CẬP NHẬT thông tin Account có RoleId = {EditedOne.RoleId} (Vai trò không phải quản lý) qua cửa sổ này.", "Lỗi Quyền Hạn", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Cho phép cập nhật Account Quản lý
                    cmbRole.SelectedValue = EditedOne.RoleId;
                }
            }
            else
            {
                // Chế độ THÊM MỚI (Luôn cho phép thêm các Role Quản lý)
                txtWindowTitle.Text = "THÊM MỚI ACCOUNT QUẢN LÝ";
                txtPassword.Tag = "Mật khẩu bắt buộc...";
                cmbRole.SelectedIndex = 0; // Mặc định chọn Admin
            }
        }

        // [Các phương thức btnSave_Click và btnCancel_Click giữ nguyên]
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) || // Kiểm tra FullName
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email, Họ và Tên và chọn Vai trò.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            // Kiểm tra mật khẩu chỉ khi tạo mới HOẶC khi người dùng nhập mật khẩu mới
            if (EditedOne == null && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Mật khẩu không được để trống khi tạo mới.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            // 2. Chuẩn bị Entity
            Account obj = EditedOne ?? new Account();

            obj.Email = txtEmail.Text;
            obj.FullName = txtFullName.Text; // Lưu FullName

            // Lấy RoleId từ SelectedItem
            obj.RoleId = (cmbRole.SelectedItem as RoleItem)?.RoleId ?? 0;

            // Xử lý Password: chỉ cập nhật nếu có nhập
            if (!string.IsNullOrEmpty(txtPassword.Password))
            {
                // TODO: *LƯU Ý QUAN TRỌNG*: Đây là nơi bạn nên gọi hàm HashPassword trước khi gán.
                obj.Password = txtPassword.Password;
            }

            // 3. Gọi Service
            try
            {
                if (EditedOne == null)
                {
                    _accountService.CreateAccount(obj);
                    MessageBox.Show("Thêm Account Quản Lý thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Đảm bảo chỉ gọi Update nếu form không bị disable
                    if (btnSave.IsEnabled)
                    {
                        _accountService.UpdateAccount(obj);
                        MessageBox.Show("Cập nhật Account thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Trường hợp này không xảy ra nếu logic Load đã hoạt động, nhưng thêm để bảo vệ
                        MessageBox.Show("Không thể lưu. Account không phải quản lý.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                IsUpdated = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu dữ liệu Account: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Mọi thông tin thay đổi sẽ bị HỦY! \n" +
                "Bạn chắc chắn Hủy?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
