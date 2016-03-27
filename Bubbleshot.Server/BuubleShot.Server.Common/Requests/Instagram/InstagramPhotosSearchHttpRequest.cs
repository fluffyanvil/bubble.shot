using BubbleShot.Server.Common.Base;
using BubbleShot.Server.Common.Results.Instagram;

namespace BubbleShot.Server.Common.Requests.Instagram
{
	public class InstagramPhotosSearchHttpRequest : BaseHttpRequest<InstagramPhotosSearchRequestParameters, InstagramPhotosSearchHttpResponse>
	{
		public InstagramPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
