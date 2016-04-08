using System;
using Bubbleshot.Core.Portable.Adapters.EventArgs;
using Bubbleshot.Core.Portable.Adapters.Manager;
using Bubbleshot.Core.Portable.Adapters.Rules;

namespace Bubbleshot.TelegramBot
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