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
using System.Xml.Linq;

namespace ManagementMusicPlayer.SE1824
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public MusicPlayer EditedOne { get; set; }
        private MusicPlayerService _playerService = new();
        private CompanyService _companyService = new();
        private CategoryService _categoryService = new();

        public DetailWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //đổ supplier cho combo box
            cmbCompany.ItemsSource = _companyService.GetAllCompany();
            cmbCompany.DisplayMemberPath = "CompanyName";
            cmbCompany.SelectedValuePath = "CompanyId";

            cmbCategory.ItemsSource = _categoryService.GetAllCategory();
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.SelectedValuePath = "CategoryId";
            // lưu ý: biến EditedOne là biến flag, đánh dấu trạng thái, mode của màn hình
            // nếu biến  này  = null, tạo mới, vì ko có selected gửi sang
            // nếu != null thì có gửi sang từ selected
            if (EditedOne != null)
            {
                txtTitle.Content = "Cập nhật sản phẩm";
                txtId.Text = EditedOne.PlayerId.ToString();
                //khóa  key ID
                txtId.IsEnabled = false;
                txtPlayerName.Text = EditedOne.PlayerName;
                txtPrice.Text = EditedOne.Price.ToString();
                txtQuantity.Text = EditedOne.Quantity.ToString();
                cmbCategory.SelectedValue = EditedOne.CategoryId;
                dpPublishDate.Text = EditedOne.PublishDate.ToString();
                cmbCompany.SelectedValue = EditedOne.CompanyId;

            }
            else
            {
                txtTitle.Content = "Thêm sản phẩm mới";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Mọi thông tin thay đổi sẽ bị HỦY! \n" +
                "Bạn chắc chắn Hủy?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayer obj = new() { };

            if (EditedOne != null)
            {   //edit mode thì ta lấy id của ô nhập đập ngược trở lại object
                //để where trên id này
                obj.PlayerId = int.Parse(txtId.Text);
            }
            //tạo mới thì field id hok đc gán giá trị!!!!!!!!!!! để tự tăng

            obj.Quantity = int.Parse(txtQuantity.Text);
            obj.Price = decimal.Parse(txtPrice.Text);
            obj.PlayerName = txtPlayerName.Text;
            obj.CategoryId = (int)cmbCategory.SelectedValue;
            obj.PublishDate = (DateTime)dpPublishDate.SelectedDate;
            //gán vào cuốn lịch thì .Text, lấy ra thì .SelectedData

            //CÁI CUỐI KHOÁ NGOẠI lấy thịt heo
            obj.CompanyId = (int)cmbCompany.SelectedValue;

            if (EditedOne == null)
            {
                _playerService.CreatePlayer(obj);
            }
            else
            {
                _playerService.UpdatePlayer(obj);
            }
            this.Close();
        }
    }
}
