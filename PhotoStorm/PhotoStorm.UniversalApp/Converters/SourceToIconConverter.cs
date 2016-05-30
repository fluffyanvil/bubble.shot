using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using PhotoStorm.Core.Portable.Common.Enums;

namespace PhotoStorm.UniversalApp.Converters
{
    public class SourceToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var source = value as ChannelType? ?? ChannelType.Unknown;
            switch (source)
            {
                case ChannelType.Unknown:
                    return Application.Current.Resources["UnknownSourceIcon"] as DataTemplate;
                case ChannelType.Vkontakte:
                    return Application.Current.Resources["VkSourceIcon"] as DataTemplate;
                case ChannelType.Instagram:
                    return Application.Current.Resources["InstagramSourceIcon"] as DataTemplate;
                case ChannelType.Facebook:
                    return Application.Current.Resources["UnknownSourceIcon"] as DataTemplate;
                default:
                    return Application.Current.Resources["UnknownSourceIcon"] as DataTemplate;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
