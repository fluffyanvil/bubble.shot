using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Popups;

namespace PhotoStorm.UniversalApp.Helpers
{
	public static class GeocodingHelper
	{
		public static async Task<string> GetAddressByCoordinates(double longitude, double latitude)
		{
			try
			{
				var location = new BasicGeoposition
				{
					Latitude = latitude,
					Longitude = longitude
				};
				var pointToReverseGeocode = new Geopoint(location);
				var result =
					  await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

				return result.Status == MapLocationFinderStatus.Success && result.Locations.Any() ? result.Locations[0].Address.FormattedAddress : string.Empty;
			}
			catch (Exception ex)
			{
				var message = new MessageDialog(ex.Message);
				await message.ShowAsync();
			}
			return string.Empty;
		}

		public static async Task<string> GetAddressByCoordinates(Geopoint geopoint)
		{
			try
			{
				var pointToReverseGeocode = geopoint;
				var result =
					  await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

				return result.Status == MapLocationFinderStatus.Success && result.Locations.Any() ? result.Locations[0].Address.FormattedAddress : string.Empty;
			}
			catch (Exception ex)
			{
				var message = new MessageDialog(ex.Message);
				await message.ShowAsync();
			}
			return string.Empty;
		}

		public static async Task<IEnumerable<MapLocation>> DirectGeocoding(string address, Geopoint mapCenterGeopoint)
		{
			if (string.IsNullOrEmpty(address))
			{
				return new List<MapLocation>();
			}

			var mapLocationFinderResult = await MapLocationFinder.FindLocationsAsync(address, mapCenterGeopoint, 10);
			return mapLocationFinderResult.Status == MapLocationFinderStatus.Success ? mapLocationFinderResult.Locations.ToList() : new List<MapLocation>();
		}
	}
}
