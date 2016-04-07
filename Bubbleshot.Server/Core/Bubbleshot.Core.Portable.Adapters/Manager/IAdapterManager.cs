using Bubbleshot.Core.Portable.Adapters.Rules;

namespace Bubbleshot.Core.Portable.Adapters.Manager
{
	public interface IAdapterManager
	{
		void AddAdapter(IAdapter adapter);
		void RemoveAdapter(IAdapter adapter);
		bool CanStart { get; }
		bool CanStop { get; }
		void Start(IAdapterRule rule);
		void Stop();
	}
}