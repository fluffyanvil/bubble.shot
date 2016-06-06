using PhotoStorm.Core.Portable.Adapters.Base;

namespace PhotoStorm.Core.Portable.Adapters.Instagram
{
	public class InstagramAdapterConfig : BaseAdapterConfig
	{
		public string ApiAddress { get; set; }
        public string ClientId { get; set; }
        public string AccessToken { get; set; }
	}
}