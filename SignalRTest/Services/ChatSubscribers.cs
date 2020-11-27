using Microsoft.AspNetCore.SignalR;
using SignalRTest.Hubs;
using SignalRTest.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Services
{
    /// <summary>
    /// This is a singleton for manage user precense
    /// </summary>
    public class ChatSubscribers : IRealTimeSubscriber
    {
        readonly List<Presence> _users = new List<Presence>();
        readonly Tracer _tracer;

        // injection
        readonly IHubContext<ChatHub> _hub;

        public ChatSubscribers(IHubContext<ChatHub> hub, Tracer tracer)
        {
            _hub = hub;
            _tracer = tracer;
            //
            _tracer.Start("ChatSubscribers");
        }

        public async Task SendMessage(string user, string message)
        {
            if (IsSubscribed(user)) {
                await _hub.Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            else {
                // just for example
                await _hub.Clients.All.SendAsync("ReceiveMessage", "Anonymus", "Unathorized");
            }
        }

        public async Task<bool> Subscribe(string user, string id)
        {
            if (IsSubscribed(user)) {
                return false;
            }
            _users.Add(new Presence { Name = user, Id = id });
            //
            _tracer.Log($"{user} is subscribed | id: {id}");
            //
            await _hub.Clients.All.SendAsync("ConnectedClients", ClientsCount());
            return true;
        }

        public async Task<bool> Unsubscribe(string user)
        {
            if (IsSubscribed(user)) {
                //
                _tracer.Log($"Unsubscribe {user}");
                //
                _users.Remove(_users.Find(x => x.Name == user));
                await _hub.Clients.All.SendAsync("ConnectedClients", ClientsCount());
                return true;
            }
            return false;
        }

        public async Task<bool> UnsubscribeUnatended(string id)
        {
            if (_users.Any(x => x.Id == id)) {
                // testing
                _tracer.Log($"UnsubscribeUnatended {id} {_users.Find(x => x.Id == id).Name}");
                //
                _users.Remove(_users.Find(x => x.Id == id));
                await _hub.Clients.All.SendAsync("ConnectedClients", ClientsCount());
                return true;
            }
            return false;
        }

        bool IsSubscribed(string user)
        {
            return _users.Any(x => x.Name == user);
        }

        int ClientsCount()
        {
            return _users.Count;
        }

        struct Presence
        {
            public string Name { get; set; }
            public string Id { get; set; }
        };

    }
}
