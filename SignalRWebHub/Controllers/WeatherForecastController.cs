using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SignalRWebHub.DataService;
using System.Collections.Specialized;
using System;
using System.Reflection;
using System.Text;

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


        //[HttpGet(Name = "NewGroup")]
        //public Stakeholders NewGroup(string Id)
        //{
        //    var group = _realstakeholders.FirstOrDefault(x => x.Room == Id);
        //    return group;





        //}
        [HttpGet(Name = "NewGroup")]
        public  IActionResult NewGroup(string Id)
        {
            //var group = _realstakeholders.FirstOrDefault(x => x.Room == Id);
            //return group;


            /// create a new httpresponse object
            /// 
            HttpResponse response = HttpContext.Response;
            NameValueCollection collections = new NameValueCollection();
            //collections.Add("FirstName", txtFirstName.Text.Trim());
            //collections.Add("LastName", txtLastName.Text.Trim());
            string remoteUrl = "http://localhost:3000/quickstart?value=Akoms";

            response.Clear();

            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", remoteUrl);
            //foreach (string key in data)
            //{
            //    s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", key, data[key]);
            //}
            s.Append("</form></body></html>");
             response.WriteAsync(s.ToString());

            return Ok();
           

        }


        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            // Use messageDto properties here...


            // public async Task<IActionResult> SendMessage([FromBody] string groupId = "Enginnering_R``oom2", string messageStatus = "NEW", bool messageVisible = true, string content = "This is the content", string sender = "sender@massload.com", string targetRecipient = "user2@user2", string messageTitle = "MessageTitle", string messagePriority = "Normal")

            var group = _realstakeholders.FirstOrDefault(x => x.Room == messageDto.RoomName);
            if (group == null)
            {
                _logger.LogInformation($"Group {messageDto.RoomName} not found");
                return NotFound($"Group {messageDto.RoomName} not found");
            }

            Message message = new Message
            {
                RoomName= messageDto.RoomName,
                //MessageOwnerStatus = messageDto.MessageOwnerStatus,
                Sender = messageDto.Sender,
                MainRecipient = messageDto.TargetRecipient,
                AllRecipients = "",
                MessageBody = messageDto.Content,
                MessageGroupGuid = Guid.NewGuid(),
                DateSent = DateTime.UtcNow,
               // MessageType = "messageType",
                MessageStatus = messageDto.MessageStatus,
                //MessagePriority = messageDto.MessagePriority,
                MessageTitle = messageDto.MessageTitle,
                //MessageUrl = "messageUrl",
                MessageOwner = false,
                MessageVisible = messageDto.MessageVisible,
                MessageId = Guid.NewGuid().ToString()
            };
            Console.WriteLine("Message sent" + message.MessageBody);
            await SendMessageToRoomParticipants(group, message);
            return Ok();
        }

        //[HttpPost(Name = "SendMessage")]
        //public async Task<IActionResult> SendMessage(string groupId = "Enginnering_Room",string messageStatus="NEW", bool messageVisible=true,  string content = "This is the content", string sender = "sender@massload.com", string targetRecipient = "user2@user2", string messageTitle = "MessageTitle", string messagePriority = "Normal")
        //{
        //    var group = _realstakeholders.FirstOrDefault(x => x.Room == groupId);
        //    if (group == null)
        //    {
        //        return NotFound();
        //    }

        //    Message message = new Message
        //    {
        //        Sender = sender,
        //        MainRecipient = targetRecipient,
        //        AllRecipients = "",
        //        MessageBody = content,
        //        MessageGroupGuid = Guid.NewGuid(),
        //        DateSent = DateTime.UtcNow,
        //        MessageType = "messageType",
        //        MessageStatus = messageStatus,
        //        MessagePriority = messagePriority,
        //        MessageTitle = messageTitle,
        //        MessageUrl = "messageUrl",
        //        MessageOwner = false,
        //        MessageVisible = messageVisible,
        //        MessageId = Guid.NewGuid().ToString()
        //    };
        //    Console.WriteLine("Message sent" + message.MessageBody);
        //    await SendMessageToRoomParticipants(group, message);
        //    return Ok();
        //}
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