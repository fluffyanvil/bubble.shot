using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace PhotoStorm.TestApp
{
    class Program
	{
        static Guid _id = new Guid("4b646e00-f3d7-49b7-88c0-5e19872ade29");
        static void Main(string[] args)
		{
            var hubConnection = new HubConnection("http://localhost:9000/signalr/hubs");
            var hubProxy = hubConnection.CreateHubProxy("notificationHub");
            hubProxy.On<string>("notify", (message) => Console.Write("Recieved: {0}", message));
            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }
            }).Wait();
            hubProxy.Invoke("Join", hubConnection.ConnectionId, _id).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }

            }).Wait();
            while (true)
            {
                
            }
		}
    }
}
