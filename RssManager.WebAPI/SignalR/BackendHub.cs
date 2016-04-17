using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RssManager.WebAPI.SignalR
{
    public class BackendHub : Hub
    {
        public readonly static ConnectionMapping<string> connections =
            new ConnectionMapping<string>();

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        //public void SendChatMessage(string who, string message)
        //{
        //    string name = Context.User.Identity.Name;

        //    foreach (var connectionId in _connections.GetConnections(who))
        //    {
        //        //this.backendHub.Clients.Group("Home").broadcastMessage("AUTOREFRESH");
        //        Clients.Client(connectionId).broadcastMessage(name + ": " + message);
        //    }
        //}

        public void Subscribe(string username)
        {
            if (string.IsNullOrEmpty(username))
                return;

            string id = Context.ConnectionId;
            connections.Add(username, Context.ConnectionId);
        }

        public void Unsubscribe(string username)
        {
            if (string.IsNullOrEmpty(username))
                return;

            string id = Context.ConnectionId;
            connections.Remove(username, Context.ConnectionId);
        }
    }
}