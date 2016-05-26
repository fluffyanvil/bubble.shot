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
        private readonly IWorkManager _manager = WorkManager.Instance;

        public NotificationHub()
        {
            _manager.OnNewPhotosReceived += ManagerOnOnNewPhotosReceived;
        }

        private void ManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            try
            {
                var work = (IWork) sender;
                if (UserHandler.ConnectedIds.Contains(work.OwnerId.ToString()))
                {
                    var json = JsonConvert.SerializeObject(newPhotoAlertEventArgs);
                    Clients.Client(work.OwnerId.ToString()).notify(json);
                }
                    
                else
                {
                    var toDelete = _manager.Works.FirstOrDefault(w => w.Id == work.Id);

                    if (toDelete != null)
                    {
                        _manager.StopWork(toDelete);
                        YellowCode("Estimating work deleted: {0}", JsonConvert.SerializeObject(toDelete));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        public void AddWork(CreateWorkModel model)
        {
            GreenCode("AddWork requested: {0}", JsonConvert.SerializeObject(model));
            try
            {
                if (!model.IsValid)
                    return;
                var newWork = new Work(new Guid(Context.ConnectionId), model.Longitude, model.Latitude, model.Radius);
                _manager.AddWork(newWork);
                Clients.Client(Context.ConnectionId).workAdded(JsonConvert.SerializeObject(newWork));
            }
            catch (Exception)
            {
                
            }
            
        }

        public void StopWork(Work work)
        {
            RedCode("StopWork requested: {0}", JsonConvert.SerializeObject(work));
            try
            {
                var toStop = _manager.Works.FirstOrDefault(w => w.Id == work.Id);
                if (toStop != null)
                {
                    _manager.StopWork(toStop);
                    if (!_manager.Works.Contains(toStop))
                    {
                        GreenCode("Work deleted: {0}", JsonConvert.SerializeObject(toStop));
                        Clients.Client(Context.ConnectionId).workDeleted();
                        return;
                    }
                    RedCode("Work not deleted: {0}", JsonConvert.SerializeObject(toStop));
                }
            }
            catch (Exception)
            {
                
            }
        }

        public override Task OnConnected()
        {
            GreenCode("Connected: {0}", Context.ConnectionId);
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            RedCode("Disconnected: {0}", Context.ConnectionId);
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            YellowCode("Reconnected: {0}", Context.ConnectionId);
            return (base.OnReconnected());
        }

        private void RedCode(string format, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, args);
        }

        private void GreenCode(string format, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(format, args);
        }

        private void YellowCode(string format, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, args);
        }

        private void BlueCode(string format, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(format, args);
        }
    }
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
}
