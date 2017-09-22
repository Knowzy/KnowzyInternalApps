using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.Collections;
using Microsoft.Knowzy.Xamarin.Services;
using Microsoft.Knowzy.Xamarin.Model;
using Microsoft.Graph;

namespace Microsoft.Knowzy.Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            lvInventory.ItemsSource = MockInventoryService.Current.GetInventory();

            btnSignInSignOut.Text = "Sign In";
            slLoggedIn.IsVisible = false;
        }

        private async void InventoryList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lblActivityStatus.Text = await UserActivityService.Current.RecordInventoryActivityAndHistoryItemAsync(e.SelectedItem as InventoryModel);
        }

        private async void btnSignInSignOut_Clicked(object sender, EventArgs e)
        {
            if (btnSignInSignOut.Text == "Sign In")
            {
                AuthenticationService.Current.InitAuthenticatedClient();
                var currentUserObject = await App.GraphClient.Me.Request().GetAsync();
                lblUserName.Text = App.Username = currentUserObject.DisplayName;
                slLoggedIn.IsVisible = true;
                btnSignInSignOut.Text = "Sign Out";
            }
            else
            {
                AuthenticationService.Current.SignOut();
                slLoggedIn.IsVisible = false;
                lblUserName.Text = string.Empty;
                btnSignInSignOut.Text = "Sign In";
            }
        }
    }

    
}
