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
        public DetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //đổ supplier cho combo box
            cmbCompany.ItemsSource = _companyService.GetAllCompany();
            cmbCompany.DisplayMemberPath = "CompanyName";
            cmbCompany.SelectedValuePath = "CompanyId";
            // lưu ý: biến EditedOne là biến flag, đánh dấu trạng thái, mode của màn hình
            // nếu biến  này  = null, tạo mới, vì ko có selected gửi sang
            // nếu != null thì có gửi sang từ selected
            if (EditedOne != null)
            {
                txtTitle.Content = "Sửa thông tin";
                txtId.Text = EditedOne.Id.ToString();
                //khóa  key ID
                txtId.IsEnabled = false;
                txtPlayerName.Text = EditedOne.PlayerName;
                txtPrice.Text = EditedOne.Price.ToString();
                txtQuantity.Text = EditedOne.Quantity.ToString();
                txtCategory.Text = EditedOne.Category;
                dpPublishDate.Text = EditedOne.PublishDate.ToString();
                cmbCompany.SelectedValue = EditedOne.CompanyId;

            }
            else
            {
                txtTitle.Content = "Tạo mới thông tin";

            }
        }
    }
}
