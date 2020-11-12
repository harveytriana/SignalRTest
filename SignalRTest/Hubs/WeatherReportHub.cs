using Microsoft.AspNetCore.SignalR;
using SignalRTest.Shared;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class WeatherReportHub : Hub
    {
        public async Task Send(WeatherReport data)
        {
            // simulate delay
            await Task.Delay(100);

            await Clients.All.SendAsync("Receive", data);
        }
    }
}
