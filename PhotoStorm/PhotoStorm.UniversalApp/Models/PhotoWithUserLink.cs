using Windows.UI.Xaml.Media;
using PhotoStorm.Core.Portable.Common.Models;

namespace PhotoStorm.UniversalApp.Models
{
	public class PhotoWithUserLink
	{
		public ImageSource Image { get; set; }
		public PhotoItemModel Source { get; set; }
		public string FormattedAddress { get; set; }
	}
}
