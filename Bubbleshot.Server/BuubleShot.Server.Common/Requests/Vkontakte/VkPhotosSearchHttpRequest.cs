using BubbleShot.Server.Common.Base;
using BubbleShot.Server.Common.Results.Vkontakte;

namespace BubbleShot.Server.Common.Requests.Vkontakte
{
	public class VkPhotosSearchHttpRequest : BaseHttpRequest<VkPhotosSearchRequestParameters, VkPhotosSearchHttpResult, VkPhotosSearchResponse>
	{
		public VkPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
