using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Microsoft.Knowzy.Xamarin
{
    public class App : Application
    {
        public static PublicClientApplication PCA = null;
        public static string ClientID = "1ada69dc-9e00-482f-9f29-769d89dd1e78";
        public static string[] Scopes = { "User.Read UserTimelineActivity.Write.CreatedByApp" };
        public static string Username = string.Empty;

        public static UIParent UiParent = null;

        public static GraphServiceClient GraphClient = null;

        public App()
        {
            // default redirectURI; each platform specific project will have to override it with its own
            PCA = new PublicClientApplication(ClientID);
                        
            MainPage = new NavigationPage(new Microsoft.Knowzy.Xamarin.MainPage());        
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
