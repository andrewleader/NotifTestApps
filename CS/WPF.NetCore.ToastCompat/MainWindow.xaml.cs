using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF.NetCore.ToastCompat
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
            string title = "Andrew sent you a picture";
            string content = "Check this out, The Enchantments!";
            string image = "https://picsum.photos/364/202?image=883";
            int conversationId = 5;

            // Construct the toast content and show it!
            new ToastContentBuilder()
                .AddToastActivationInfo(new ToastArguments()
                    .Set("action", "viewConversation")
                    .Set("conversationId", conversationId)) // Arguments when the user taps body of toast
                .AddText(title)
                .AddText(content)
                .AddInlineImage(new Uri(await DownloadImageToDisk(image)))
                .AddAppLogoOverride(new Uri(await DownloadImageToDisk("https://unsplash.it/64?image=1005")), ToastGenericAppLogoCrop.Circle)
                .AddInputTextBox("tbReply", "Type a reply")

                .AddButton("Reply", ToastActivationType.Background, new ToastArguments()
                    .Set("action", "reply")
                    .Set("conversationId", conversationId))

                .AddButton("Like", ToastActivationType.Background, new ToastArguments()
                    .Set("action", "like")
                    .Set("conversationId", conversationId))

                .AddButton("View", ToastActivationType.Foreground, new ToastArguments()
                    .Set("action", "viewImage")
                    .Set("imageUrl", image))

                .Show();
        }

        private static bool _hasPerformedCleanup;
        private static async Task<string> DownloadImageToDisk(string httpImage)
        {
            // Toasts can live for up to 3 days, so we cache images for up to 3 days.
            // Note that this is a very simple cache that doesn't account for space usage, so
            // this could easily consume a lot of space within the span of 3 days.

            try
            {
                if (ToastNotificationManagerCompat.CanUseHttpImages)
                {
                    return httpImage;
                }

                var directory = Directory.CreateDirectory(System.IO.Path.GetTempPath() + "WindowsNotifications.DesktopToasts.Images");

                if (!_hasPerformedCleanup)
                {
                    // First time we run, we'll perform cleanup of old images
                    _hasPerformedCleanup = true;

                    foreach (var d in directory.EnumerateDirectories())
                    {
                        if (d.CreationTimeUtc.Date < DateTime.UtcNow.Date.AddDays(-3))
                        {
                            d.Delete(true);
                        }
                    }
                }

                var dayDirectory = directory.CreateSubdirectory(DateTime.UtcNow.Day.ToString());
                string imagePath = dayDirectory.FullName + "\\" + (uint)httpImage.GetHashCode();

                if (File.Exists(imagePath))
                {
                    return imagePath;
                }

                HttpClient c = new HttpClient();
                using (var stream = await c.GetStreamAsync(httpImage))
                {
                    using (var fileStream = File.OpenWrite(imagePath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                return imagePath;
            }
            catch { return ""; }
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
            ToastNotificationManagerCompat.History.Clear();
        }
    }
}
