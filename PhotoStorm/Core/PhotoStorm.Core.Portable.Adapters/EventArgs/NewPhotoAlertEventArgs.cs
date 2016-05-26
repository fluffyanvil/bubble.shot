using System.Collections.Generic;
using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Common.Models;

namespace PhotoStorm.Core.Portable.Adapters.EventArgs
{
    [JsonObject]
	public class NewPhotoAlertEventArgs : System.EventArgs
	{
        [JsonProperty("count")]
		public int Count { get; set; }

        [JsonProperty("photos")]
		public List<PhotoItemModel> Photos { get; set; }
	}
}
