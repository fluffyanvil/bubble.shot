using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleShot.Server.Adapters.Base;
using BubbleShot.Server.Adapters.Helpers;
using BubbleShot.Server.Common.Requests.Instagram;

namespace BubbleShot.Server.Adapters.Instagram
{
	public class InstagramAdapter : BaseAdapter<InstagramAdapterConfig>
	{
		private InstagramPhotosSearchRequestParameters  _instagramPhotosSearchRequestParameters;
		private readonly InstagramPhotosSearchHttpRequest _instagramPhotosSearchHttpRequest;

		private string _accessToken;
		public InstagramAdapter(InstagramAdapterConfig c) : base(c)
		{
			_accessToken = c.AccessToken;
			_instagramPhotosSearchHttpRequest = new InstagramPhotosSearchHttpRequest(c.ApiAddress);
		}

		public override void Start()
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), () =>
			{
				_instagramPhotosSearchRequestParameters = new InstagramPhotosSearchRequestParameters()
				{
					StartTime = DateTime.UtcNow.AddSeconds(-5),
					EndTime = DateTime.UtcNow,
					Latitude = 57.876779,
					Longitude = 35.00511,
					Distance = 10000,
					AccessToken = _accessToken
				};
				var result = _instagramPhotosSearchHttpRequest.Execute(_instagramPhotosSearchRequestParameters);
				if (result?.Images.Count > 0)
				{
					result.Images = result.Images.Where(
						i =>
							i.Date >= _instagramPhotosSearchRequestParameters.StartTime &&
							i.Date <= _instagramPhotosSearchRequestParameters.EndTime).ToList();
				}

				if (result?.Images.Count > 0)
				{
					
					OnNewPhotoAlert(new NewPhotoAlertEventArgs { Count = result.Images.Count, Photos = result.Images });
				}
					
			});
		}

		public override void Start(double latitude, double longitude, int radius)
		{
			Active = true;
			PollingManager.Start(TimeSpan.FromSeconds(5), () =>
			{
				_instagramPhotosSearchRequestParameters = new InstagramPhotosSearchRequestParameters
				{
					StartTime = DateTime.UtcNow.AddSeconds(-5),
					EndTime = DateTime.UtcNow,
					Latitude = latitude,
					Longitude = longitude,
					Distance = radius,
					AccessToken = _accessToken
				};
				var result = _instagramPhotosSearchHttpRequest.Execute(_instagramPhotosSearchRequestParameters);
				if (result?.Images.Count > 0)
				{
					result.Images = result.Images.Where(
						i =>
							i.Date >= _instagramPhotosSearchRequestParameters.StartTime &&
							i.Date <= _instagramPhotosSearchRequestParameters.EndTime).ToList();
				}
				if (result?.Images.Count > 0)
				{
					var mapper = new InstagramPhotoItemMapper();
					var genericResult = mapper.MapVkPhotoItems(result.Images).ToList();
					OnNewPhotoAlert(new NewPhotoAlertEventArgs { Count = result.Images.Count, Photos = genericResult });
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
