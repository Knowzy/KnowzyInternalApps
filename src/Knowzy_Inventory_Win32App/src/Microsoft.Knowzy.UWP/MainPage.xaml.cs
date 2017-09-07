using Microsoft.Knowzy.Domain.Enums;
using Microsoft.Knowzy.NET.BLL;
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
            var editItemView = new EditItemView
            {
                EditItemViewModel = new ViewModels.EditItemViewModel()
            };

            await editItemView.ShowAsync();
        }

        private async void dataGridInventory_SelectionChanged(object sender, Telerik.UI.Xaml.Controls.Grid.DataGridSelectionChangedEventArgs e)
        {
            var selectedInventory = dataGridInventory.SelectedItem as InventoryRow;

            if (selectedInventory != null)
            {
                var editItemView = new EditItemView
                {
                    EditItemViewModel = new ViewModels.EditItemViewModel
                    {
                        Id = selectedInventory.Id,
                        Engineer = selectedInventory.Engineer,
                        Name = selectedInventory.Name,
                        RawMaterial = selectedInventory.RawMaterial,
                        DevelopmentStatus = Enum.Parse<DevelopmentStatus>(selectedInventory.Status,true),
                        DevelopmentStartDate = selectedInventory.DevelopmentStartDate,
                        ExpectedCompletionDate = selectedInventory.ExpectedCompletionDate,
                        Notes = selectedInventory.Notes,
                        ImageSource = selectedInventory.ImageSource
                    }
                };

                await editItemView.ShowAsync();
            }
        }
    }
}
