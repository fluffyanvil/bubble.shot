using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace PhotoStorm.WebApi.Hubs
{
    public class NotificationHub : Hub
    {
        public void Notify(string connectionId, string message)
        {
            Clients.Client(connectionId).Notify(message);
        }

        public void Notify(string message)
        {
            Clients.All.Notify(message);
        }

        private NotificationHub()
        {
            
        }

        private static NotificationHub _instance;

        public static NotificationHub Instance => _instance ?? (_instance = new NotificationHub());

        public override Task OnConnected()
        {
            OnNewUserConnected?.Invoke(this, new NewUserConnectedEventArgs() {ConnectionId = Context.ConnectionId });
            return base.OnConnected();
        }

        public event EventHandler<NewUserConnectedEventArgs> OnNewUserConnected;

        public class NewUserConnectedEventArgs : EventArgs
        {
            public string ConnectionId { get; set; }
        }
    }
}
