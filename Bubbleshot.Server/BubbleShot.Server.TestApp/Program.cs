using System;
using BubbleShot.Server.Adapters.Instagram;
using BubbleShot.Server.Adapters.Vkontakte;

namespace BubbleShot.Server.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new InstagramAdapterConfig() { ApiAddress = "https://api.instagram.com/v1/media/search", AccessToken = "241559688.1677ed0.9d287accaaab4830885735d53ccc6018" };
			var adapter = new InstagramAdapter(config);
			adapter.NewPhotoAlertEventHandler += (sender, eventArgs) =>
			{
				Console.WriteLine("New photos: {0}", eventArgs.Count);
			};
			adapter.Start();
			Console.ReadLine();
		}
	}
}
