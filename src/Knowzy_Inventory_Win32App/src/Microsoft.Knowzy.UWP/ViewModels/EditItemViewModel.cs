using Microsoft.Knowzy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Data.Core;

namespace Microsoft.Knowzy.UWP.ViewModels
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        [Display(Header = "Id")]
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string engineer;

        [Display(Header = "Engineer")]
        public string Engineer
        {
            get { return engineer; }
            set
            {
                engineer = value;
                OnPropertyChanged("Engineer");
            }
        }

        private string name;

        [Display(Header = "Name")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string rawMaterial;

        [Display(Header = "Raw Material")]
        public string RawMaterial
        {
            get { return rawMaterial; }
            set
            {
                rawMaterial = value;
                OnPropertyChanged("RawMaterial");
            }
        }

        private DevelopmentStatus developmentStatus;

        [Display(Header = "Development Status")]
        public DevelopmentStatus DevelopmentStatus
        {
            get { return developmentStatus; }
            set
            {
                developmentStatus = value;
                OnPropertyChanged("DevelopmentStatus");
            }
        }

        private DateTime developmentStartDate;

        [Display(Header = "Development Start Date")]
        public DateTime DevelopmentStartDate
        {
            get { return developmentStartDate; }
            set
            {
                developmentStartDate = value;
                OnPropertyChanged("DevelopmentStartDate");
            }
        }

        private DateTime expectedCompletionDate;

        [Display(Header = "Expected Completion Date")]
        public DateTime ExpectedCompletionDate
        {
            get { return expectedCompletionDate; }
            set
            {
                expectedCompletionDate = value;
                OnPropertyChanged("ExpectedCompletionDate");
            }
        }

        private string supplyManagemendContact;

        [Display(Header = "Supply Management Contact")]
        public string SupplyManagementContact
        {
            get { return supplyManagemendContact; }
            set
            {
                supplyManagemendContact = value;
                OnPropertyChanged("SupplyManagementContact");
            }
        }

        private string notes;

        [Display(Header = "Notes")]
        public string Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }

        private string imageSource;

        [Ignore]
        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
    }
}
