using System.Collections.Generic;
using BubbleShot.Server.Common.Base;
using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Results.Instagram
{
	[JsonObject]
	public class InstagramPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("data")]
		public List<InstagramPhotosSearchResultPhotoItem> Images { get; set; }
	}
}
