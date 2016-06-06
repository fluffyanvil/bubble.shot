using System.Collections.Generic;
using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Common.Base;

namespace PhotoStorm.Core.Portable.Adapters.Vk.Results
{
	public class VkPhotosSearchResult : BaseResult
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("items")]
		public List<VkPhotosSearchResponsePhotoItem> Images { get; set; }
	}
}
