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
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubContext<ChatHub, IChatHub> chatHub)
        {


            _logger = logger;
            _chatHub = chatHub;
            _realstakeholders = LoadStakeholders();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> SendMessage(string groupId, string msg, string ownerEmail)
        {
            var group = _realstakeholders.FirstOrDefault(x => x.Room == groupId);

            Message message = new Message
            {
                FromUser = ownerEmail,
                MessageBody = msg,
                MessageGroupGuid = Guid.NewGuid(),
                DateSent = DateTime.UtcNow,
                MessageType = "Chat",
                MessageStatus = "Sent",
                MessagePriority = "Normal",
                MessageSubject = "Chat",
                MessageTitle = "Chat",
                MessageUrl = "Chat",
                MessageOwner = false,
                MessageId = Guid.NewGuid().ToString()
            };
            await SendMessageToRoomParticipants(group, message, ownerEmail);
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
        private async Task SendMessageToRoomParticipants(Stakeholders group, Message message, string ownerEmail)
        {
            if (group != null)
            {
                foreach (var participant in group.Participants)
                {
                    message.ToUser = participant.Email;
                    message.FromUser = participant.Email;// this should be the email of the sender , get this from logged in user
                                                         // message.ToEmail = participant.Email;
                    message.MessageOwner = participant.Email == ownerEmail;
                    message.Guid = Guid.NewGuid();

                    await _chatHub.Clients.All.ReceiveSpecificMessage(message);
                }
            }
        }
    }
}
