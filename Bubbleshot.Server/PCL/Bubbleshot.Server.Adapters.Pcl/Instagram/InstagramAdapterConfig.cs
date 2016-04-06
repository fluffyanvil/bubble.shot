using Bubbleshot.Server.Adapters.Pcl.Base;

namespace Bubbleshot.Server.Adapters.Pcl.Instagram
{
	public class InstagramAdapterConfig : BaseAdapterConfig
	{
		 public string AccessToken { get; set; }

		public string ApiAddress { get; set; }
	}
}