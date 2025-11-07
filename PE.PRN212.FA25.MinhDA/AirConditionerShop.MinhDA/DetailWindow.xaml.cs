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
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        //private AirConditioner _editedOne; => thay bằng property
        public AirConditioner EditedOne { get; set; }

        private AirConditionerService _airService = new();
        private SupplierCompanyService _supplierService = new();
        public DetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //đổ supplier cho combo box
            txtSupplierCombo.ItemsSource = _supplierService.GetAllSupplier();
            txtSupplierCombo.DisplayMemberPath = "SupplierName";
            txtSupplierCombo.SelectedValuePath = "SupplierId";
            // lưu ý: biến EditedOne là biến flag, đánh dấu trạng thái, mode của màn hình
            // nếu biến  này  = null, tạo mới, vì ko có selected gửi sang
            // nếu != null thì có gửi sang từ selected
            if (EditedOne != null)
            {
                DetailWindowMode.Content = "Sửa thông tin";
                txtID.Text = EditedOne.AirConditionerId.ToString();
                //khóa  key ID
                txtID.IsEnabled = false;
                txtName.Text = EditedOne.AirConditionerName;
                txtPrice.Text = EditedOne.DollarPrice.ToString();
                txtQuantity.Text = EditedOne.Quantity.ToString();
                txtWarranty.Text = EditedOne.Warranty;
                txtSoundLevel.Text = EditedOne.SoundPressureLevel;
                txtFeature.Text = EditedOne.FeatureFunction;
                txtSupplierCombo.SelectedValue = EditedOne.SupplierId;

            }
            else
            {
                DetailWindowMode.Content = "Tạo mới thông tin";

            }
        }

        
        private bool CheckVar()
        {
            // --- 1. Kiểm tra ID (Bắt buộc, Số nguyên, > 0) ---
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("ID sản phẩm là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(txtID.Text, out int airConId) || airConId <= 0)
            {
                MessageBox.Show("ID sản phẩm phải là một số nguyên dương (> 0).", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 2. Kiểm tra Tên (Bắt buộc, Độ dài 5-50) ---
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên sản phẩm là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int len = txtName.Text.Length;
            if (len < 5 || len > 50)
            {
                MessageBox.Show($"Tên sản phẩm phải có độ dài từ 5 đến 50 ký tự. Hiện tại là {len} ký tự.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 3. Kiểm tra Giá (Bắt buộc, Số thực, >= 0) ---
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Giá tiền là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!double.TryParse(txtPrice.Text, out double price) || price < 0)
            {
                MessageBox.Show("Giá tiền phải là một số (Double) không âm.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 4. Kiểm tra Số lượng (Bắt buộc, Số nguyên, >= 0) ---
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Số lượng là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên không âm.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 5. Kiểm tra Bảo hành (Bắt buộc) ---
            if (string.IsNullOrWhiteSpace(txtWarranty.Text))
            {
                MessageBox.Show("Thời gian bảo hành là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 6. Kiểm tra Độ ồn (Bắt buộc) ---
            if (string.IsNullOrWhiteSpace(txtSoundLevel.Text))
            {
                MessageBox.Show("Mức áp suất âm thanh (Độ ồn) là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 7. Kiểm tra Tính năng (Bắt buộc) ---
            if (string.IsNullOrWhiteSpace(txtFeature.Text))
            {
                MessageBox.Show("Tính năng/Chức năng là bắt buộc.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // --- 8. Kiểm tra Nhà cung cấp (Bắt buộc chọn) ---
            if (txtSupplierCombo.SelectedValue == null)
            {
                MessageBox.Show("Nhà cung cấp là bắt buộc. Vui lòng chọn một nhà cung cấp.", "Lỗi Kiểm Tra", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //check var trước khi save
            if (CheckVar() == false) 
            {
                return;
            }
            AirConditioner obj = new() { };
            obj.AirConditionerId = int.Parse(txtID.Text);
            obj.Quantity = int.Parse(txtQuantity.Text);
            obj.Warranty = txtWarranty.Text;
            obj.FeatureFunction = txtFeature.Text;
            obj.DollarPrice = double.Parse(txtPrice.Text);
            obj.SoundPressureLevel = txtSoundLevel.Text;
            obj.AirConditionerName = txtName.Text;
            obj.SupplierId = (string)txtSupplierCombo.SelectedValue;

            if (EditedOne == null)
            {
                _airService.CreateAirCon(obj);
            }
            else
            {
                _airService.UpdateAirCon(obj);
            }

            this.Close();
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure", "Quit?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
