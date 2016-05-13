using System;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Rules;

namespace PhotoStorm.Core.Portable.Adapters
{
    public interface IAdapter
	{
		void Start(IAdapterRule rule);
		void Stop();
		bool IsActive { get; }

		event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
	}
}
