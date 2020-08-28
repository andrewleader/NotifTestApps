﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace DesktopToastsApp
{
    // The GUID CLSID must be unique to your app. Create a new GUID if copying this code.
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("50cfb67f-bc8a-477d-938c-93cf6bfb3321"), ComVisible(true)]
    public class MyNotificationActivator : NotificationActivator
    {
        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (arguments.Length == 0)
                {
                    OpenWindowIfNeeded();
                    return;
                }

                // Parse the query string (using NuGet package QueryString.NET)
                QueryString args = QueryString.Parse(arguments);

                // See what action is being requested 
                switch (args["action"])
                {
                    // Open the image
                    case "viewImage":

                        // The URL retrieved from the toast args
                        string imageUrl = args["imageUrl"];

                        // Make sure we have a window open and in foreground
                        OpenWindowIfNeeded();

                        // And then show the image
                        (App.Current.Windows[0] as MainWindow).ShowImage(imageUrl);

                        break;

                    // Open the conversation
                    case "viewConversation":

                        // The conversation ID retrieved from the toast args
                        int conversationId = int.Parse(args["conversationId"]);

                        // Make sure we have a window open and in foreground
                        OpenWindowIfNeeded();

                        // And then show the conversation
                        (App.Current.Windows[0] as MainWindow).ShowConversation();

                        break;

                    // Background: Quick reply to the conversation
                    case "reply":

                        // Get the response the user typed
                        string msg = userInput["tbReply"];

                        // And send this message
                        ShowToast("Message sent: " + msg);

                        // If there's no windows open, exit the app
                        if (App.Current.Windows.Count == 0)
                        {
                            Application.Current.Shutdown();
                        }

                        break;

                    // Background: Send a like
                    case "like":

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
            // Construct the visuals of the toast
            ToastContent toastContent = new ToastContentBuilder()
                .AddToastActivationInfo("action=ok", ToastActivationType.Foreground)
                .AddText(msg)
                .GetToastContent();

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());

            // And then show it
            DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);
        }
    }
}
