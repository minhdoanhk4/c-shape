using AirConditionerShop.BLL.Services;
using AirConditionerShop.DAL.Models;
using AirConditionerShop.DAL.Repositories;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirConditionerShop.MinhDA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // khai báo biến để hứng role từ login
        public int? Role { get; set; }

        private AirConditionerService _airService = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //đổ vào grid, nhờ service lấy data
            AirconditionerDataGrid.ItemsSource = _airService.GetAllAirCons();

            if (Role == 2)
            {
                CreateButton.IsEnabled = false;
                UpdateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //1.  check xem đã click dòng chx

            AirConditioner selected = AirconditionerDataGrid.SelectedItem as AirConditioner;
            if (selected == null)
            {
                MessageBox.Show("Chọn 1 ô trước khi Delete", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            //2. Are u sure?

            MessageBoxResult answer = MessageBox.Show("Are you sure?", "Confirm",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No) 
            {
                return;
            }

            //3. nhờ Service xóa -> nhờ repo -> nhờ dbcontext
            _airService.DeleteAirCon(selected);

            //Refresh để thấy dòng đã mất
            FillDataGrid(_airService.GetAllAirCons());

        }

        private void FillDataGrid(List<AirConditioner> data)
        {
            AirconditionerDataGrid.ItemsSource = null;
            AirconditionerDataGrid.ItemsSource = data;

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //1.  check xem đã click dòng chx

            AirConditioner selected = AirconditionerDataGrid.SelectedItem as AirConditioner;
            if (selected == null)
            {
                MessageBox.Show("Chọn 1 ô trước khi Update", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            //2. gửi data dòng đã chọn qua detail window và đổ data vào 

            DetailWindow detail = new();
            //Gửi data tại đây
            detail.EditedOne = selected;

            detail.ShowDialog();

            //3. Chỉnh sửa data bên detail và đóng lại
            _airService.DeleteAirCon(selected);

            //Refresh để thấy dòng đã update
            
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow detail = new();
            
            detail.ShowDialog();
            FillDataGrid(_airService.GetAllAirCons());
        }
    }
}