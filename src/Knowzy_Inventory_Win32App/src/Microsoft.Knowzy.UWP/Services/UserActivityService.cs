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
        private UserActivityService() { }

        private static UserActivityService current;

        public static UserActivityService Current => current ?? (current = new UserActivityService());

        public UserActivitySession CurrentRunningActivity { get; set; }

        public async Task RecordInventoryUserActivity(EditItemViewModel editItemViewModel)
        {
            UserActivityChannel channel = UserActivityChannel.GetDefault();
            string activityId = string.Concat("item?id=", editItemViewModel.Id);

            UserActivity userActivity = await channel.GetOrCreateUserActivityAsync(activityId);

            if (userActivity.State == UserActivityState.New)
            {
                userActivity.ActivationUri = new Uri($"knowzyinventory:{activityId}");

                userActivity.VisualElements.DisplayText = editItemViewModel.Name;
                userActivity.VisualElements.Description = editItemViewModel.Notes;
                await userActivity.SaveAsync();
            }

            CurrentRunningActivity?.Dispose();
            CurrentRunningActivity = userActivity.CreateSession();
        }

    }
}
