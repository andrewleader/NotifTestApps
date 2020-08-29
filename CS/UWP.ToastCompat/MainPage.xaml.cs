using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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

            // Construct the toast content
            ToastContent toastContent = new ToastContentBuilder()
                .AddToastActivationInfo(new QueryString()
                {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString(), ToastActivationType.Foreground) // Arguments when the user taps body of toast
                .AddText(title)
                .AddText(content)
                .AddInlineImage(new Uri(image))
                .AddAppLogoOverride(new Uri("https://unsplash.it/64?image=1005"), ToastGenericAppLogoCrop.Circle)
                .AddInputTextBox("tbReply", "Type a reply")

                // Note that there's no reason to specify background activation, since our COM
                // activator decides whether to process in background or launch foreground window
                .AddButton(new ToastButton("Reply", new QueryString()
                {
                    { "action", "reply" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()))
                .AddButton(new ToastButton("Like", new QueryString()
                {
                    { "action", "like" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()))
                .AddButton(new ToastButton("View", new QueryString()
                {
                    { "action", "viewImage" },
                    { "imageUrl", image }

                }.ToString()))
                .GetToastContent();

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());

            // And then show it
            ToastNotificationManagerCompat.CreateToastNotifier().Show(toast);
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
