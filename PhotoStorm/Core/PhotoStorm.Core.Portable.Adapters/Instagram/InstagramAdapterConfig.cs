using PhotoStorm.Core.Portable.Adapters.Base;

namespace PhotoStorm.Core.Portable.Adapters.Instagram
{
	public class InstagramAdapterConfig : BaseAdapterConfig
	{
		 public string AccessToken { get; set; }

		public string ApiAddress { get; set; }
	}
}