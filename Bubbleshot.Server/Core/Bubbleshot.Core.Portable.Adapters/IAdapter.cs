using Bubbleshot.Core.Portable.Adapters.Rules;

namespace Bubbleshot.Core.Portable.Adapters
{
	public interface IAdapter
	{
		void Start(IAdapterRule rule);
		void Stop();
		bool IsActive { get; }
	}
}
