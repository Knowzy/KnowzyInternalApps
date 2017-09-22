using Microsoft.Graph;
using Microsoft.Knowzy.Xamarin.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Knowzy.Xamarin.Model.Activity;

namespace Microsoft.Knowzy.Xamarin.Services
{
    public class UserActivityService
    {
        private const string WEB_ROOT = "https://github.com/Knowzy/KnowzyInternalApps/";
        private const string WEB_ASSETS_ROOT = "https://github.com/Knowzy/KnowzyInternalApps/tree/master/src/Knowzy_Engineering_Win32App/src/Microsoft.Knowzy.WPF/Assets/";
        private const string APP_ITEM_REDIRECT = "item?id=";

        private UserActivityService()
        {

        }

        private static UserActivityService current;

        public static UserActivityService Current => current ?? (current = new UserActivityService());

        public async Task<string> RecordInventoryUserActivityAsync(InventoryModel model)
        {
            var appActivityId = string.Concat(APP_ITEM_REDIRECT, model.InventoryId);

            Activity activity = new Activity
            {
                AppActivityId = appActivityId,
                ActivationUrl = $"knowzyinventory:{appActivityId}",
                VisualElements = new VisualInfo { DisplayText = model.Name },
                ActivitySourceHost = WEB_ROOT
            };

            string activitiesUrl = App.GraphClient.Me.AppendSegmentToRequestUrl("activities");
            string activitiesUrlWithId = string.Concat(activitiesUrl, "/", WebUtility.UrlEncode(model.InventoryId));

            var status = await CreateOrUpdateActivity(activity, activitiesUrlWithId);
            
            switch(status)
            {
                case HttpStatusCode.OK:
                    return $"Activity Updated for {model.Name}.";
                case HttpStatusCode.Created:
                    return $"Activity Created for {model.Name}.";
                default:
                    return $"Unknown or failed activity request for {model.Name}.";
            }
        }

        private async Task<HttpStatusCode> CreateOrUpdateActivity(Activity activity, string activitiesUrlWithId)
        {
            List<Activity> list = new List<Activity>();
            list.Add(activity);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, activitiesUrlWithId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            string activityJson = JsonConvert.SerializeObject(list, settings);
            var stringContent = new StringContent(activityJson, Encoding.UTF8, "text/json");

            request.Content = stringContent;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.Current.TokenForUser);

            var response = await App.GraphClient.HttpProvider.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return response.StatusCode;
        }
    }
}
