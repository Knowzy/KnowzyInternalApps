using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Knowzy.UWP.Helpers
{
    public class ImageSourceToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;

            var imageSourceRedirect = $"ms-appx:///Images/{value.ToString().Replace(".jpg", ".png")}";

            BitmapImage bmp = new BitmapImage(new Uri(imageSourceRedirect));

            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
