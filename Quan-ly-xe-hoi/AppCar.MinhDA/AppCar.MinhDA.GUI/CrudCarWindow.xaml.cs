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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AppCar.MinhDA.GUI
{
    /// <summary>
    /// Interaction logic for CrudCarWindow.xaml
    /// </summary>
    public partial class CrudCarWindow : Window
    {
        public Car EditedOne { get; set; }
        private CarService _carService = new();
        private BrandService _brandService = new();
        private CarTypeService _typeService = new();

        public CrudCarWindow()
        {
            InitializeComponent();
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Gọi hàm CheckVar trước, nếu sai thì dừng ngay (return)
            if (CheckVar() == false) return;

            try
            {
                // Lúc này dữ liệu đã sạch sẽ, chỉ việc lấy dùng
                Car carObj = EditedOne ?? new Car();

                carObj.CarModel = txtCarModel.Text.Trim();
                carObj.CarEngine = txtEngine.Text.Trim();
                carObj.CarDescription = txtDescription.Text.Trim();
                carObj.BrandId = (int)cboBrand.SelectedValue;
                carObj.TypeId = (int)cboCarType.SelectedValue;

                // Chuyển đổi số an toàn (vì đã check ở CheckVar hoặc dùng NumberValidationTextBox)
                double.TryParse(txtPrice.Text, out double price);
                carObj.DollarPrice = price;

                int.TryParse(txtYear.Text, out int year);
                carObj.YearPublish = year;

                int.TryParse(txtWarranty.Text, out int warranty);
                carObj.CarWarranty = warranty;

                int.TryParse(txtSeat.Text, out int seat);
                carObj.CarSeat = seat;

                // Lưu xuống DB
                if (EditedOne == null)
                {
                    _carService.CreateCar(carObj);
                    NotificationWindow.Show("Thông báo", "Thêm mới thành công!", NotificationType.Success);
                }
                else
                {
                    _carService.UpdateCar(carObj);
                    NotificationWindow.Show("Thông báo", "Cập nhật thành công!", NotificationType.Success);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                NotificationWindow.Show("Cảnh báo", "Lỗi khi lưu: " + ex.Message, NotificationType.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = NotificationWindow.ShowYesNo("Xác nhận", "Mọi thông tin thay đổi sẽ không được lưu vào hệ thống!\n" +
                "Bạn có chắc chắn thoát?", NotificationType.Warning);
            if (result == NotificationResult.Yes)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboBrand.ItemsSource = _brandService.GetAllBrand();
            cboBrand.DisplayMemberPath = "BrandName";
            cboBrand.SelectedValuePath = "BrandId";

            cboCarType.ItemsSource = _typeService.GetAllType();
            cboCarType.DisplayMemberPath = "TypeModel";
            cboCarType.SelectedValuePath = "TypeId";

            if (EditedOne != null)
            {
                CrudMode.Text = "Cập Nhật Thông Tin Xe";
                txtCarId.Text = EditedOne.CarId.ToString();
                //khóa  key ID
                txtCarId.IsEnabled = false;
                txtCarModel.Text = EditedOne.CarModel;
                txtPrice.Text = EditedOne.DollarPrice.ToString();
                txtWarranty.Text = EditedOne.CarWarranty.ToString();
                txtDescription.Text = EditedOne.CarDescription;
                txtSeat.Text = EditedOne.CarSeat.ToString();
                txtEngine.Text = EditedOne.CarEngine;
                txtYear.Text = EditedOne.YearPublish.ToString();
                cboBrand.SelectedValue = EditedOne.BrandId;
                cboCarType.SelectedValue = EditedOne.TypeId;

            }
            else
            {
                CrudMode.Text = "Thêm Thông Tin Xe Mới";

            }
        }

        

        // --- HÀM KIỂM TRA DỮ LIỆU (CHECK VAR) ---
        private bool CheckVar()
        {
            // 1. Kiểm tra Mẫu xe
            if (string.IsNullOrWhiteSpace(txtCarModel.Text))
            {
                NotificationWindow.Show("Thiếu thông tin", "Vui lòng nhập tên Mẫu xe!", NotificationType.Warning);
                txtCarModel.Focus();
                return false;
            }

            // 2. Kiểm tra Hãng
            if (cboBrand.SelectedValue == null)
            {
                NotificationWindow.Show("Vui lòng chọn Thương hiệu!", "Thiếu thông tin", NotificationType.Warning);
                cboBrand.IsDropDownOpen = true; // Tự động xổ xuống cho user chọn
                return false;
            }

            // 3. Kiểm tra Loại
            if (cboCarType.SelectedValue == null)
            {
                NotificationWindow.Show("Vui lòng chọn Loại xe!", "Thiếu thông tin", NotificationType.Warning);
                cboCarType.IsDropDownOpen = true;
                return false;
            }

            // 4. Kiểm tra Giá tiền (Bắt buộc và phải là số)
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                NotificationWindow.Show("Vui lòng nhập Giá thành!", "Thiếu thông tin", NotificationType.Warning);
                txtPrice.Focus();
                return false;
            }

            // Kiểm tra xem giá có phải số thực không (đề phòng copy paste chữ vào)
            if (!double.TryParse(txtPrice.Text, out double checkPrice) || checkPrice < 0)
            {
                NotificationWindow.Show("Giá thành phải là số dương hợp lệ!", "Lỗi nhập liệu", NotificationType.Warning);
                txtPrice.SelectAll();
                txtPrice.Focus();
                return false;
            }

            // Nếu vượt qua tất cả cửa ải trên => Dữ liệu Hợp lệ
            return true;
        }

        
    }
}
