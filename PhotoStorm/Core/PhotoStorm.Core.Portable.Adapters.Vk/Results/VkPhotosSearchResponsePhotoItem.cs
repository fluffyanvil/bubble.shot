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

		[JsonProperty("photo_75")]
		public string Photo75 { get; set; }

		[JsonProperty("photo_130")]
		public string Photo130 { get; set; }

		[JsonProperty("photo_604")]
		public string Photo604 { get; set; }

		[JsonProperty("photo_807")]
		public string Photo807 { get; set; }

		[JsonProperty("photo_1280")]
		public string Photo1280 { get; set; }

		[JsonProperty("photo_2560")]
		public string Photo2560 { get; set; }

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
	}
}
