using Microsoft.AspNetCore.SignalR;
using SignalRTest.Services;
using SignalRTest.Shared;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class ChatHub : Hub
    {
        readonly IRealTimeSubscriber _subscriber;
        readonly Tracer _tracer;

        public ChatHub(IRealTimeSubscriber subscriber, Tracer tracer)
        {
            _subscriber = subscriber;
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

        public bool Subscribe(string user)
        {
            var result = _subscriber.Subscribe(user);
            _tracer.Log($"Subscribe: {user}? {result}");

            return result;
        }

        public bool Unsubscribe(string user)
        {
            var result = _subscriber.Unsubscribe(user);
            _tracer.Log($"Unsubscribe: {user}? {result}");

            return result;
        }
    }
}
