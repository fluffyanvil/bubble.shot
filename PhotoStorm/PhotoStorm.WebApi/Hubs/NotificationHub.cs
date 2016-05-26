using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace PhotoStorm.WebApi.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<string, Guid> _connectionMapping = new Dictionary<string, Guid>();
        public void Join(string connectionId, Guid id)
        {
            Console.WriteLine($"Joined [{connectionId}:{id}]\n");
            _connectionMapping.Add(connectionId, id);
            Clients.All.joined(connectionId, id);
        }

        public void Notify(string connectionId, string message)
        {
            Clients.Client(connectionId).notify(message);
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Hub OnConnected {0}\n", Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Hub OnDisconnected {0}\n", Context.ConnectionId);
            Clients.All.disconnected(Context.ConnectionId, _connectionMapping[Context.ConnectionId]);
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("Hub OnReconnected {0}\n", Context.ConnectionId);

            return (base.OnReconnected());
        }
    }
}
