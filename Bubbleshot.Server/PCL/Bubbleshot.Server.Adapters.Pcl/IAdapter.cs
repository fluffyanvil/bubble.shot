using Bubbleshot.Server.Adapters.Pcl.Rules;

namespace Bubbleshot.Server.Adapters.Pcl
{
	public interface IAdapter
	{
		void Start(IAdapterRule rule);
		void Stop();

		bool IsActive { get; }
	}
}
