using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace ToastCompat
{
    public static class ToastHelpers
    {
        public static async Task ShowImageReceivedToastAsync()
        {
            string title = "Andrew sent you a picture";
            string content = "Check this out, The Enchantments!";
            string image = "https://picsum.photos/364/202?image=883";
            int conversationId = 5;

            // Construct the toast content and show it!
            new ToastContentBuilder()
                .AddArgument("action", MyToastActions.ViewConversation)
                .AddArgument("conversationId", conversationId)
                .AddText(title)
                .AddText(content)
                .AddInlineImage(new Uri(await DownloadImageToDisk(image)))
                .AddAppLogoOverride(new Uri(await DownloadImageToDisk("https://unsplash.it/64?image=1005")), ToastGenericAppLogoCrop.Circle)
                .AddInputTextBox("tbReply", "Type a reply")

                .AddButton(new ToastButton()
                    .SetContent("Reply")
                    .AddArgument("action", MyToastActions.Reply)
                    .SetBackgroundActivation())

                .AddButton(new ToastButton()
                    .SetContent("Like")
                    .AddArgument("action", MyToastActions.Like)
                    .SetBackgroundActivation())

                .AddButton(new ToastButton()
                    .SetContent("View")
                    .AddArgument("action", MyToastActions.ViewImage)
                    .AddArgument("imageUrl", image))

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

        public static void ClearToasts()
        {
            ToastNotificationManagerCompat.History.Clear();
        }

        public static void ScheduleToastFor5Seconds()
        {
            // Schedule a toast to appear in 5 seconds
            new ToastContentBuilder()

                // Arguments that are returned when the user clicks the toast or a button
                .AddArgument("action", MyToastActions.ViewConversation)
                .AddArgument("conversationId", 7764)

                .AddText("Scheduled toast notification")

                .Schedule(DateTime.Now.AddSeconds(5));
        }

        public static async void ShowProgressToast()
        {
            const string tag = "progressToast";

            new ToastContentBuilder()
                .AddArgument("action", MyToastActions.ViewConversation)
                .AddArgument("conversationId", 423)
                .AddText("Sending image to conversation...")
                .AddVisualChild(new AdaptiveProgressBar()
                {
                    Value = new BindableProgressBarValue("progress"),
                    Status = "Sending..."
                })
                .Show(toast =>
                {
                    toast.Tag = tag;

                    toast.Data = new NotificationData(new Dictionary<string, string>()
                    {
                        { "progress", "0" }
                    });
                });

            double progress = 0;

            while (progress < 1)
            {
                await Task.Delay(new Random().Next(1000, 3000));

                progress += (new Random().NextDouble() * 0.15) + 0.1;

                ToastNotificationManagerCompat.CreateToastNotifier().Update(
                    new NotificationData(new Dictionary<string, string>()
                    {
                        { "progress", progress.ToString() }
                    }), tag);
            }

            new ToastContentBuilder()
                .AddArgument("action", MyToastActions.ViewConversation)
                .AddArgument("conversationId", 423)
                .AddText("Sent image to conversation!")
                .Show(toast =>
                {
                    toast.Tag = tag;
                });
        }
    }
}
