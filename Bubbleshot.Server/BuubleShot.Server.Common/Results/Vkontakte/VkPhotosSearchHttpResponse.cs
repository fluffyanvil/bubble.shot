using BubbleShot.Server.Common.Base;
using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Results.Vkontakte
{
	[JsonObject]
	public class VkPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("response")]
		public VkPhotosSearchResult Response { get; set; }
	}
}
