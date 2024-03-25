using Newtonsoft.Json;

namespace SignalRWebHub.DataService
{


    public class Participants
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "general@massload.com";
        public string Status { get; set; } = "target";
    }

    public class Stakeholders : IStakeholders
    {
        private readonly string _sampleJsonFilePath = "Stakeholdersdb.json";
        //private List<Stakeholders> _stakeholders;
        //private string _stakeholdersstring;
        public string Room { get; set; }
        public IList<Participants> Participants { get; set; }

        public Stakeholders()
        {
            // _stakeholders = LoadNotifiersFromJson();
            // _stakeholdersstring = LoadJsonAstring();
        }

        public List<Stakeholders> LoadNotifiersFromJson()
        {
            try
            {
                using (StreamReader reader = new StreamReader(_sampleJsonFilePath))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Stakeholders>>(json);
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Please check notifiersdb.json", ex);
            }
        }
        //private string  LoadJsonAstring()
        //{
        //    try
        //    {
        //        using (StreamReader reader = new StreamReader(_sampleJsonFilePath))
        //        {
        //            var json = reader.ReadToEnd();
        //            return (json);
        //        }
        //    }
        //    catch (IOException ex)
        //    {
        //        throw new IOException("Please check notifiersdb.json", ex);
        //    }
        //}
        //private <List<Stakeholders> LoadNotifiersFromJsonAstring(string json)
        //{
        //    try
        //    {
        //        return JsonConvert.DeserializeObject<List<Stakeholders>>(json);
        //    }
        //    catch (IOException ex)
        //    {
        //        throw new IOException("Please check notifiersdb.json", ex);
        //    }
        //}

        // public Stakeholders GetGroup(string groupName) => _stakeholders.FirstOrDefault(x => x.Room == groupName);
        // public IEnumerable<Stakeholders> GetGroupList(string groupName) => _stakeholders;
    }


}
