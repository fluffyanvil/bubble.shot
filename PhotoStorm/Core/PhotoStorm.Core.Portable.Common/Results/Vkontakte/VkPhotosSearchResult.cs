using System.Collections.Generic;
using Bubbleshot.Core.Portable.Common.Base;
using Newtonsoft.Json;

namespace Bubbleshot.Core.Portable.Common.Results.Vkontakte
{
	public class VkPhotosSearchResult : BaseResult
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("items")]
		public List<VkPhotosSearchResponsePhotoItem> Images { get; set; }
	}
}
