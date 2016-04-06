using System.Collections.Generic;
using Bubbleshot.Server.Common.Pcl.Base;
using Newtonsoft.Json;

namespace Bubbleshot.Server.Common.Pcl.Results.Instagram
{
	[JsonObject]
	public class InstagramPhotosSearchHttpResponse : BaseHttpResponse
	{
		[JsonProperty("data")]
		public List<InstagramPhotosSearchResultPhotoItem> Images { get; set; }
	}
}
