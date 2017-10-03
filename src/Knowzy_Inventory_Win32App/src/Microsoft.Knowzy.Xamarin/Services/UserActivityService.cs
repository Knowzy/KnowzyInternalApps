using Microsoft.Graph;
using Microsoft.Knowzy.Xamarin;
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
        private UserActivityService()
        {

        }

        private static UserActivityService current;

        public static UserActivityService Current => current ?? (current = new UserActivityService());

        public async Task<string> RecordInventoryActivityAndHistoryItemAsync(InventoryModel model)
        {

            //return "Checkpoint created/updated";
            return string.Empty;
        }

        private async Task<string> CreateOrUpdateActivity(Activity activity, string activitiesUrlWithId)
        {
            var response = await CreateCustomGraphRequest(activity, activitiesUrlWithId);

            // we need to pull the activity Id off of the response headers for now - we should be getting it back in the response body
            var id = response?.Headers?.Location?.Segments?[4];

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Debug.WriteLine($"Activity Updated for {id}");
                    break;
                case HttpStatusCode.Created:
                    Debug.WriteLine($"Activity Created for {id}");
                    break;
            }

            return id;
        }

        private async Task<string> CreateOrUpdateHistoryItem(HistoryItem historyItem, string historyUrlWithId)
        {
            var response = await CreateCustomGraphRequest(historyItem, historyUrlWithId);

            // we need to pull the historyItem Id off of the response headers for now - we should be getting it back in the response body
            var id = response?.Headers?.Location?.Segments?[6];

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Debug.WriteLine($"HistoryItem Updated for {id}");
                    break;
                case HttpStatusCode.Created:
                    Debug.WriteLine($"HistoryItem Created for {id}");
                    break;
            }

            return id;
        }

        private async Task<HttpResponseMessage> CreateCustomGraphRequest<T>(T item, string customUrl)
        {
            List<T> containerList = new List<T> { item };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, customUrl);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            string itemJson = JsonConvert.SerializeObject(containerList, settings);
            var stringContent = new StringContent(itemJson, Encoding.UTF8, "text/json");

            request.Content = stringContent;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.Current.TokenForUser);

            HttpResponseMessage response = null;
            try
            {
                response = await App.GraphClient.HttpProvider.SendAsync(request);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }

            return response;
        }
    }
}
