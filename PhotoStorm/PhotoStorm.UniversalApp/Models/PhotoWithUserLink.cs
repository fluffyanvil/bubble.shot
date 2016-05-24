using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media;
using PhotoStorm.Core.Portable.Common.Enums;

namespace PhotoStorm.UniversalApp.Models
{
	public class PhotoWithUserLink
	{
		public ImageSource Image { get; set; }
		public string UserLink { get; set; }
		public string ImageDescription { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public string FormattedAddress { get; set; }

        public ChannelType Source { get; set; }

		public Geopoint PositionGeopoint => new Geopoint(new BasicGeoposition() {Longitude = Longitude, Latitude = Latitude});
	}
}
