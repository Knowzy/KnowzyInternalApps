using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Microsoft.Knowzy.UWP.Helpers
{
    public class ImageSourceToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var imageSourceRedirect = $"ms-appx:///Images/{value.ToString().Replace(".jpg", ".png")}";

            return imageSourceRedirect;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
