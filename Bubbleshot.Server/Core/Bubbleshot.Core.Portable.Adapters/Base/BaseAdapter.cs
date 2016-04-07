using System;

namespace Bubbleshot.Core.Portable.Adapters.Base
{
	public abstract class BaseAdapter<TBaseAdapterConfig>
		where TBaseAdapterConfig : BaseAdapterConfig
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
