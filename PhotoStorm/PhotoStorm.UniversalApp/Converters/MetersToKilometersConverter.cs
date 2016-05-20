using System;
using Windows.UI.Xaml.Data;

namespace PhotoStorm.UniversalApp.Converters
{
    public class MetersToKilometersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var metersValue = System.Convert.ToDouble((int)value);
            return Math.Round(metersValue/1000, 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
