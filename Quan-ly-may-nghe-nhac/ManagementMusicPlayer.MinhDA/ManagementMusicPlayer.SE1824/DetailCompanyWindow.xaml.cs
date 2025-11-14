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
    /// <summary>
    /// Interaction logic for DetailCompanyWindow.xaml
    /// </summary>
    public partial class DetailCompanyWindow : Window
    {
        private CompanyService _companyService = new();
        public Company? EditedOne { get; set; }
        public bool IsUpdated { get; set; } = false;

        public DetailCompanyWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditedOne != null)
            {
                // Chế độ CẬP NHẬT
                txtWindowTitle.Text = "CẬP NHẬT HÃNG SẢN XUẤT";
                txtCompanyName.Text = EditedOne.CompanyName;
                // Gán giá trị Status vào CheckBox
                chkStatus.IsChecked = EditedOne.Status;
            }
            else
            {
                // Chế độ THÊM MỚI
                txtWindowTitle.Text = "THÊM MỚI HÃNG SẢN XUẤT";
                // Mặc định là Đang sản xuất
                chkStatus.IsChecked = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên Hãng Sản Xuất.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            // 2. Chuẩn bị Entity
            Company companyToSave = EditedOne ?? new Company();

            companyToSave.CompanyName = txtCompanyName.Text;
            // Lấy giá trị từ CheckBox
            companyToSave.Status = chkStatus.IsChecked ?? false;

            // 3. Gọi Service
            try
            {
                if (EditedOne == null)
                {
                    // Giả định phương thức AddCompany có tồn tại
                    _companyService.CreateCompany(companyToSave);
                    MessageBox.Show("Thêm Hãng Sản Xuất thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Giả định phương thức UpdateCompany có tồn tại
                    _companyService.UpdateCompany(companyToSave);
                    MessageBox.Show("Cập nhật Hãng Sản Xuất thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                IsUpdated = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu dữ liệu Hãng Sản Xuất: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
