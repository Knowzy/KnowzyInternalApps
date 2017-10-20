using Microsoft.Knowzy.Domain;
using Microsoft.Knowzy.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Knowzy.UWP
{
    public sealed partial class EditItemView : ContentDialog, INotifyPropertyChanged
    {
        public EditItemView()
        {
            this.InitializeComponent();
            Loaded += EditItemView_Loaded;
        }

        private void EditItemView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private EditItemViewModel editItemViewModel;

        public EditItemViewModel EditItemViewModel
        {
            get { return editItemViewModel; }
            set
            {
                editItemViewModel = value;
                OnPropertyChanged("EditItemViewModel");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
