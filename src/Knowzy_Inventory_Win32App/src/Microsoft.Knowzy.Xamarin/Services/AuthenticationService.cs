using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Knowzy.Xamarin.Services
{
    public class AuthenticationService
    {
        public string TokenForUser = null;
        private static DateTimeOffset expiration;       

        private AuthenticationService()
        {

        }

        private static AuthenticationService current;

        public static AuthenticationService Current => current ?? (current = new AuthenticationService());

        public void InitAuthenticatedClient()
        {
            if (App.GraphClient == null)
            {
                try
                {
                    App.GraphClient = new GraphServiceClient(Constants.GRAPH_BASE_URI, new DelegateAuthenticationProvider(
                            async (requestMessage) => {
                                var token = await GetTokenForUserAsync();
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            }));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Could not create a graph client: " + ex.Message);
                }
            }
        }

        public async Task<string> GetTokenForUserAsync()
        {
            if (TokenForUser == null || expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                AuthenticationResult authResult = await App.PCA.AcquireTokenAsync(App.Scopes, App.UiParent);
                TokenForUser = authResult.AccessToken;
                expiration = authResult.ExpiresOn;
            }

            return TokenForUser;
        }

        public void SignOut()
        {
            foreach (var user in App.PCA.Users)
            {
                App.PCA.Remove(user);
            }
            App.GraphClient = null;
            TokenForUser = null;
        }
    }
}
