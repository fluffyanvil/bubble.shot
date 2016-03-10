using System.Collections.Generic;
using BubbleShot.Server.Common.Base;
using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Results.Vkontakte
{
	public class VkPhotosSearchResponse : BaseResponse
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("items")]
		public List<VkPhotosSearchResponsePhotoItem> Items { get; set; }
	}
}
