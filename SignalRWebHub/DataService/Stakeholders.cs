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
        public string Room { get; set; }
        public IList<Participants> Participants { get; set; }

        public Stakeholders()
        {
  
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
 
    }


}
