
using Microsoft.AspNetCore.SignalR;

namespace SignalRWebHub
{
    public class ContinuosSignal : BackgroundService
    {
        private readonly IHubContext<ChatHub, IChatHub> _chatHub;
        public ContinuosSignal(IHubContext<ChatHub, IChatHub> chatHub)
        {
            _chatHub = chatHub;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                // do something
                //  _chatHub.Clients.All.ReceiveMessage(i.ToString(), "Message" + i + "@" + DateTime.UtcNow.ToString());
                Console.WriteLine("Signal Message =>" + i + " Sent @" + DateTime.UtcNow.ToString());
                await Task.Delay(5000, stoppingToken);
                ++i;
            }
        }
    }
}
