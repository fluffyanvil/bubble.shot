using System;

namespace Bubbleshot.Server.Adapters.Pcl.Base
{
	public abstract class BaseAdapter<TBaseAdapterConfig> : IAdapter where TBaseAdapterConfig : BaseAdapterConfig
	{
		public TBaseAdapterConfig C { get; set; }
		protected PollingManager PollingManager;

		public bool Active { get; set; }

		public event EventHandler<NewPhotoAlertEventArgs> NewPhotoAlertEventHandler;
		protected BaseAdapter(TBaseAdapterConfig c)
		{
			C = c;
			PollingManager = new PollingManager();
		}

		public abstract void Start();
		public abstract void Start(double latitude, double longitude, int radius);
		public abstract void Stop();

		protected virtual void OnNewPhotoAlert(NewPhotoAlertEventArgs args)
		{
			NewPhotoAlertEventHandler?.Invoke(this, args);
		}
	}

	public class NewPhotoAlertEventArgs : System.EventArgs
	{
		public int Count { get; set; }

		public object Photos { get; set; }
	}
}
