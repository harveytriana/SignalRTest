using Microsoft.AspNetCore.SignalR;
using SignalRTest.Services;
using SignalRTest.Shared;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class ChatHub : Hub
    {
        readonly ChatSubscribers _subscriber;
        readonly Tracer _tracer;

        public ChatHub(IRealTimeSubscriber subscriber, Tracer tracer)
        {
            _subscriber = subscriber as ChatSubscribers;
            _tracer = tracer;

            _tracer.Start("SignalRTest.ChatHub");
        }

        public async Task SendMessage(string user, string message)
        {
            if (_subscriber.IsSubscribed(user)) {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            } else {
                // just for example
                await Clients.All.SendAsync("ReceiveMessage", "Anonymus", "Unathorized");
            }
        }

        public async Task<bool> Subscribe(string user)
        {
            var result = _subscriber.Subscribe(user);
            if (result) {
                _tracer.Log($"Subscribe: {user}? {result}.");
                await Clients.All.SendAsync("ConnectedClients", _subscriber.ClientsCount());
            }
            else {
                _tracer.Log($"{user} was not subscribed.");
            }
            return result;
        }

        public async Task< bool> Unsubscribe(string user)
        {
            var result = _subscriber.Unsubscribe(user);
            if (result) {
                _tracer.Log($"Unsubscribe: {user}? {result}");
                await Clients.All.SendAsync("ConnectedClients", _subscriber.ClientsCount());
            }
            return result;
        }
    }
}
