using System;
using System.Collections.Generic;
using System.Linq;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Instagram;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Adapters.Vkontakte;

namespace PhotoStorm.Core.Portable.Adapters.Manager
{
	public class AdapterManager : IAdapterManager
	{
		private readonly List<IAdapter> _adapters;
		public AdapterManager()
		{
			_adapters = new List<IAdapter>();
            ConfigureAddapterManager();
		}

	    private void ConfigureAddapterManager()
	    {

            var vkAdapter = new VkAdapter(new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" });
            AddAdapter(vkAdapter);

            var instagramAdapter = new InstagramAdapter(new InstagramAdapterConfig { ApiAddress = "https://api.instagram.com/v1/media/search", ClientId = "1677ed07ddd54db0a70f14f9b1435579", AccessToken = "241559688.1677ed0.4b7b8ad7ea8249a39e94fde279cca059" });
            AddAdapter(instagramAdapter);
        }

	    public void AddAdapter(IAdapter adapter)
		{
			adapter.OnNewPhotosReceived += AdapterOnOnNewPhotosReceived;
			_adapters.Add(adapter);
		}

		public void RemoveAdapter(IAdapter adapter)
		{
			adapter.OnNewPhotosReceived -= AdapterOnOnNewPhotosReceived;
			_adapters.Remove(adapter);
		}


		private void AdapterOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
		{
            OnNewPhotosReceived?.Invoke(this, newPhotoAlertEventArgs);
        }

		public bool CanStart => _adapters.Any(a => !a.IsActive);
		public bool CanStop => _adapters.Any(a => a.IsActive);

		public void Start(IAdapterRule rule)
		{
			if (!_adapters.Any()) return;
			foreach (var adapter in _adapters)
			{
				if (!adapter.IsActive)
					adapter.Start(rule);
			}
		}

		public void Stop()
		{
			if (!_adapters.Any()) return;
			foreach (var adapter in _adapters)
			{
				if (adapter.IsActive)
					adapter.Stop();
			}
		}

		public event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
	}
}
