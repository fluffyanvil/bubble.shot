using PhotoStorm.Core.Portable.Common.Base;
using PhotoStorm.Core.Portable.Common.Results.Vkontakte;

namespace PhotoStorm.Core.Portable.Common.Requests.Vkontakte
{
	public class VkPhotosSearchHttpRequest : BaseHttpRequest<VkPhotosSearchRequestParameters, VkPhotosSearchHttpResponse>
	{
		public VkPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
