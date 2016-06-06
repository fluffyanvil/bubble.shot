using System.Collections.Generic;
using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Common.Base;

namespace PhotoStorm.Core.Portable.Adapters.Instagram.Results
{
	[JsonObject]
	public class InstagramPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("data")]
		public List<InstagramPhotosSearchResultPhotoItem> Images { get; set; }
	}
}
