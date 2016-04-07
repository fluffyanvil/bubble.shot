using Bubbleshot.Core.Portable.Common.Base;
using Newtonsoft.Json;

namespace Bubbleshot.Core.Portable.Common.Results.Vkontakte
{
	[JsonObject]
	public class VkPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("response")]
		public VkPhotosSearchResult Response { get; set; }
	}
}
