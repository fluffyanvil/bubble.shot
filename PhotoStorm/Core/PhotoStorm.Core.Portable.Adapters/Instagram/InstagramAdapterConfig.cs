using Bubbleshot.Core.Portable.Adapters.Base;

namespace Bubbleshot.Core.Portable.Adapters.Instagram
{
	public class InstagramAdapterConfig : BaseAdapterConfig
	{
		 public string AccessToken { get; set; }

		public string ApiAddress { get; set; }
	}
}