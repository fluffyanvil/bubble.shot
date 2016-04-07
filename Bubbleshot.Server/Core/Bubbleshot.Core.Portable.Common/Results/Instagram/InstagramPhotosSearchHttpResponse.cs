using System.Collections.Generic;
using Bubbleshot.Core.Portable.Common.Base;
using Newtonsoft.Json;

namespace Bubbleshot.Core.Portable.Common.Results.Instagram
{
	[JsonObject]
	public class InstagramPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("data")]
		public List<InstagramPhotosSearchResultPhotoItem> Images { get; set; }
	}
}
