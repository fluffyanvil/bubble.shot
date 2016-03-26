using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media;
using Prism.Windows.Mvvm;

namespace BubbleShot.UniversalApp.Models
{
	public class VkPhotoWithUserLink : ViewModelBase
	{
		public ImageSource Image { get; set; }
		public string UserLink { get; set; }
		public string ImageDescription { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public string FormattedAddress { get; set; }

		public Geopoint PositionGeopoint => new Geopoint(new BasicGeoposition() {Longitude = Longitude, Latitude = Latitude});
	}
}
