using System;
using System.Linq;
using Bubbleshot.Server.Adapters.Pcl.Base;
using Bubbleshot.Server.Adapters.Pcl.Helpers;
using Bubbleshot.Server.Common.Pcl.Requests.Vkontakte;

namespace Bubbleshot.Server.Adapters.Pcl.Vkontakte
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
			PollingManager.Start(TimeSpan.FromSeconds(5), async () =>
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
				var result = await _vkPhotosSearchHttpRequest.Execute(_vkPhotosSearchRequestParameters);
				if (result.Response.Images.Count > 0)
					OnNewPhotoAlert(new NewPhotoAlertEventArgs {Count = result.Response.Images.Count, Photos = result.Response.Images});
			});
		}

		public override void Start(double latitude, double longitude, int radius)
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), async () =>
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
				var result = await _vkPhotosSearchHttpRequest.Execute(_vkPhotosSearchRequestParameters);
				if (!(result.Response.Images.Count > 0)) return;
				var mapper = new VkPhotoItemMapper();
				var genericResult = mapper.MapVkPhotoItems(result.Response.Images).ToList();
				OnNewPhotoAlert(new NewPhotoAlertEventArgs { Count = result.Response.Images.Count, Photos = genericResult });
			});
		}

		public override void Stop()
		{
			Active = false;
			PollingManager.Stop();
		}
	}
}
