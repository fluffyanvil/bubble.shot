using System;
using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Common.Extensions;

namespace PhotoStorm.Core.Portable.Adapters.Vk.Results
{
	public class VkPhotosSearchResponsePhotoItem
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("album_id")]
		public string AlbumId { get; set; }

		[JsonProperty("owner_id")]
		public string OwnerId { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonProperty("heigth")]
		public int Height { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("date")]
		public long DateUnixStyle { get; set; }

		[JsonIgnore]
		public DateTime Date => DateUnixStyle.ToDateTime();

		[JsonProperty("lat")]
		public double Latitude { get; set; }

		[JsonProperty("long")]
		public double Longitude { get; set; }

		[JsonProperty("sizes")]
		public VkPhotosSearchSize[] Sizes { get; set; }
	}
}
