using AppCar.MinhDA.BLL.Services;
using AppCar.MinhDA.DAL.Models;
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

namespace AppCar.MinhDA.GUI.Views
{
    /// <summary>
    /// Interaction logic for CarView.xaml
    /// </summary>
    public partial class CarView : UserControl
    {
        private CarService _service = new();
        public CarView()
        {
            InitializeComponent();
        }

        private void FillDataGrid(List<Car> data)
        {
            CarDataGrid.ItemsSource = null;
            CarDataGrid.ItemsSource = data;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Gọi service để lấy dữ liệu từ Database
                var list = _service.GetAllCar();

                // 2. Kiểm tra dữ liệu trả về
                if (list == null || list.Count == 0)
                {
                    // Trường hợp A: Kết nối được nhưng không có dòng nào
                    MessageBox.Show("Kết nối thành công nhưng không tìm thấy dữ liệu xe nào!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    // Vẫn gán list rỗng vào lưới để xóa dữ liệu cũ (nếu có)
                    CarDataGrid.ItemsSource = list;
                }
                else
                {
                    // Trường hợp B: Có dữ liệu -> Hiển thị lên lưới
                    CarDataGrid.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                // Trường hợp C: Lỗi kết nối Database hoặc lỗi Code (VD: Sai tên cột, sai mật khẩu DB)
                MessageBox.Show("LỖI KẾT NỐI DATABASE:\n" + ex.Message,
                                "Lỗi Nghiêm Trọng",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CrudCarWindow detail = new();

            detail.ShowDialog();
            FillDataGrid(_service.GetAllCar());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Car selected = CarDataGrid.SelectedItem as Car;
            if (selected == null)
            {
                NotificationWindow.Show("Cảnh  báo", "Chọn 1 ô trước khi Sửa!", NotificationType.Error);
                return;
            }

            CrudCarWindow detail = new();
            //Gửi data tại đây
            detail.EditedOne = selected;

            detail.ShowDialog();

            //3. Chỉnh sửa data bên detail và đóng lại
            _service.UpdateCar(selected);

            FillDataGrid(_service.GetAllCar());
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //1.  check xem đã click dòng chx

            Car? selected = CarDataGrid.SelectedItem as Car;
            if (selected == null)
            {
                NotificationWindow.Show("Cảnh  báo", "Chọn 1 ô trước khi Xóa!", NotificationType.Error);
                return;
            }

            //2. Are u sure?

            NotificationResult answer = NotificationWindow.ShowYesNo("Xác  nhận", "Bạn chắc chắn XÓA", NotificationType.Warning);
            if (answer == NotificationResult.No)
            {
                return;
            }

            //3. nhờ Service xóa -> nhờ repo -> nhờ dbcontext
            _service.DeleteCar(selected);

            //Refresh để thấy dòng đã mất
            FillDataGrid(_service.GetAllCar());
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid(_service.GetAllCar());
        }
    }
}
