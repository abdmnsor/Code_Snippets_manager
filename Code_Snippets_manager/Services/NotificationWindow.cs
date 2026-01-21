using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows;

namespace Code_Snippets_manager.Services
{
    public class NotificationWindow : Window
    {
        public NotificationWindow(string message)
        {
            // Window settings
            Width = 300;
            Height = 100;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background =  Brushes.Transparent;
            Topmost = true;
            ShowInTaskbar = false;
            ResizeMode = ResizeMode.NoResize;
            Opacity = 0.9;

            // Create UI elements
            Border border = new Border
            {
                Background =Brushes.DimGray,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10)
            };

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            border.Child = messageText;
            Content = border;

            // Timer to close after 3 seconds
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                Close();
            };
            timer.Start();

            // Position at bottom-right corner of the screen
            Loaded += (s, e) =>
            {
                var workArea = SystemParameters.WorkArea;
                Left = workArea.Right - Width - 10;
                Top = workArea.Bottom - Height - 10;
            };
        }
    }
}
