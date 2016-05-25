using System;
using Microsoft.AspNet.SignalR.Client;

namespace PhotoStorm.TestApp
{
    class Program
	{
        
        static void Main(string[] args)
		{
            var hubConnection = new HubConnection("http://localhost:9000/signalr/hubs");
            var hubProxy = hubConnection.CreateHubProxy("notificationHub");
            hubProxy.On<string>("notify", Console.WriteLine);
            hubConnection.Start().Wait();
            Console.ReadLine();
		}
	}
}
