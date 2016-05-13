using System;
using System.Collections.Generic;
using System.Globalization;
using PhotoStorm.Core.Portable.Common.Base;
using PhotoStorm.Core.Portable.Common.Extensions;

namespace PhotoStorm.Core.Portable.Common.Requests.Instagram
{
	
	public class InstagramPhotosSearchRequestParameters : BaseRequestParameters
	{
		public string SearchQueryString { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTime StartTime { get; set; }
		public long StartTimeUnixStyle => StartTime.ToUnixStyle();
		public DateTime EndTime { get; set; }
		public long EndTimeUnixStyle => EndTime.ToUnixStyle();
		public int Distance { get; set; }
		public string AccessToken { get; set; }
		public string Type => "image";

		public InstagramPhotosSearchRequestParameters()
		{
			StartTime = DateTime.UtcNow.AddHours(-1);
			EndTime = DateTime.UtcNow;
			Distance = 5000;
			Latitude = 30;
			Longitude = 30;
		}

		public override Dictionary<string,string> Parameters
		{
			get
			{
				var result = new Dictionary<string, string>
				{
					{"lat", Latitude.ToString(CultureInfo.InvariantCulture)},
					{"lng", Longitude.ToString(CultureInfo.InvariantCulture)},
					{"MIN_TIMESTAMP", StartTimeUnixStyle.ToString()},
					{"MAX_TIMESTAMP", EndTimeUnixStyle.ToString()},
					{"distance", Distance.ToString()},
					{"access_token", AccessToken},
					{ "type", Type}
				};

				return result;
			}
		}
	}
}
