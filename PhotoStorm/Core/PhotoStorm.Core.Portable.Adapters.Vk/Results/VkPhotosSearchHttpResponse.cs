using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Common.Base;

namespace PhotoStorm.Core.Portable.Adapters.Vk.Results
{
	[JsonObject]
	public class VkPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("response")]
		public VkPhotosSearchResult Response { get; set; }
	}
}
