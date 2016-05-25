using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
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
        private static readonly NotificationHub Hub = NotificationHub.Instance; 

        public WorksController()
        {
            Manager.OnNewPhotosReceived += ManagerOnOnNewPhotosReceived;
            Hub.OnNewUserConnected += HubOnOnNewUserConnected;
        }

        private void HubOnOnNewUserConnected(object sender, NotificationHub.NewUserConnectedEventArgs newUserConnectedEventArgs)
        {
            Console.WriteLine(newUserConnectedEventArgs.ConnectionId);
        }

        private void ManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            var work = (IWork) sender;
            foreach (var photoItemModel in newPhotoAlertEventArgs.Photos)
            {
                Console.WriteLine($"WorkId: {work.Id}, ImageLink: {photoItemModel.ImageLink}");
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.All.Notify("stop the chat");
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
            var newWork = new Work(model.Longitude, model.Latitude, model.Radius) {WorkCreatorDevice = model.Device};
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
