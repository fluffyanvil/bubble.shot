using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Base
{
	public abstract class BaseHttpResult<TResponse> where TResponse : BaseResponse
	{
		[JsonProperty("response")]
		public TResponse Response { get; set; }
	}
}
