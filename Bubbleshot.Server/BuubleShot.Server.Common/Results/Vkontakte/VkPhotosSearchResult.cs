using System.Collections.Generic;
using BubbleShot.Server.Common.Base;
using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Results.Vkontakte
{
	public class VkPhotosSearchResult : BaseResult
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("items")]
		public List<VkPhotosSearchResponsePhotoItem> Images { get; set; }
	}
}
