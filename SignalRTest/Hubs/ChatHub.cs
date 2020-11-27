using Microsoft.AspNetCore.SignalR;
using SignalRTest.Services;
using System;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class ChatHub : Hub
    {
        readonly ChatSubscribers _subscriber;

        public ChatHub(IRealTimeSubscriber subscriber)
        {
            _subscriber = subscriber as ChatSubscribers;
        }

        public async Task SendMessage(string user, string message)
        {
            await _subscriber.SendMessage(user, message);
        }

        public async Task<bool> Subscribe(string user)
        {
            return await _subscriber.Subscribe(user, Context.ConnectionId);
        }

        public async Task<bool> Unsubscribe(string user)
        {
            return await _subscriber.Unsubscribe(user);
        }

        //public override Task OnConnectedAsync()
        //{
        //    return base.OnConnectedAsync();
        //}

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _subscriber.UnsubscribeUnatended(Context.ConnectionId).Wait();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
