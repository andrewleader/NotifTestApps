using Microsoft.Toolkit.Uwp.Notifications;
using System;
using ToastCompat;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToastCompat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // IMPORTANT: Look at App.xaml.cs for activation
        }

        private void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            _ = ToastHelpers.ShowImageReceivedToastAsync();
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

        private void ButtonScheduleToast_Click(object sender, RoutedEventArgs e)
        {
            ToastHelpers.ScheduleToastFor5Seconds();
        }

        private void ButtonProgressToast_Click(object sender, RoutedEventArgs e)
        {
            ToastHelpers.ShowProgressToast();
        }
    }
}
