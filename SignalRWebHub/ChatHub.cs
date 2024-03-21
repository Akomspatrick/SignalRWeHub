using Microsoft.AspNetCore.SignalR;
using SignalRWebHub.DataService;

namespace SignalRWebHub
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly SharedDb _sharedDb;
        // private readonly Notifiers _notifiers;
        public ChatHub(SharedDb sharedDb)//, Notifiers notifiers)
        {
            _sharedDb = sharedDb;
            //  _notifiers = notifiers;
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


        public async Task SendMessageToMany(string msg, string groupId, string ownerEmail)
        {


            //var group = _notifiers.GetGroup(groupId);
            //if (group != null)
            //{
            //    foreach (var participant in group.Participants)
            //    {
            //        await Clients.User(participant.Email).SendAsync("ReceiveMessage", msg);
            //    }
            //}
        }
    }

    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveSpecificMessage(string user, string message);
        Task ReceiveConnectedUsers(User[] users);
    }
}
