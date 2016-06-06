using PhotoStorm.Core.Portable.Adapters.Vk.Results;
using PhotoStorm.Core.Portable.Common.Base;

namespace PhotoStorm.Core.Portable.Adapters.Vk.Requests
{
	public class VkPhotosSearchHttpRequest : BaseHttpRequest<VkPhotosSearchRequestParameters, VkPhotosSearchHttpResponse>
	{
		public VkPhotosSearchHttpRequest(string address, string method = "GET") : base(address, method)
		{
		}
	}
}
