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
    /// Interaction logic for DetailCategoryWindow.xaml
    /// </summary>
    public partial class DetailCategoryWindow : Window
    {
        private CategoryService _categoryService = new();
        public Category? EditedOne { get; set; }
        public bool IsUpdated { get; set; } = false;
        public DetailCategoryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditedOne != null)
            {
                // Chế độ CẬP NHẬT
                txtWindowTitle.Text = "CẬP NHẬT LOẠI MÁY";
                txtCategoryName.Text = EditedOne.CategoryName;
            }
            else
            {
                // Chế độ THÊM MỚI
                txtWindowTitle.Text = "THÊM MỚI LOẠI MÁY";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên Loại Máy.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            // 2. Chuẩn bị Entity
            Category obj = EditedOne ?? new Category();

            obj.CategoryName = txtCategoryName.Text;

            // 3. Gọi Service
            try
            {
                if (EditedOne == null)
                {
                    // Giả định phương thức AddCategory có tồn tại
                    _categoryService.CreateCategory(obj);
                    MessageBox.Show("Thêm Loại Máy thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Giả định phương thức UpdateCategory có tồn tại
                    _categoryService.UpdateCategory(obj);
                    MessageBox.Show("Cập nhật Loại Máy thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                IsUpdated = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu dữ liệu Loại Máy: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
