using System.Collections.Generic;
using System.Linq;
using Bubbleshot.Core.Portable.Adapters.Rules;

namespace Bubbleshot.Core.Portable.Adapters.Manager
{
	public class AdapterManager : IAdapterManager
	{
		private readonly List<IAdapter> _adapters;
		private long _chatId;
		public AdapterManager()
		{
			_adapters = new List<IAdapter>();
		}

		public AdapterManager(long chatId)
		{
			_chatId = chatId;
			_adapters = new List<IAdapter>();
		}
		public void AddAdapter(IAdapter adapter)
		{
			_adapters.Add(adapter);
		}

		public void RemoveAdapter(IAdapter adapter)
		{
			_adapters.Remove(adapter);
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
	}
}
