using Bubbleshot.Core.Portable.Common.Base;
using Bubbleshot.Core.Portable.Common.Results.Vkontakte;

namespace Bubbleshot.Core.Portable.Common.Requests.Vkontakte
{
	public class VkPhotosSearchHttpRequest : BaseHttpRequest<VkPhotosSearchRequestParameters, VkPhotosSearchHttpResponse>
	{
		public VkPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
