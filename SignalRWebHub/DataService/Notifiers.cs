using Newtonsoft.Json;

namespace SignalRWebHub.DataService
{


    public class Participants
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "general@massload.com";
        public string Status { get; set; } = "target";
    }

    public class Notifiers
    {
        private readonly string _sampleJsonFilePath = "notifiersdb.json";
        private List<Notifiers> _notifiers;

        public string Room { get; set; }
        public IList<Participants> Participants { get; set; }

        public Notifiers()
        {
            _notifiers = LoadNotifiersFromJson();
        }

        private List<Notifiers> LoadNotifiersFromJson()
        {
            try
            {
                using (StreamReader reader = new StreamReader(_sampleJsonFilePath))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Notifiers>>(json);
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Please check notifiersdb.json", ex);
            }
        }

        public Notifiers GetGroup(string groupName) => _notifiers.FirstOrDefault(x => x.Room == groupName);
        public IEnumerable<Notifiers> GetGroupList(string groupName) => _notifiers;
    }


}
