using Newtonsoft.Json;

namespace PhotoStorm.Core.Portable.Adapters.Vk.Results
{
	[JsonObject]
	public class VkPhotosSearchSize
	{
		[JsonProperty("url")]
		public string Source { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonProperty("height")]
		public int Height { get; set; }

		[JsonProperty("type")]
		public string InternalType { get; set; }

		[JsonIgnore]
		public string Type
		{
			get
			{
				return string.Empty;
			}
		}

		
	}
}