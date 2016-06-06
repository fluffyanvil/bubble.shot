using System;
using System.Linq;
using PhotoStorm.Core.Portable.Adapters.Base;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Adapters.Vk.Requests;

namespace PhotoStorm.Core.Portable.Adapters.Vk
{
	public class VkAdapter : BaseAdapter<VkAdapterConfig>, IAdapter
	{
		private VkPhotosSearchRequestParameters _vkPhotosSearchRequestParameters;
		private readonly VkPhotosSearchHttpRequest _vkPhotosSearchHttpRequest;

		public VkAdapter(VkAdapterConfig c) : base(c)
		{
			_vkPhotosSearchHttpRequest = new VkPhotosSearchHttpRequest(c.ApiAddress);
            _mapper = new VkPhotoItemMapper();
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
				
				var genericResult = _mapper.MapVkPhotoItems(result.Response.Images).ToList();
                OnNewPhotosReceived?.Invoke(this, new NewPhotoAlertEventArgs { Count = result.Response.Images.Count, Photos = genericResult });
            });
		}

	    private readonly VkPhotoItemMapper _mapper;

        public void Stop()
		{
			Active = false;
			PollingManager.Stop();
		}

		public bool IsActive => Active;
		public event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
	}
}
