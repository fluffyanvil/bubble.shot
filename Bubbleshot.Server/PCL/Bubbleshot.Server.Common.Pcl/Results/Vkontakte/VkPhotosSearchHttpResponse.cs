using Bubbleshot.Server.Common.Pcl.Base;
using Newtonsoft.Json;

namespace Bubbleshot.Server.Common.Pcl.Results.Vkontakte
{
	[JsonObject]
	public class VkPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("response")]
		public VkPhotosSearchResult Response { get; set; }
	}
}
