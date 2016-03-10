using System;
using System.Linq;
using BubbleShot.Server.Adapters.Base;
using BubbleShot.Server.Adapters.Helpers;
using BubbleShot.Server.Common.Requests.Vkontakte;

namespace BubbleShot.Server.Adapters.Vkontakte
{
	public class VkAdapter : BaseAdapter<VkAdapterConfig>
	{
		private VkPhotosSearchRequestParameters _vkPhotosSearchRequestParameters;
		private readonly VkPhotosSearchHttpRequest _vkPhotosSearchHttpRequest;

		public VkAdapter(VkAdapterConfig c) : base(c)
		{
			_vkPhotosSearchHttpRequest = new VkPhotosSearchHttpRequest(c.ApiAddress);
		}

		public override void Start()
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), () =>
			{
				_vkPhotosSearchRequestParameters = new VkPhotosSearchRequestParameters
				{
					Count = 100,
					StartTime = DateTime.UtcNow.AddSeconds(-5),
					EndTime = DateTime.UtcNow,
					Latitude = 57.876779,
					Longitude = 35.00511,
					Radius = 10000
				};
				var result = _vkPhotosSearchHttpRequest.Execute(_vkPhotosSearchRequestParameters);
				if (result?.Items.Count > 0)
					OnNewPhotoAlert(new NewPhotoAlertEventArgs {Count = result.Items.Count, Photos = result.Items});
			});
		}

		public override void Start(double latitude, double longitude, int radius)
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), () =>
			{
				_vkPhotosSearchRequestParameters = new VkPhotosSearchRequestParameters
				{
					Count = 100,
					StartTime = DateTime.UtcNow.AddSeconds(-5),
					EndTime = DateTime.UtcNow,
					Latitude = latitude,
					Longitude = longitude,
					Radius = radius
				};
				var result = _vkPhotosSearchHttpRequest.Execute(_vkPhotosSearchRequestParameters);
				if (result?.Items.Count > 0)
				{
					var mapper = new VkPhotoItemMapper();
					var genericResult = mapper.MapVkPhotoItems(result.Items).ToList();
					OnNewPhotoAlert(new NewPhotoAlertEventArgs { Count = result.Items.Count, Photos = genericResult });
				}
					
			});
		}

		public override void Stop()
		{
			Active = false;
			PollingManager.Stop();
		}
	}
}
