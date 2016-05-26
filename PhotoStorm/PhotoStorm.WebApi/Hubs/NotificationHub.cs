using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.Core.Portable.WorkManager;
using PhotoStorm.Core.Portable.Works.Works;

namespace PhotoStorm.WebApi.Hubs
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        private static IWorkManager Manager = WorkManager.Instance;

        public NotificationHub()
        {
            Manager.OnNewPhotosReceived += ManagerOnOnNewPhotosReceived;
        }

        private void ManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            try
            {
                var work = (IWork) sender;
                if (UserHandler.ConnectedIds.Contains(work.OwnerId.ToString()))
                    Clients.Client(work.OwnerId.ToString()).notify(JsonConvert.SerializeObject(newPhotoAlertEventArgs));
                else
                {
                    var toDelete = Manager.Works.FirstOrDefault(w => w.Id == work.Id);

                    if (toDelete != null)
                    {
                        Manager.StopWork(toDelete);
                        Manager.DeleteWork(toDelete);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void AddWork(CreateWorkModel model)
        {
            try
            {
                if (!model.IsValid)
                    return;
                var newWork = new Work(new Guid(Context.ConnectionId), model.Longitude, model.Latitude, model.Radius);
                Manager.AddWork(newWork);
                Clients.Client(Context.ConnectionId).workAdded(JsonConvert.SerializeObject(newWork));
            }
            catch (Exception)
            {
                
            }
            
        }

        public void StopWork(BaseWorkModel model)
        {
            try
            {
                var work = Manager.Works.FirstOrDefault(w => w.Id == model.Id);
                if (work != null)
                {
                    if (work != null)
                    {
                        Manager.StopWork(work);
                        Manager.DeleteWork(work);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Connected: {0}\n", Context.ConnectionId);
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Disconnected: {0}\n", Context.ConnectionId);
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("Reconnected: {0}\n", Context.ConnectionId);
            return (base.OnReconnected());
        }


    }
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
}
