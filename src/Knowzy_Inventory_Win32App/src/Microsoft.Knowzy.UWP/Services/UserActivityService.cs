using Microsoft.Knowzy.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;

namespace Microsoft.Knowzy.UWP.Services
{
    public class UserActivityService
    {
        private const string WEB_ROOT = "https://github.com/Knowzy/KnowzyInternalApps/";
        private const string WEB_ASSETS_ROOT = "https://github.com/Knowzy/KnowzyInternalApps/tree/master/src/Knowzy_Engineering_Win32App/src/Microsoft.Knowzy.WPF/Assets/";
        private const string APP_ITEM_REDIRECT = "item?id=";

        private UserActivityService() { }

        private static UserActivityService current;

        public static UserActivityService Current => current ?? (current = new UserActivityService());

        public UserActivitySession CurrentRunningActivity { get; set; }

        public async Task RecordInventoryUserActivity(EditItemViewModel editItemViewModel)
        {
            UserActivityChannel channel = UserActivityChannel.GetDefault();
            string activityId = string.Concat(APP_ITEM_REDIRECT, editItemViewModel.Id);

            UserActivity userActivity = await channel.GetOrCreateUserActivityAsync(activityId);

            if (userActivity.State == UserActivityState.New)
            {
                userActivity.ActivationUri = new Uri($"knowzyinventory:{activityId}");

                userActivity.VisualElements.DisplayText = editItemViewModel.Name;
                userActivity.VisualElements.Description = editItemViewModel.Notes;
                //userActivity.VisualElements.Attribution.IconUri = new Uri(string.Concat(WEB_ASSETS_ROOT, editItemViewModel.ImageSource));

                await userActivity.SaveAsync();
            }

            CurrentRunningActivity?.Dispose();
            CurrentRunningActivity = userActivity.CreateSession();
        }

    }
}
