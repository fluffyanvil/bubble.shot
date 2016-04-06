using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleShot.Server.Common.Extensions;
using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Results.Instagram
{
	public class InstagramPhotosSearchResultPhotoItem
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("images")]
		public InstagramSimpleImageArray Images { get; set; }

		[JsonProperty("location")]
		public InstagramLocationObject Location { get; set; }

		[JsonProperty("created_time")]
		public long DateUnixStyle { get; set; }

		[JsonIgnore]
		public DateTime Date => DateUnixStyle.ToDateTime();

		[JsonProperty("link")]
		public string Link { get; set; }
	}

	public class InstagramLocationObject
	{
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		[JsonProperty("latitude")]
		public double Latitude { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class InstagramSimpleImageObject
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonProperty("height")]
		public int Height { get; set; }
	}

	public class InstagramSimpleImageArray
	{
		[JsonProperty("low_resolution")]
		public InstagramSimpleImageObject LowResolution { get; set; }

		[JsonProperty("thumbnail")]
		public InstagramSimpleImageObject Thumbnail { get; set; }

		[JsonProperty("standard_resolution")]
		public InstagramSimpleImageObject StandardResolution { get; set; }
	}
}
