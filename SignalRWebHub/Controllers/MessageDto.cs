namespace SignalRWebHub.Controllers
{
    public class MessageDto
    {

        public string RoomName { get; set; } = "Enginnering_Room2";
        public string MessageStatus { get; set; } = "NEW";
        public bool MessageVisible { get; set; } = true;
        public string Content { get; set; } = "This is the content";
        public string Sender { get; set; } = "sender@massload.com";
        public string TargetRecipient { get; set; } = "user2@user2";
        public string MessageTitle { get; set; } = "MessageTitle";
        public bool MessagePriority { get; set; } = true;

    }
}
