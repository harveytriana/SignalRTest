using System.Threading.Tasks;

namespace SignalRTest.Services
{
    public interface IRealTimeSubscriber
    {
        Task<bool> Subscribe(string user, string id);
        Task<bool> Unsubscribe(string user);
    }
}
