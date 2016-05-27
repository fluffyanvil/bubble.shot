using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using PhotoStorm.Core.Portable.Common.Models;

namespace PhotoStorm.TestApp
{
    class Program
	{
        static Guid _id = new Guid("4b646e00-f3d7-49b7-88c0-5e19872ade29");
        static void Main(string[] args)
		{
            var hubConnection = new HubConnection("http://192.168.101.161:9000/signalr/hubs");
            var hubProxy = hubConnection.CreateHubProxy("notificationHub");
            hubProxy.On<string>("notify", (message) => Console.WriteLine("Recieved: {0}", message));
            hubProxy.On<string>("workAdded", (message) => Console.WriteLine("Recieved: {0}", message));
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

            CreateWorkModel model = new CreateWorkModel() {Latitude = 55.750341, Longitude = 37.62225, Radius = 10000};
            hubProxy.Invoke("AddWork", model).ContinueWith(task =>
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
