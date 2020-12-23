using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ToastCompat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // IMPORTANT: Look at App.xaml.cs for activation
        }

        private async void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            ButtonPopToast.IsEnabled = false;
            await ToastHelpers.ShowImageReceivedToastAsync();
            ButtonPopToast.IsEnabled = true;
        }

        internal void ShowConversation()
        {
            ContentBody.Content = new TextBlock()
            {
                Text = "You've just opened a conversation!",
                FontWeight = FontWeights.Bold
            };
        }

        internal void ShowImage(string imageUrl)
        {
            ContentBody.Content = new Image()
            {
                Source = new BitmapImage(new Uri(imageUrl))
            };
        }

        private void ButtonClearToasts_Click(object sender, RoutedEventArgs e)
        {
            ToastHelpers.ClearToasts();
        }

        private async void ButtonScheduleToast_Click(object sender, RoutedEventArgs e)
        {
            ButtonScheduleToast.IsEnabled = false;

            // Schedule the toast to appear in 5 seconds
            ToastHelpers.ScheduleToastFor5Seconds();

            // Inform the user
            var tb = new TextBlock()
            {
                Text = "Toast scheduled to appear in 5 seconds",
                FontWeight = FontWeights.Bold
            };

            ContentBody.Content = tb;

            // And after 5 seconds, clear the informational message
            await Task.Delay(5000);

            ButtonScheduleToast.IsEnabled = true;

            if (ContentBody.Content == tb)
            {
                ContentBody.Content = null;
            }
        }

        private void ButtonProgressToast_Click(object sender, RoutedEventArgs e)
        {
            ToastHelpers.ShowProgressToast();
        }

        private void ButtonPopToastFromNetStandard14_Click(object sender, RoutedEventArgs e)
        {
            ToastHelpers.PopToastFromNetStandard14();
        }
    }
}
