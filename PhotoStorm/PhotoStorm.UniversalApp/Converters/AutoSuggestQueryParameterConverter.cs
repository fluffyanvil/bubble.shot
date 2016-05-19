using System;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PhotoStorm.UniversalApp.Converters
{
    public class AutoSuggestQueryParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // cast value to whatever EventArgs class you are expecting here
            var args = (AutoSuggestBoxQuerySubmittedEventArgs)value;
            // return what you need from the args
            var mapLocation = args.ChosenSuggestion as MapLocation;
            return mapLocation?.Point;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
