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

namespace AppCar.MinhDA.GUI
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    /// 
    public enum NotificationButton { OK, YesNo }
    public enum NotificationResult { OK, Yes, No }

    public enum NotificationType
    {
        Information, // Mặc định: Teal
        Success,     // Thành công: Green
        Error,       // Lỗi nghiêm trọng: Đỏ
        Warning      // Cảnh báo: Vàng Cam
    }
    // ==================================================

    public partial class NotificationWindow : Window
    {
        // Thuộc tính để lưu kết quả người dùng chọn (cần thiết cho ShowYesNo)
        public NotificationResult Result { get; private set; } = NotificationResult.No;

        // Constructor ĐẦY ĐỦ tham số
        public NotificationWindow(string title, string message, NotificationType type, NotificationButton buttonConfig = NotificationButton.OK)
        {
            InitializeComponent();
            SetupNotification(title, message, type, buttonConfig);
        }

        private void SetupNotification(string title, string message, NotificationType type, NotificationButton buttonConfig)
        {
            // 1. Gán nội dung
            TitleTextBlock.Text = title;
            MessageTextBlock.Text = message;

            // 2. Tùy chỉnh màu sắc và Icon dựa trên loại thông báo
            Brush colorBrush;
            string iconText;

            switch (type)
            {
                case NotificationType.Success:
                    // Màu xanh lá cây cho thành công
                    colorBrush = new SolidColorBrush(Color.FromRgb(76, 175, 80)); // #4CAF50
                    iconText = "✔";
                    break;
                case NotificationType.Error:
                    // Màu đỏ cho lỗi nghiêm trọng
                    colorBrush = new SolidColorBrush(Color.FromRgb(211, 47, 47)); // #D32F2F
                    iconText = "✖";
                    break;
                case NotificationType.Warning:
                    // Màu Vàng Cam cho Cảnh Báo
                    colorBrush = new SolidColorBrush(Color.FromRgb(255, 193, 7)); // #FFC107
                    iconText = "⚠";
                    break;
                case NotificationType.Information:
                default:
                    // Màu Teal từ App.xaml cho thông tin
                    colorBrush = (Brush)Application.Current.Resources["PrimaryBrush"];
                    iconText = "ℹ";
                    break;
            }

            // Áp dụng màu sắc cho viền và Icon
            NotificationContainer.BorderBrush = colorBrush;
            IconTextBlock.Foreground = colorBrush;
            IconTextBlock.Text = iconText;

            // 3. Hiển thị nút bấm phù hợp
            if (buttonConfig == NotificationButton.OK)
            {
                OkButton.Visibility = Visibility.Visible;
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
            }
            else // YesNo
            {
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Visibility = Visibility.Visible;
                NoButton.Visibility = Visibility.Visible;
            }
        }

        // Xử lý sự kiện click chung cho tất cả các nút
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Dựa vào tên của nút để xác định kết quả
            if (clickedButton.Name == "OkButton")
            {
                this.Result = NotificationResult.OK;
            }
            else if (clickedButton.Name == "YesButton")
            {
                this.Result = NotificationResult.Yes;
            }
            else if (clickedButton.Name == "NoButton")
            {
                this.Result = NotificationResult.No;
            }

            this.Close();
        }

        // =========================================================================
        // PHƯƠNG THỨC TĨNH HỖ TRỢ GỌI THÔNG BÁO
        // =========================================================================

        // Phương thức cho thông báo chỉ cần nút OK
        public static NotificationResult Show(string title, string message, NotificationType type = NotificationType.Information)
        {
            var dialog = new NotificationWindow(title, message, type, NotificationButton.OK);
            dialog.ShowDialog();
            return dialog.Result;
        }

        // Phương thức cho thông báo cần trả lời Có/Không
        public static NotificationResult ShowYesNo(string title, string message, NotificationType type = NotificationType.Information)
        {
            var dialog = new NotificationWindow(title, message, type, NotificationButton.YesNo);
            dialog.ShowDialog();
            return dialog.Result;
        }
    }
}
