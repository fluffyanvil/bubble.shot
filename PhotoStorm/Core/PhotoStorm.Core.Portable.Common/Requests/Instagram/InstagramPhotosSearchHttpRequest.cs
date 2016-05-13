using PhotoStorm.Core.Portable.Common.Base;
using PhotoStorm.Core.Portable.Common.Results.Instagram;

namespace PhotoStorm.Core.Portable.Common.Requests.Instagram
{
	public class InstagramPhotosSearchHttpRequest : BaseHttpRequest<InstagramPhotosSearchRequestParameters, InstagramPhotosSearchHttpResponse>
	{
		public InstagramPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
