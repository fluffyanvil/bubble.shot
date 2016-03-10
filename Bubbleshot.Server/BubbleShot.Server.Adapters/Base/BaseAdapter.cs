using System;

namespace BubbleShot.Server.Adapters.Base
{
	public abstract class BaseAdapter<TBaseAdapterConfig> : IAdapter where TBaseAdapterConfig : BaseAdapterConfig
	{
		protected PollingManager PollingManager;

		public bool Active { get; set; }

		public event EventHandler<NewPhotoAlertEventArgs> NewPhotoAlertEventHandler;
		protected BaseAdapter(TBaseAdapterConfig c)
		{
			PollingManager = new PollingManager();
			Console.WriteLine("Adapter created");
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
