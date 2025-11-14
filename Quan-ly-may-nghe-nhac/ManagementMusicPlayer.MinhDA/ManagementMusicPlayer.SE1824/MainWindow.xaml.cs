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
                FillAccountDataGrid(_accountService.GetAllAccount());
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
        private void BtnSearchAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Tìm kiếm Account đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadAccounts();
        }

        private void BtnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở cửa sổ Detail để TẠO Account...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DetailAccountWindow detail = new DetailAccountWindow(); detail.ShowDialog();
            LoadAccounts();
        }

        private void BtnUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (dgAccounts.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để UPDATE Account.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Mở cửa sổ Detail để CẬP NHẬT Account...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DetailAccountWindow detail = new DetailAccountWindow(); 
            detail.EditedOne = dgAccounts.SelectedItem as Account; detail.ShowDialog();
            LoadAccounts();
        }

        private void BtnRemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            Account? selected = dgAccounts.SelectedItem as Account;

            if (selected == null)
            {
                MessageBox.Show("Please select a row before REMOVE", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete Account '{selected.FullName}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _accountService.DeleteAccount(selected);
                MessageBox.Show("Account removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Account: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Cần cập nhật lại dữ liệu sau khi xóa
            LoadAccounts();
        }

        // COMPANY CRUD PLACEHOLDERS
        private void BtnSearchCompany_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Tìm kiếm Hãng Sản Xuất đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCompanies();
        }

        private void BtnCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở cửa sổ Detail để TẠO Hãng Sản Xuất...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DetailCompanyWindow detail = new DetailCompanyWindow(); 
            detail.ShowDialog();
            LoadCompanies();
        }

        private void BtnUpdateCompany_Click(object sender, RoutedEventArgs e)
        {
            if (dgCompanies.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để UPDATE Hãng Sản Xuất.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Mở cửa sổ Detail để CẬP NHẬT Hãng Sản Xuất...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DetailCompanyWindow detail = new DetailCompanyWindow();
            detail.EditedOne = dgCompanies.SelectedItem as Company; detail.ShowDialog();
            LoadCompanies();
        }

        private void BtnRemoveCompany_Click(object sender, RoutedEventArgs e)
        {
            Company? selected = dgCompanies.SelectedItem as Company;

            if (selected == null)
            {
                MessageBox.Show("Please select a row before REMOVE", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete Company '{selected.CompanyName}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _companyService.DeleteCompany(selected);
                MessageBox.Show("Company removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Company: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
            LoadCompanies();
        }

        // CATEGORY CRUD PLACEHOLDERS
        private void BtnSearchCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Tìm kiếm Loại Máy đang được phát triển...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCategories();
        }

        private void BtnCreateCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở cửa sổ Detail để TẠO Loại Máy...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DetailCategoryWindow detail = new DetailCategoryWindow();
            detail.ShowDialog();
            LoadCategories();
        }

        private void BtnUpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem == null) { MessageBox.Show("Vui lòng chọn một hàng để UPDATE Loại Máy.", "Chọn một", MessageBoxButton.OK, MessageBoxImage.Stop); return; }
            MessageBox.Show("Mở cửa sổ Detail để CẬP NHẬT Loại Máy...", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DetailCategoryWindow detail = new DetailCategoryWindow();
            detail.EditedOne = dgCategories.SelectedItem as Category; 
            detail.ShowDialog();
            LoadCategories();
        }

        private void BtnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            Category? selected = dgCategories.SelectedItem as Category;

            if (selected == null)
            {
                MessageBox.Show("Please select a row before REMOVE", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            MessageBoxResult answer = MessageBox.Show($"Are you sure you want to delete Category '{selected.CategoryName}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _categoryService.DeleteCategory(selected);
                MessageBox.Show("Category removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Category: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
            LoadCategories();
        }
    }
}
