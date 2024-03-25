using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SignalRWebHub.DataService;

namespace SignalRWebHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly List<Stakeholders> _realstakeholders;
        private readonly IHubContext<ChatHub, IChatHub> _chatHub;
        // IOptions<Stakeholders> options
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubContext<ChatHub, IChatHub> chatHub, List<Stakeholders> realstakeholders)
        {


            _logger = logger;
            _chatHub = chatHub;
            _realstakeholders = realstakeholders; //;LoadStakeholders();
        }

        // [HttpGet(Name = "GetWeatherForecast")]
        // public IEnumerable<WeatherForecast> Get()
        // {

        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        // Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        // TemperatureC = Random.Shared.Next(-20, 55),
        // Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
        // }


        [HttpGet(Name = "NewGroup")]
        public Stakeholders NewGroup(string Id)
        {
            var group = _realstakeholders.FirstOrDefault(x => x.Room == Id);
            return group;




        }



        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> SendMessage(string groupId = "Enginnering_Room", string content = "This is the content", string sender = "sender@massload.com", string targetRecipient = "user2@user2", string messageTitle = "MessageTitle", string messagePriority = "Normal")
        {
            var group = _realstakeholders.FirstOrDefault(x => x.Room == groupId);
            if (group == null)
            {
                return NotFound();
            }

            Message message = new Message
            {
                Sender = sender,
                MainRecipient = targetRecipient,
                AllRecipients = "",
                MessageBody = content,
                MessageGroupGuid = Guid.NewGuid(),
                DateSent = DateTime.UtcNow,
                MessageType = "messageType",
                MessageStatus = "messageStatus",
                MessagePriority = messagePriority,
                MessageTitle = messageTitle,
                MessageUrl = "messageUrl",
                MessageOwner = false,
                MessageId = Guid.NewGuid().ToString()
            };
            Console.WriteLine("Message sent" + message.MessageBody);
            await SendMessageToRoomParticipants(group, message);
            return Ok();
        }
        private readonly string _stakeholdersFilePath = "Stakeholdersdb.json";
        private List<Stakeholders> LoadStakeholders()
        {
            string json = System.IO.File.ReadAllText(_stakeholdersFilePath);
            return JsonConvert.DeserializeObject<List<Stakeholders>>(json);
        }
        // public Stakeholders GetGroup(string groupName) => _stakeholders.FirstOrDefault(x => x.Room == groupName);
        //}
        private async Task SendMessageToRoomParticipants(Stakeholders group, Message message)
        {

            if (group != null)
            {
                var UserNotInList = group.Participants.FirstOrDefault(x => x.Email == message.MainRecipient);
                if ((UserNotInList == null) && (message.MainRecipient != ""))
                {

                    _logger.LogInformation($"User {message.MainRecipient} not in List of Participants of {group.Room}");
                    Console.WriteLine($"User {message.MainRecipient} not in List of Participants of {group.Room}");

                }
                foreach (var participant in group.Participants)
                {
                    message.AllRecipients = participant.Email;
                    message.MessageOwner = participant.Email == message.MainRecipient;
                    message.Guid = Guid.NewGuid();
                    Console.WriteLine("Message sent to " + participant.Email);
                    await _chatHub.Clients.All.ReceiveSpecificMessage(message);
                }
            }
        }
    }
}