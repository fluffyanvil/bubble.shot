using Bubbleshot.Server.Common.Pcl.Base;
using Bubbleshot.Server.Common.Pcl.Results.Vkontakte;

namespace Bubbleshot.Server.Common.Pcl.Requests.Vkontakte
{
	public class VkPhotosSearchHttpRequest : BaseHttpRequest<VkPhotosSearchRequestParameters, VkPhotosSearchHttpResult, VkPhotosSearchResponse>
	{
		public VkPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
