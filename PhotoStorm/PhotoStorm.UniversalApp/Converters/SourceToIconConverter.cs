using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using PhotoStorm.Core.Portable.Common.Enums;
using ResourceManager = System.Resources.ResourceManager;

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
                    return Application.Current.Resources["UnknownGlyph"] as string;
                case ChannelType.Vkontakte:
                    return Application.Current.Resources["VkGlyph"] as string;
                case ChannelType.Instagram:
                    return Application.Current.Resources["InstagramGlyph"] as string;
                case ChannelType.Facebook:
                    return Application.Current.Resources["UnknownGlyph"] as string;
                default:
                    return Application.Current.Resources["UnknownGlyph"] as string;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
