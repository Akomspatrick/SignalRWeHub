namespace SignalRWebHub
{
    public class Message
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid MessageGroupGuid { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public DateTime DateViewed { get; set; }
        public DateTime DateCompleted { get; set; }
        public DateTime DateArchived { get; set; }


        public string FromUser { get; set; }
        public string ToUser { get; set; }
        //public string SourceEmail { get; set; }
        //public string ToEmail { get; set; }
        public string OriginatingApp { get; set; }
        public string MessageBody { get; set; }
        public string MessageTitle { get; set; } = string.Empty;
        public string MessageType { get; set; }
        public bool MessageOwner { get; set; }
        public string MessageUrl { get; set; }

        public string MessageId { get; set; }

        public string MessageStatus { get; set; }
        public string MessagePriority { get; set; }
       // public string MessageSubject { get; set; }

    }
}
