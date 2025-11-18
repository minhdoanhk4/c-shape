using System;
using System.Windows;
using System.Windows.Controls;

namespace CarManagement.GUI
{
    public partial class CustomMessageBox : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        // Constructor private
        private CustomMessageBox(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            InitializeComponent();

            // Gán text
            txtMessage.Text = message;
            txtTitle.Text = caption;

            // Xử lý hiển thị nút
            SetupButtons(button);

            // Gán sự kiện Click
            btnOk.Click += (s, e) => { Result = MessageBoxResult.OK; Close(); };
            btnCancel.Click += (s, e) => { Result = MessageBoxResult.Cancel; Close(); };
            btnYes.Click += (s, e) => { Result = MessageBoxResult.Yes; Close(); };
            btnNo.Click += (s, e) => { Result = MessageBoxResult.No; Close(); };
        }

        // --- LOGIC ẨN HIỆN NÚT ---
        private void SetupButtons(MessageBoxButton button)
        {
            // Mặc định ẩn hết (đã set trong XAML, nhưng set lại cho chắc)
            btnOk.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            btnYes.Visibility = Visibility.Collapsed;
            btnNo.Visibility = Visibility.Collapsed;

            switch (button)
            {
                case MessageBoxButton.OK:
                    btnOk.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.OKCancel:
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNo:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNoCancel:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

        // --- CÁC HÀM SHOW (STATIC) ---

        // 1. Hàm đầy đủ
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            var msgBox = new CustomMessageBox(message, caption, button, icon);
            msgBox.ShowDialog();
            return msgBox.Result;
        }

        // 2. Hàm rút gọn (3 tham số) - Tự động thêm icon Information
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button)
        {
            return Show(message, caption, button, MessageBoxImage.Information);
        }

        // 3. Hàm rút gọn (2 tham số) - Mặc định nút OK
        public static MessageBoxResult Show(string message, string caption)
        {
            return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 4. Hàm đơn giản nhất (1 tham số)
        public static MessageBoxResult Show(string message)
        {
            return Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}