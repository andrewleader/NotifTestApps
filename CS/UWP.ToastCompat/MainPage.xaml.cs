using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP.ToastCompat
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
                .AddInlineImage(new Uri(image))
                .AddAppLogoOverride(new Uri("https://unsplash.it/64?image=1005"), ToastGenericAppLogoCrop.Circle)
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
