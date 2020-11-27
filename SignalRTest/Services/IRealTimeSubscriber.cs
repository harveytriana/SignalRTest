namespace SignalRTest.Services
{
    public interface IRealTimeSubscriber
    {
        bool Subscribe(string user);
        bool Unsubscribe(string user);
    }
}
