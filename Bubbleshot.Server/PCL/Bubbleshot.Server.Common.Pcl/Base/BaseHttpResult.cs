using Newtonsoft.Json;

namespace Bubbleshot.Server.Common.Pcl.Base
{
	public abstract class BaseHttpResult<TResponse> where TResponse : BaseResponse
	{
		[JsonProperty("response")]
		public TResponse Response { get; set; }
	}
}
