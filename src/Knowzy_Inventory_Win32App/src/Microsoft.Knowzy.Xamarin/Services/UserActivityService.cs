using Microsoft.Knowzy.Xamarin.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Knowzy.Xamarin.Services
{
    public class UserActivityService
    {
        private UserActivityService()
        {

        }

        private static UserActivityService current;

        public static UserActivityService Current => current ?? (current = new UserActivityService());

        public void RecordInventoryUserActivity(InventoryModel model)
        {
            
            Debug.WriteLine(model.Name);

        }
                
        //public async Task RecordInventoryUserActivity(EditItemViewModel editItemViewModel)
        //{
        //    UserActivityChannel channel = UserActivityChannel.GetDefault();
        //    string activityId = string.Concat(APP_ITEM_REDIRECT, editItemViewModel.Id);
    
        //    UserActivity userActivity = await channel.GetOrCreateUserActivityAsync(activityId);

        //    if (userActivity.State == UserActivityState.New)
        //    {
        //        userActivity.ActivationUri = new Uri($"knowzyinventory:{APP_ITEM_REDIRECT}{editItemViewModel.Id}");
        //        userActivity.VisualElements.DisplayText = editItemViewModel.Name;
        //        userActivity.VisualElements.Description = editItemViewModel.Notes;
        //        await userActivity.SaveAsync();
        //    }
        //}
    }
}
