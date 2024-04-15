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
        public string Sender { get; set; }//FromUser sender
        public string MainRecipient { get; set; } //ToUser
        public string AllRecipients { get; set; }// this is the email of people from the group

        public string OriginatingApp { get; set; } = "APPNAME";
        public string MessageBody { get; set; }
        public string MessageTitle { get; set; } = string.Empty;
        //public string MessageType { get; set; }
        public bool MessageOwner { get; set; }
        // public string MessageUrl { get; set; }

        public string MessageId { get; set; }
        public string RoomName { get; set; }
        public string MessageOwnerStatus { get; set; } = "NOTDONE";//,DONE,
        public string MessageStatus { get; set; } = "NOTVIEWED";//VIEWED,
        public bool MessageVisible { get; set; } = true;// if it is false, it means it has been archived
        //public bool MessagePriority { get; set; } = true;////High, Normal,


    }
}
