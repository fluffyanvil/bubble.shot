using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using PhotoStorm.Console.Extensions;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.Core.Portable.WorkManager;
using PhotoStorm.Core.Portable.Works.Works;

namespace PhotoStorm.WebApi.Hubs
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        private readonly IWorkManager _workManager = new WorkManager();
        public NotificationHub()
        {
            _workManager.OnNewPhotosReceived -= ManagerOnOnNewPhotosReceived;
            _workManager.OnNewPhotosReceived += ManagerOnOnNewPhotosReceived;
        }

        private void ManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            try
            {
                var work = (IWork) sender;
                if (WorkManagerStaticFields.ConnectedIds.Contains(work.OwnerId.ToString()))
                {
                    XConsole.WriteLine("Received {0} new items for work {1}", ConsoleColor.Cyan, newPhotoAlertEventArgs.Count, JsonConvert.SerializeObject(work));
                    var json = JsonConvert.SerializeObject(newPhotoAlertEventArgs);
                    var ownerConnectionId = work.OwnerId.ToString();
                    Clients.Client(ownerConnectionId).notify(newPhotoAlertEventArgs);
                }
                else
                {
                    var toDelete = _workManager.Works.FirstOrDefault(w => w.Id == work.Id);
                    if (toDelete != null)
                    {
                        _workManager.StopWork(toDelete);
                        XConsole.WriteLine("Estimating work deleted: {0}", ConsoleColor.Yellow, JsonConvert.SerializeObject(toDelete));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        public void AddWork(CreateWorkModel model)
        {
           
            var newWork = new Work(new Guid(Context.ConnectionId), model.Longitude, model.Latitude, model.Radius);
            XConsole.WriteLine("AddWork requested: {0}", ConsoleColor.Green, JsonConvert.SerializeObject(newWork));
            try
            {
                if (!model.IsValid)
                    return;

                _workManager.AddWork(newWork);
                Clients.Client(Context.ConnectionId).workAdded(newWork);
            }
            catch (Exception ex)
            {
                XConsole.WriteLine(ex.Message, ConsoleColor.Red);
            }
            
        }

        public void StopWork(Work work)
        {
            XConsole.WriteLine("StopWork requested: {0}", ConsoleColor.Red, JsonConvert.SerializeObject(work));
            try
            {
                var toStop = _workManager.Works.FirstOrDefault(w => w.Id == work.Id);
                if (toStop != null)
                {
                    _workManager.StopWork(toStop);
                    if (!_workManager.Works.Contains(toStop))
                    {
                        XConsole.WriteLine("Work deleted: {0}", ConsoleColor.Green, JsonConvert.SerializeObject(toStop));
                        Clients.Client(Context.ConnectionId).workDeleted(JsonConvert.SerializeObject(work));
                        return;
                    }
                    XConsole.WriteLine("Work not deleted: {0}", ConsoleColor.Red, JsonConvert.SerializeObject(toStop));
                }
            }
            catch (Exception ex)
            {
                XConsole.WriteLine(ex.Message, ConsoleColor.Red);
            }
        }

        public void GetWorks()
        {
            Clients.Client(Context.ConnectionId).getWorks(_workManager.Works);
        }

        public void GetConnections()
        {
            Clients.Client(Context.ConnectionId).getConnections(WorkManagerStaticFields.ConnectedIds);
        }

        public override Task OnConnected()
        {
            XConsole.WriteLine("New client connected: {0}", ConsoleColor.Green, Context.ConnectionId);
            WorkManagerStaticFields.ConnectedIds.Add(Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            XConsole.WriteLine("Client disconnected: {0}", ConsoleColor.Red, Context.ConnectionId);
            WorkManagerStaticFields.ConnectedIds.Remove(Context.ConnectionId);
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            XConsole.WriteLine("Client reconnected: {0}", ConsoleColor.Yellow, Context.ConnectionId);
            return (base.OnReconnected());
        }
    }
   
}
