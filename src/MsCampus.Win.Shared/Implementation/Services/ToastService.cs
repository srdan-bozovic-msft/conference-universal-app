using MsCampus.Win.Shared.Contracts.Services;
using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public class ToastService : IToastService
    {
        public void SendSimpleTextToast(string text)
        {
            IToastNotificationContent toastContent = null;

            IToastText01 templateContent = ToastContentFactory.CreateToastText01();
            templateContent.TextBodyWrap.Text = text;
            toastContent = templateContent;

            ToastNotification toast = toastContent.CreateNotification();

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public void SendImageAndTextToast(string text, string imageUrl)
        {
            IToastNotificationContent toastContent = null;


            IToastImageAndText01 templateContent = ToastContentFactory.CreateToastImageAndText01();
            templateContent.TextBodyWrap.Text = text;
            templateContent.Image.Src = imageUrl;
            toastContent = templateContent;

            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
