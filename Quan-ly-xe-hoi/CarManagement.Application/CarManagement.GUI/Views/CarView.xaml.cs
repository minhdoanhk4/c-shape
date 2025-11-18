using CarManagement.BLL.Services;
using CarManagement.DAL;
using CarManagement.DAL.Entities;
using CarManagement.DAL.Repositories;
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

namespace CarManagement.GUI.Views
{
    /// <summary>
    /// Interaction logic for CarView.xaml
    /// </summary>
    public partial class CarView : UserControl
    {
        private CarService _carService = new();
        public CarView()
        {
            InitializeComponent();
        }

        private void txtSearchCar_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleSearch();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dgCars.ItemsSource = _carService.GetAllCar();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void cboCarFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandleSearch();
        }

        private void btnCarAdd_Click(object sender, RoutedEventArgs e)
        {// Mở cửa sổ chi tiết để thêm (Giả sử bạn có DetailWindow)
            // Bạn cần tạo logic mở DetailWindow ở đây. Ví dụ:
            /*
            DetailWindow detail = new DetailWindow();
            if (detail.ShowDialog() == true) // Nếu người dùng bấm Save và đóng window
            {
                LoadData(); // Refresh lại bảng
            }
            */
            CustomMessageBox.Show("Chức năng Add đang chờ bạn tích hợp với DetailWindow!");
        }

        private void btnCarUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCars.SelectedItem is Car selectedCar)
            {
                // Mở cửa sổ chi tiết và truyền object selectedCar sang để sửa
                /*
                DetailWindow detail = new DetailWindow(selectedCar); // Cần overload constructor nhận Car
                if (detail.ShowDialog() == true)
                {
                    LoadData();
                }
                */
                CustomMessageBox.Show($"Bạn đang chọn sửa xe ID: {selectedCar.CarId}. Cần tích hợp DetailWindow!");
            }
            else
            {
                CustomMessageBox.Show("Please select a car to update!");
            }
        }

        private void btnCarDelete_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dòng đang chọn trên Grid
            if (dgCars.SelectedItem is Car selectedCar)
            {
                // SỬ DỤNG CUSTOM MESSAGE BOX
                var result = CustomMessageBox.Show(
                    $"Are you sure you want to delete {selectedCar.CarModel}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                // Kiểm tra kết quả trả về
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _carService.DeleteCar(selectedCar);
                        FillDataGrid(_carService.GetAllCar());

                        // Thông báo thành công
                        CustomMessageBox.Show("Deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBox.Show("Error deleting: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                CustomMessageBox.Show("Please select a car to delete!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // 1. Chức năng Tìm Kiếm (Kết hợp TextBox và ComboBox)
        private void HandleSearch()
        {
            // Lấy từ khóa
            string keyword = txtSearchCar.Text;

            // Lấy loại lọc (All, Brand, Model...)
            // Cần ép kiểu SelectedItem về ComboBoxItem để lấy chuỗi Content
            string filterType = "All";
            if (cboCarFilter.SelectedItem is ComboBoxItem item && item.Content != null)
            {
                filterType = item.Content.ToString();
            }

            // Gọi Service tìm kiếm
            var result = _carService.SearchCar(keyword, filterType);
            dgCars.ItemsSource = result;
        }

        //Refresh lại grid
        private void FillDataGrid(List<Car> data)
        {
            dgCars.ItemsSource = null;
            dgCars.ItemsSource = data;

        }
    }
}
