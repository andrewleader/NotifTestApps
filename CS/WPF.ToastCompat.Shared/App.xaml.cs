﻿using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows;

namespace ToastCompat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;

            // If launched from a toast
            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
            {
                // Our OnActivated callback will run after this completes,
                // and will show a window if necessary.
            }

            else
            {
                // Show the window
                // In App.xaml, be sure to remove the StartupUri so that a window doesn't
                // get created by default, since we're creating windows ourselves (and sometimes we
                // don't want to create a window if handling a background activation).
                new MainWindow().Show();
            }
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            Dispatcher.Invoke(delegate
            {
                if (e.Argument.Length == 0)
                {
                    OpenWindowIfNeeded();
                    return;
                }

                // Parse the arguments
                var args = ToastArguments.Parse(e.Argument);

                // See what action is being requested 
                if (args.TryGetValue("action", out MyToastActions action))
                {
                    switch (action)
                    {
                        // Open the image
                        case MyToastActions.ViewImage:

                            // The URL retrieved from the toast args
                            string imageUrl = args["imageUrl"];

                            // Make sure we have a window open and in foreground
                            OpenWindowIfNeeded();

                            // And then show the image
                            (App.Current.Windows[0] as MainWindow).ShowImage(imageUrl);

                            break;

                        // Open the conversation
                        case MyToastActions.ViewConversation:

                            // The conversation ID retrieved from the toast args
                            int conversationId = args.GetInt("conversationId");

                            // Make sure we have a window open and in foreground
                            OpenWindowIfNeeded();

                            // And then show the conversation
                            (App.Current.Windows[0] as MainWindow).ShowConversation();

                            break;

                        // Background: Quick reply to the conversation
                        case MyToastActions.Reply:

                            // Get the response the user typed
                            string msg = e.UserInput["tbReply"] as string;

                            // And send this message
                            ShowToast("Message sent: " + msg);

                            // If there's no windows open, exit the app
                            if (App.Current.Windows.Count == 0)
                            {
                                Application.Current.Shutdown();
                            }

                            break;

                        // Background: Send a like
                        case MyToastActions.Like:

                            ShowToast("Like sent!");

                            // If there's no windows open, exit the app
                            if (App.Current.Windows.Count == 0)
                            {
                                Application.Current.Shutdown();
                            }

                            break;

                        default:

                            OpenWindowIfNeeded();

                            break;
                    }
                }
                else
                {
                    // Open window if action wasn't found
                    OpenWindowIfNeeded();
                }
            });
        }

        private void OpenWindowIfNeeded()
        {
            // Make sure we have a window open (in case user clicked toast while app closed)
            if (App.Current.Windows.Count == 0)
            {
                new MainWindow().Show();
            }

            // Activate the window, bringing it to focus
            App.Current.Windows[0].Activate();

            // And make sure to maximize the window too, in case it was currently minimized
            App.Current.Windows[0].WindowState = WindowState.Normal;
        }

        private void ShowToast(string msg)
        {
            // Construct the visuals of the toast and show it!
            new ToastContentBuilder()
                .AddArgument("action", "ok")
                .AddText(msg)
                .Show();
        }
    }
}
