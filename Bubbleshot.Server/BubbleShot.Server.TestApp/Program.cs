using Bubbleshot.Core.Portable.Adapters.Instagram;
using System;

namespace BubbleShot.Server.TestApp
{
    class Program
	{
		static void Main(string[] args)
		{
			var config = new InstagramAdapterConfig() { ApiAddress = "https://api.instagram.com/v1/media/search", AccessToken = "241559688.1677ed0.9d287accaaab4830885735d53ccc6018" };
			var adapter = new InstagramAdapter(config);
			adapter.OnNewPhotosReceived += (sender, eventArgs) =>
			{
				Console.WriteLine("New photos: {0}", eventArgs.Count);
			};
			
			Console.ReadLine();
		}
	}
}
