using System;
using BubbleShot.Server.Adapters.Vkontakte;

namespace BubbleShot.Server.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var c = new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" };
			var adapter = new VkAdapter(c);
			adapter.NewPhotoAlertEventHandler += (sender, eventArgs) =>
			{
				Console.WriteLine("New photos: {0}", eventArgs.Count);
			};
			adapter.Start();
			Console.ReadLine();
		}
	}
}
