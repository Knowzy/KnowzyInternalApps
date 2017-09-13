using Microsoft.Knowzy.Domain.Enums;
using Microsoft.Knowzy.NET.BLL;
using Microsoft.Knowzy.UWP.Services;
using Microsoft.Knowzy.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Microsoft.Knowzy.NET.DAL.KnowzyDataSet;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Microsoft.Knowzy.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private EditItemViewModel _editItemViewModel;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridInventory.ItemsSource = InventoryBLL.Current.GetInventory();
            
        }

        private async void NewInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            _editItemViewModel = new ViewModels.EditItemViewModel
            {
                Id = "NewUserId",
                Name = "New Name",
                Notes = "New Notes"
            };

            var editItemView = new EditItemView
            {
                EditItemViewModel = _editItemViewModel
            };

            await UserActivityService.Current.RecordInventoryUserActivity(_editItemViewModel);

            await editItemView.ShowAsync();
        }

        private async void dataGridInventory_SelectionChanged(object sender, Telerik.UI.Xaml.Controls.Grid.DataGridSelectionChangedEventArgs e)
        {
            var selectedInventory = dataGridInventory.SelectedItem as InventoryRow;

            if (selectedInventory != null)
            {
                _editItemViewModel = new ViewModels.EditItemViewModel
                {
                    Id = selectedInventory.Id,
                    Engineer = selectedInventory.Engineer,
                    Name = selectedInventory.Name,
                    RawMaterial = selectedInventory.RawMaterial,
                    DevelopmentStatus = Enum.Parse<DevelopmentStatus>(selectedInventory.Status, true),
                    DevelopmentStartDate = selectedInventory.DevelopmentStartDate,
                    ExpectedCompletionDate = selectedInventory.ExpectedCompletionDate,
                    Notes = selectedInventory.Notes,
                    ImageSource = selectedInventory.ImageSource
                };

                var editItemView = new EditItemView
                {
                    EditItemViewModel = _editItemViewModel
                };

                await UserActivityService.Current.RecordInventoryUserActivity(_editItemViewModel);

                await editItemView.ShowAsync();
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            var parameters = e.Parameter.ToString().Split("id=");

            if (parameters.Length <= 1) return;

            bool isNewItem = false;

            InventoryRow row = null;

            if (!isNewItem)
            {
                row = InventoryBLL.Current.GetInventory().FindById(parameters[1]);
            }

            isNewItem = (row == null);

            _editItemViewModel = new EditItemViewModel();

            var editItemView = new EditItemView
            {
                EditItemViewModel = _editItemViewModel
            };

            await editItemView.ShowAsync();
        }
    }
}
