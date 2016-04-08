using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bubbleshot.Core.Portable.Adapters.EventArgs;
using Bubbleshot.Core.Portable.Adapters.Manager;
using Bubbleshot.Core.Portable.Adapters.Rules;

namespace Bubbleshot.TelegramBot
{
	public class BotWork : IWork
	{
		public IAdapterManager AdapterManager { get; set; }
		public long ChatId { get; set; }

		public event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
		public void Start(IAdapterRule adapterRule)
		{
			if (AdapterManager.CanStart)
				AdapterManager.Start(adapterRule);
		}

		public void Stop()
		{
			if (AdapterManager.CanStop)
				AdapterManager.Stop();
		}

		public BotWork(long chatId, IAdapterManager adapterManager)
		{
			ChatId = chatId;
			AdapterManager = adapterManager;
			AdapterManager.OnNewPhotosReceived += AdapterManagerOnOnNewPhotosReceived;
		}

		private void AdapterManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
		{
			OnNewPhotosReceived?.Invoke(this, newPhotoAlertEventArgs);
		}
	}
}
