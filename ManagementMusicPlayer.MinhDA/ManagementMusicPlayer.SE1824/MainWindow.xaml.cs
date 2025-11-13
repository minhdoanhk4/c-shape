using ManagementMusicPlayer.BLL.Services;
using ManagementMusicPlayer.DAL.Entities;
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

namespace ManagementMusicPlayer.SE1824
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MusicPlayerService _playerService = new();
        private AccountService _accountService = new();
        private CompanyService _companyService = new();
        private CategoryService _categoryService = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            LoadDataForCurrentTab();

        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Đảm bảo chỉ phản ứng với sự kiện thay đổi của TabControl chính
            if (e.Source is TabControl)
            {
                // Khi chuyển tab, tải dữ liệu cho tab mới được chọn
                LoadDataForCurrentTab();
            }
        }

        private void LoadDataForCurrentTab()
        {
            if (MainTabControl.SelectedItem == TabMusicPlayer)
            {
                LoadMusicPlayers();
            }
            else if (MainTabControl.SelectedItem == TabAccount)
            {
                LoadAccounts();
            }
            else if (MainTabControl.SelectedItem == TabCompany)
            {
                LoadCompanies();
            }
            else if (MainTabControl.SelectedItem == TabCategory)
            {
                LoadCategories();
            }
        }

        // **********************************************
        // LOGIC TẢI DỮ LIỆU CỤ THỂ CHO TỪNG ENTITY
        // **********************************************

        private void LoadMusicPlayers()
        {
            try
            {
                // TODO: Áp dụng logic tìm kiếm/lọc nếu có dữ liệu trong các control tìm kiếm
                FillMusicPlayerGrid(_playerService.GetAllPlayer());
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI KHỞI TẠO DỮ LIỆU MUSIC PLAYER: " + ex.Message,
                                "Lỗi Hệ Thống Database/Entity Framework",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void LoadAccounts()
        {
            try
            {
                // TODO: Áp dụng logic tìm kiếm/lọc
                FillAccountDataGrid(_accountService.GetAllAccounts());
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI KHỞI TẠO DỮ LIỆU ACCOUNT: " + ex.Message,
                                "Lỗi Hệ Thống Database/Entity Framework",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void LoadCompanies()
        {
            try
            {
                // TODO: Áp dụng logic tìm kiếm/lọc
                FillCompanyDataGrid(_companyService.GetAllCompany());
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI KHỞI TẠO DỮ LIỆU COMPANY: " + ex.Message,
                                "Lỗi Hệ Thống Database/Entity Framework",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                // TODO: Áp dụng logic tìm kiếm/lọc
                FillCategoryDataGrid(_categoryService.GetAllCategory());
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI KHỞI TẠO DỮ LIỆU CATEGORY: " + ex.Message,
                                "Lỗi Hệ Thống Database/Entity Framework",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // **********************************************
        // PHẦN FILL DATAGRID CỤ THỂ CHO TỪNG ENTITY
        // **********************************************

        // Đổi tên để rõ ràng hơn (trước đây là FillDataGrid)
        private void FillMusicPlayerGrid(List<MusicPlayer> data)
        {
            dgMusicPlayers.ItemsSource = null;
            dgMusicPlayers.ItemsSource = data;
        }

        // Phương thức fill cho các DataGrid mới
        private void FillAccountDataGrid(List<Account> data)
        {
            dgAccounts.ItemsSource = null;
            dgAccounts.ItemsSource = data;
        }

        private void FillCompanyDataGrid(List<Company> data)
        {
            dgCompanies.ItemsSource = null;
            dgCompanies.ItemsSource = data;
        }

        private void FillCategoryDataGrid(List<Category> data)
        {
            dgCategories.ItemsSource = null;
            dgCategories.ItemsSource = data;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow detail = new();

            detail.ShowDialog();
            // Cần cập nhật lại dữ liệu sau khi DetailWindow đóng
            LoadMusicPlayers();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayer? selected = dgMusicPlayers.SelectedItem as MusicPlayer;

            if (selected == null)
            {
                MessageBox.Show("Please select a row before UPDATE", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            DetailWindow detail = new();
            detail.EditedOne = selected;
            detail.ShowDialog();
            LoadMusicPlayers();        
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayer? selected = dgMusicPlayers.SelectedItem as MusicPlayer;

            if (selected == null)
            {
                MessageBox.Show("Please select a row before REMOVE", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete Music Player '{selected.PlayerName}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _playerService.DeletePlayer(selected);
                MessageBox.Show("Music Player removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Music Player: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Cần cập nhật lại dữ liệu sau khi xóa
            LoadMusicPlayers();


        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement search logic for MusicPlayer
            MessageBox.Show("Chức năng Tìm kiếm Music Player đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // Sau khi tìm kiếm, gọi: LoadMusicPlayers();
        }

        // ACCOUNT CRUD PLACEHOLDERS
        private void btnSearchAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Tìm kiếm Account đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadAccounts();
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở cửa sổ Detail để TẠO Account...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // DetailAccountWindow detail = new DetailAccountWindow(); detail.ShowDialog();
            // LoadAccounts();
        }

        private void btnUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (dgAccounts.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để UPDATE Account.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Mở cửa sổ Detail để CẬP NHẬT Account...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // DetailAccountWindow detail = new DetailAccountWindow(); detail.EditedOne = dgAccounts.SelectedItem as Account; detail.ShowDialog();
            // LoadAccounts();
        }

        private void btnRemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (dgAccounts.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để REMOVE Account.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Thực hiện logic XÓA Account...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // Logic xóa và gọi LoadAccounts();
        }

        // COMPANY CRUD PLACEHOLDERS
        private void btnSearchCompany_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Tìm kiếm Hãng Sản Xuất đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCompanies();
        }

        private void btnCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở cửa sổ Detail để TẠO Hãng Sản Xuất...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // LoadCompanies();
        }

        private void btnUpdateCompany_Click(object sender, RoutedEventArgs e)
        {
            if (dgCompanies.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để UPDATE Hãng Sản Xuất.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Mở cửa sổ Detail để CẬP NHẬT Hãng Sản Xuất...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // LoadCompanies();
        }

        private void btnRemoveCompany_Click(object sender, RoutedEventArgs e)
        {
            if (dgCompanies.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để REMOVE Hãng Sản Xuất.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Thực hiện logic XÓA Hãng Sản Xuất...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // Logic xóa và gọi LoadCompanies();
        }

        // CATEGORY CRUD PLACEHOLDERS
        private void btnSearchCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Tìm kiếm Loại Máy đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCategories();
        }

        private void btnCreateCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở cửa sổ Detail để TẠO Loại Máy...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // LoadCategories();
        }

        private void btnUpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để UPDATE Loại Máy.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Mở cửa sổ Detail để CẬP NHẬT Loại Máy...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // LoadCategories();
        }

        private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để REMOVE Loại Máy.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Thực hiện logic XÓA Loại Máy...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            // Logic xóa và gọi LoadCategories();
        }
    }
}