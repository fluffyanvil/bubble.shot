using Bubbleshot.Core.Portable.Common.Base;
using Bubbleshot.Core.Portable.Common.Results.Instagram;

namespace Bubbleshot.Core.Portable.Common.Requests.Instagram
{
	public class InstagramPhotosSearchHttpRequest : BaseHttpRequest<InstagramPhotosSearchRequestParameters, InstagramPhotosSearchHttpResponse>
	{
		public InstagramPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
