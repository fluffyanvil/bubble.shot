using System;
using System.Linq;
using Bubbleshot.Core.Portable.Adapters.Base;
using Bubbleshot.Core.Portable.Adapters.EventArgs;
using Bubbleshot.Core.Portable.Adapters.Helpers;
using Bubbleshot.Core.Portable.Adapters.Rules;
using Bubbleshot.Core.Portable.Common.Requests.Vkontakte;

namespace Bubbleshot.Core.Portable.Adapters.Vkontakte
{
	public class VkAdapter : BaseAdapter<VkAdapterConfig>, IAdapter
	{
		private VkPhotosSearchRequestParameters _vkPhotosSearchRequestParameters;
		private readonly VkPhotosSearchHttpRequest _vkPhotosSearchHttpRequest;

		public VkAdapter(VkAdapterConfig c) : base(c)
		{
			_vkPhotosSearchHttpRequest = new VkPhotosSearchHttpRequest(c.ApiAddress);
		}

		public void Start(IAdapterRule rule)
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), async () =>
			{
				_vkPhotosSearchRequestParameters = new VkPhotosSearchRequestParameters
				{
					Count = 100,
					StartTime = DateTime.UtcNow.AddSeconds(-5),
					EndTime = DateTime.UtcNow,
					Latitude = rule.Latitude,
					Longitude = rule.Longitude,
					Radius = rule.Radius
				};
				var result = await _vkPhotosSearchHttpRequest.Execute(_vkPhotosSearchRequestParameters);
				if (!(result.Response.Images.Count > 0)) return;
				var mapper = new VkPhotoItemMapper();
				var genericResult = mapper.MapVkPhotoItems(result.Response.Images).ToList();
				if (OnNewPhotosReceived != null)
					OnNewPhotosReceived(this, new NewPhotoAlertEventArgs { Count = result.Response.Images.Count, Photos = genericResult });
			});
		}

		public void Stop()
		{
			Active = false;
			PollingManager.Stop();
		}

		public bool IsActive => Active;
		public event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
	}
}
