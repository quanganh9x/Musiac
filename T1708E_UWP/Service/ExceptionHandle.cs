using NotificationsExtensions;
using NotificationsExtensions.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace T1708E_UWP.Service
{
    public static class ExceptionHandle
    {

        public static void ThrowDebug(string message)
        {
            try
            {
                ProgressBar.SetDebug(message);
            }
            catch
            {
                throw new Exception(message); // original type exception
            }
        }

        public static void ThrowToast(string message)
        {
            ToastContent content = new ToastContent()
            {
                Launch = "sign-in-error",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children = {
                        new AdaptiveText() {
                            Text = "Sign In Failed!"
                        },
                        new AdaptiveText() {
                            Text = message
                        },
                        new AdaptiveImage() {
                            Source = "https://unsplash.it/360/180?image=1043"
                        }
                        }
                    }
                }
            };
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }

        public static async void ThrowDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "T1708E-UWP Exception",
                Content = message,
                CloseButtonText = ":)"
            };

            await dialog.ShowAsync();
        }
    }
}
