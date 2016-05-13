using System;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;

namespace PhotoStorm.TelegramBot
{
	public interface IWork
	{
		IAdapterManager AdapterManager { get; set; }
		long ChatId { get; set; }

		event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;

		void Start(IAdapterRule adapterRule);

		void Stop();
	}
}