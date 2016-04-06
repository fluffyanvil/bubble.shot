using Bubbleshot.Server.Common.Pcl.Base;
using Bubbleshot.Server.Common.Pcl.Results.Instagram;

namespace Bubbleshot.Server.Common.Pcl.Requests.Instagram
{
	public class InstagramPhotosSearchHttpRequest : BaseHttpRequest<InstagramPhotosSearchRequestParameters, InstagramPhotosSearchHttpResponse>
	{
		public InstagramPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
