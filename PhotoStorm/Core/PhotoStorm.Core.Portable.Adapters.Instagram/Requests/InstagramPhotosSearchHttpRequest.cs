using PhotoStorm.Core.Portable.Adapters.Instagram.Results;
using PhotoStorm.Core.Portable.Common.Base;

namespace PhotoStorm.Core.Portable.Adapters.Instagram.Requests
{
	public class InstagramPhotosSearchHttpRequest : BaseHttpRequest<InstagramPhotosSearchRequestParameters, InstagramPhotosSearchHttpResponse>
	{
		public InstagramPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
