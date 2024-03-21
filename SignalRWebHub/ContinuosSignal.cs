
using Microsoft.AspNetCore.SignalR;

namespace SignalRWebHub
{
    public class ContinuosSignal : BackgroundService
    {
        private readonly IHubContext<ChatHub> _chatHub;
        public ContinuosSignal(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                // do something
                _chatHub.ReceiveMessage("Message" + 1, "@" + DateTime.UtcNow.ToString());
                await Task.Delay(1000, stoppingToken);

            }
        }
    }
}
