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
        private static GraphServiceClient _graphClient = null;

        private AuthenticationService()
        {

        }

        private static AuthenticationService current;

        public static AuthenticationService Current => current ?? (current = new AuthenticationService());

        public GraphServiceClient GetAuthenticatedClient()
        {
            if (_graphClient == null)
            {
                try
                {
                    _graphClient = new GraphServiceClient("https://graph.microsoft.com/v1.0", new DelegateAuthenticationProvider(
                            async (requestMessage) => {
                                var token = await GetTokenForUserAsync();
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                            }));
                    return _graphClient;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Could not create a graph client: " + ex.Message);
                }
            }

            return _graphClient;
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
            _graphClient = null;
            TokenForUser = null;
        }
    }
}
