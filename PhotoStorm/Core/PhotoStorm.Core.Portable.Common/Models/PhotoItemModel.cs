using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PhotoStorm.Core.Portable.Common.Enums;

namespace PhotoStorm.Core.Portable.Common.Models
{
    [JsonObject("photo")]
	public class PhotoItemModel
	{
        [JsonProperty("link")]
		public string ImageLink { get; set; }

        [JsonProperty("profile")]
		public string ProfileLink { get; set; }

        [JsonProperty("source")]
        [JsonConverter(typeof(StringEnumConverter))]
		public ChannelType Source { get; set; }

        [JsonProperty("time")]
		public DateTime TimeCreated { get; set; }

        [JsonProperty("lng")]
		public double Longitude { get; set; }

        [JsonProperty("ltd")]
		public double Latitude { get; set; }
	}
}
