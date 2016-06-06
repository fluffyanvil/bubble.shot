using System;
using System.Collections.Generic;
using System.Globalization;
using PhotoStorm.Core.Portable.Common.Base;
using PhotoStorm.Core.Portable.Common.Extensions;

namespace PhotoStorm.Core.Portable.Adapters.Vk.Requests
{
	
	public class VkPhotosSearchRequestParameters : BaseRequestParameters
	{
		public string SearchQueryString { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTime StartTime { get; set; }
		public long StartTimeUnixStyle => StartTime.ToUnixStyle();
		public DateTime EndTime { get; set; }
		public long EndTimeUnixStyle => EndTime.ToUnixStyle();
		public short Sort { get; set; } // 1 - по лайкам, 0 - по дате добавления
		public int Offset { get; set; }
		public int Count { get; set; } // количество возвращаемых фотографий
		public int Radius { get; set; }

		public string ApiVersion => "5.42";

		public VkPhotosSearchRequestParameters()
		{
			StartTime = DateTime.UtcNow.AddHours(-1);
			EndTime = DateTime.UtcNow;
			Sort = 0;
			Count = 100;
			Radius = 5000;
			Latitude = 30;
			Longitude = 30;
		}

		public override Dictionary<string,string> Parameters
		{
			get
			{
				var result = new Dictionary<string, string>
				{
					{"q", SearchQueryString },
					{"lat", Latitude.ToString(CultureInfo.InvariantCulture)},
					{"long", Longitude.ToString(CultureInfo.InvariantCulture)},
					{"start_time", StartTimeUnixStyle.ToString()},
					{"end_time", EndTimeUnixStyle.ToString()},
					{"sort", Sort.ToString()},
					{"offset", Offset.ToString()},
					{"count", Count.ToString()},
					{"radius", Radius.ToString()},
					{"v", ApiVersion }
				};

				return result;
			}
		}
	}
}
