using System.Collections.Generic;

namespace SignalRTest.Services
{
    public class ChatSubscribers : IRealTimeSubscriber
    {
        readonly List<string> _users = new List<string>();

        public bool Subscribe(string user)
        {
            if (_users.Contains(user)) {
                return false;
            }
            _users.Add(user);
            return true;
        }

        public bool Unsubscribe(string user)
        {
            if (_users.Contains(user)) {
                _users.Remove(user);
                return true;
            }
            return false;
        }

        public bool IsSubscribed(string user)
        {
            return _users.Contains(user);
        }
    }
}
