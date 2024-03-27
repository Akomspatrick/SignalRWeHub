using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRWebHub.Controllers;
using SignalRWebHub.DataService;
using System.Text.RegularExpressions;

namespace SignalRWebHub
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly SharedDb _sharedDb;
        // private readonly List<Stakeholders> _stakeholders;
        private readonly List<Stakeholders> _realstakeholders;
        public ChatHub(SharedDb sharedDb, List<Stakeholders> realstakeholders)//, Notifiers notifiers)
        {
            _sharedDb = sharedDb;
            _realstakeholders = realstakeholders;   



            //var group = _stakeholders.GetGroup("Enginnering_Room");
            //if (group != null)
            //{
            //    foreach (var participant in group.Participants)
            //    {
            //        // await Clients.User(participant.Email).SendAsync("ReceiveMessage", msg);
            //        int x = 0;
            //    }
            //}
        }


        //public async Task SendMessage(User user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        public async Task JoinChatRoom(User user)
        {
            //  await Groups.AddToGroupAsync(Context.ConnectionId, user.ChatRoom);
            //await Clients.Group(user.ChatRoom).SendAsync("ReceiveMessage", user, $"{user.Name} has joined the chat");

            await Clients.All.ReceiveMessage("Admin", $"{user.Username} has joined the chat");
        }

        public async Task JoinChatGroupRoom(User user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, user.ChatRoom);
            _sharedDb.Users.TryAdd(Context.ConnectionId, user);
            await Clients.Group(user.ChatRoom).ReceiveMessage("Admin", $"{user.Username} has joined the chat");
            // await Clients.All.SendAsync("ReceiveMessage", user, $"{user.Name} has joined the chat");
        }
        public async Task LeaveChatGroupRoom(User user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.ChatRoom);
            await Clients.Group(user.ChatRoom).ReceiveMessage("Admin", $"{user.Username} has left the chat");
            // await Clients.All.SendAsync("ReceiveMessage", user, $"{user.Name} has left the chat");
        }

        public async Task SendPrivateMessage(User user, string message)
        {
            //  await Clients.User(user.Username).ReceiveMessage( user, message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string msg)
        {
            if (_sharedDb.Users.TryGetValue(Context.ConnectionId, out var user))
            {
                await Clients.Group(user.ChatRoom).ReceiveMessage(user.Username, msg);
            }
            //  await Clients.All.SendAsync("ReceiveMessage", user, msg);
            //await Clients.All.SendAsync("ReceiveConnectedUsers", _sharedDb.Users.Values);
        }


        //public async Task SendMessageToRoomParticipantsOld(string msg, string groupId, string ownerEmail)
        //{


        //    var group = GetGroup(groupId);
        //    Message message = new Message
        //    {
        //        FromUser = ownerEmail,
        //        MessageBody = msg,
        //        MessageGroupGuid = Guid.NewGuid(),
        //        DateSent = DateTime.UtcNow,
        //        MessageType = "Chat",
        //        MessageStatus = "Sent",
        //        MessagePriority = "Normal",
        //        MessageSubject = "Chat",
        //        MessageTitle = "Chat",
        //        MessageUrl = "Chat",
        //        MessageOwner = false,
        //        MessageId = Guid.NewGuid().ToString()
        //    };
        //    if (group != null)
        //    {
        //        foreach (var participant in group.Participants)
        //        {
        //            message.ToUser = participant.Email;
        //            message.ToEmail = participant.Email;
        //            message.MessageOwner = participant.Email == ownerEmail;
        //            message.Guid = Guid.NewGuid();

        //            await Clients.User(participant.Email).ReceiveSpecificMessage(message);
        //            //await Clients.User(participant.Email).ReceiveSpecificMessage(participant.Email,groupId);
        //        }
        //    }
        //}
        public async Task SendMessageToRoomParticipants(Stakeholders group, Message message, string ownerEmail)
        {
            if (group != null)
            {
                foreach (var participant in group.Participants)
                {
                    message.AllRecipients = participant.Email;
                    message.Sender = participant.Email;// this should be the email of the sender , get this from logged in user

                    message.MessageOwner = participant.Email == ownerEmail;
                    message.Guid = Guid.NewGuid();

                    await Clients.User(participant.Email).ReceiveSpecificMessage(message);
                }
            }
        }
        public async Task ReplyMessageToRoomParticipants( Message message)
        {
            var group = _realstakeholders.FirstOrDefault(x => x.Room == message.RoomName);
            if (group == null)
            {
                //_logger.LogInformation($"Group {messageDto.RoomName} not found");
                return;// NotFound($"Group {messageDto.RoomName} not found");
            }

            if (group != null)
            {
                foreach (var participant in group.Participants)
                {
                    message.AllRecipients = participant.Email;
                    message.Sender = participant.Email;// this should be the email of the sender , get this from logged in user

                    //message.MessageOwner = participant.Email == ownerEmail;
                    message.Guid = Guid.NewGuid();
                    await Clients.All.ReceiveSpecificMessage(message);
                   // await Clients.User(participant.Email).ReceiveSpecificMessage(message);
                }
            }
        }

    }

    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveSpecificMessage(Message message);
        Task ReceiveConnectedUsers(User[] users);
    }
}
