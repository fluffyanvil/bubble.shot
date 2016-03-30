using Bubbleshot.Server.Adapters.Pcl.Rules;

namespace Bubbleshot.Server.Adapters.Pcl.Manager
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