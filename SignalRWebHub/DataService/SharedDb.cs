using System.Collections.Concurrent;

namespace SignalRWebHub.DataService
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
        public ConcurrentDictionary<string, User> Users => _users;
    }
}
