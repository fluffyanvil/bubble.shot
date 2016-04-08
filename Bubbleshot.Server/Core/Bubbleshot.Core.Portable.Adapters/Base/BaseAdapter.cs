using System;

namespace Bubbleshot.Core.Portable.Adapters.Base
{
	public abstract class BaseAdapter<TBaseAdapterConfig>
		where TBaseAdapterConfig : BaseAdapterConfig
	{
		public TBaseAdapterConfig C { get; set; }
		protected PollingManager PollingManager;
		public bool Active { get; set; }
		protected BaseAdapter(TBaseAdapterConfig c)
		{
			C = c;
			PollingManager = new PollingManager();
		}
	}
}
