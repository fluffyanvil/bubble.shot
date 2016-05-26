using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.WorkManager;
using PhotoStorm.Core.Portable.Works.Enums;
using PhotoStorm.Core.Portable.Works.Works;
using PhotoStorm.WebApi.Hubs;
using PhotoStorm.WebApi.Models;

namespace PhotoStorm.WebApi.Controllers
{
    public class WorksController : ApiController
    {
        private static readonly IWorkManager Manager = WorkManager.Instance;
        private static readonly IHubProxy _proxy;
        private static readonly HubConnection _hubConnection;

        private static readonly Dictionary<Guid, string> _connectionMapping = new Dictionary<Guid, string>();

        static WorksController()
        {
            Manager.OnNewPhotosReceived += ManagerOnOnNewPhotosReceived;
            _hubConnection = new HubConnection("http://localhost:9000/signalr/hubs");

            _proxy = _hubConnection.CreateHubProxy("notificationHub");

            _proxy.On<string, Guid>("joined", (s, guid) =>
            {
                _connectionMapping.Add(guid, s);
                Console.WriteLine("Mapping added");
            });

            _proxy.On<string,Guid>("disconnected", (s, guid) =>
            {
                _connectionMapping.Remove(guid);
                var work = Manager.Works.FirstOrDefault(w => w.OwnerId == guid);
                if (work != null)
                {
                    Manager.StopWork(work);
                    Manager.DeleteWork(work);
                }
                Console.WriteLine("Client disconnected");
            });

            _hubConnection.Start().ContinueWith(task =>
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
        }

        private static void ManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            try
            {
                var work = (IWork)sender;
                var to = _connectionMapping[work.OwnerId];
                if (to != null)
                    _proxy.Invoke("Notify", to, JsonConvert.SerializeObject(newPhotoAlertEventArgs));
                //foreach (var photoItemModel in newPhotoAlertEventArgs.Photos)
                //{
                //    Console.WriteLine($"WorkId: {work.Id}, ImageLink: {photoItemModel.ImageLink}");
                //}
            }
            catch (Exception)
            {
                
            }
            
        }

        public IWork Get(Guid id)
        {
            var work = Manager.Works.FirstOrDefault(w => w.Id == id);
            return work ?? Work.EmptyWork;
        }

        public IWork Post([FromBody]CreateWorkModel model)
        {
            if (!model.IsValid)
                return Work.EmptyWork;
            var newWork = new Work(model.OwnerId, model.Longitude, model.Latitude, model.Radius);
            Manager.AddWork(newWork);
            return newWork;
        }

        public IWork Delete([FromBody]BaseWorkModel model)
        {
            var toDelete = Manager.Works.FirstOrDefault(w => w.Id.Equals(model.Id));
            if (toDelete != null)
            {
                toDelete.State = WorkState.ToDelete;
                Manager.DeleteWork(toDelete);
                return toDelete;
            }
            return Work.EmptyWork;
        }
    }
}
