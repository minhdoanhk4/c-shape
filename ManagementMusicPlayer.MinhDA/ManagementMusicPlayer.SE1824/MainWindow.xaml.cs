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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgMusicPlayers.ItemsSource = _playerService.GetAllPlayer();
        }

        private void FillDataGrid(List<MusicPlayer> data)
        {
            dgMusicPlayers.ItemsSource = null;
            dgMusicPlayers.ItemsSource = data;

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow detail = new();

            detail.ShowDialog();
            FillDataGrid(_playerService.GetAllPlayer());
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
            FillDataGrid(_playerService.GetAllPlayer());
        }
    }
}