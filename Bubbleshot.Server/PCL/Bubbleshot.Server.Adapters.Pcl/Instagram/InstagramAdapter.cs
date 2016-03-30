using System;
using System.Linq;
using Bubbleshot.Server.Adapters.Pcl.Base;
using Bubbleshot.Server.Adapters.Pcl.Helpers;
using Bubbleshot.Server.Adapters.Pcl.Rules;
using Bubbleshot.Server.Common.Pcl.Extensions;
using Bubbleshot.Server.Common.Pcl.Requests.Instagram;

namespace Bubbleshot.Server.Adapters.Pcl.Instagram
{
	public class InstagramAdapter : BaseAdapter<InstagramAdapterConfig>, IAdapter
	{
		private InstagramPhotosSearchRequestParameters  _instagramPhotosSearchRequestParameters;
		private readonly InstagramPhotosSearchHttpRequest _instagramPhotosSearchHttpRequest;

		private string _accessToken;

		public InstagramAdapter(InstagramAdapterConfig c) : base(c)
		{
			_accessToken = c.AccessToken;
			_instagramPhotosSearchHttpRequest = new InstagramPhotosSearchHttpRequest(c.ApiAddress);
		}

		public void Start(IAdapterRule rule)
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), () =>
			{
				var startTime = DateTime.UtcNow.AddSeconds(-5);
				var endTime = DateTime.UtcNow;

				_instagramPhotosSearchRequestParameters = new InstagramPhotosSearchRequestParameters
				{
					StartTime = startTime,
					EndTime = endTime,
					Latitude = rule.Latitude,
					Longitude = rule.Longitude,
					Distance = rule.Radius,
					AccessToken = _accessToken
				};
				var result = _instagramPhotosSearchHttpRequest.Execute(_instagramPhotosSearchRequestParameters);
				if (result?.Result?.Images.Count > 0)
				{
					result.Result.Images = result.Result.Images.Where(
						i =>
							i.DateUnixStyle >= startTime.ToUnixStyle() &&
							i.DateUnixStyle <= endTime.ToUnixStyle()).ToList();
				}
				if (result?.Result?.Images.Count > 0)
				{
					var mapper = new InstagramPhotoItemMapper();
					var genericResult = mapper.MapVkPhotoItems(result.Result.Images).ToList();
					OnNewPhotoAlert(new NewPhotoAlertEventArgs { Count = result.Result.Images.Count, Photos = genericResult });
				}

			});
		}

		public void Stop()
		{
			Active = false;
			PollingManager.Stop();
		}

		public bool IsActive => Active;
	}
}
