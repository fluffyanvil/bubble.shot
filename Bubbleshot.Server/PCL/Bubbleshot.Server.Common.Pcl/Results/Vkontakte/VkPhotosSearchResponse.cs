using System.Collections.Generic;
using Bubbleshot.Server.Common.Pcl.Base;
using Newtonsoft.Json;

namespace Bubbleshot.Server.Common.Pcl.Results.Vkontakte
{
	public class VkPhotosSearchResponse : BaseResponse
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("items")]
		public List<VkPhotosSearchResponsePhotoItem> Items { get; set; }
	}
}
