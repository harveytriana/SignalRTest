using Microsoft.AspNetCore.SignalR;
using SignalRTest.Shared;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class SensorHub : Hub
    {
        public Task Broadcast(string sender, Measurement measurement)
        {
            return Clients
                // Do not Broadcast to Caller:
                //.AllExcept(new[] { Context.ConnectionId })
                .Others
                .SendAsync("Broadcast", sender, measurement);
        }
    }
}
