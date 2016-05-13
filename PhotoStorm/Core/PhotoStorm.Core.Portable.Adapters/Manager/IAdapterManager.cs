using System;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Rules;

namespace PhotoStorm.Core.Portable.Adapters.Manager
{
	public interface IAdapterManager
	{
		void AddAdapter(IAdapter adapter);
		void RemoveAdapter(IAdapter adapter);
		bool CanStart { get; }
		bool CanStop { get; }
		void Start(IAdapterRule rule);
		void Stop();

		event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
	}
}