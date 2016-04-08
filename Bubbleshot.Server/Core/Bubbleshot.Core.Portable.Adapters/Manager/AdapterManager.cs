using System;
using System.Collections.Generic;
using System.Linq;
using Bubbleshot.Core.Portable.Adapters.EventArgs;
using Bubbleshot.Core.Portable.Adapters.Rules;

namespace Bubbleshot.Core.Portable.Adapters.Manager
{
	public class AdapterManager : IAdapterManager
	{
		private readonly List<IAdapter> _adapters;
		public AdapterManager()
		{
			_adapters = new List<IAdapter>();
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

		public bool CanStart => _adapters.Any(a => a.IsActive == false);
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
