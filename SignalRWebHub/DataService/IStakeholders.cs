namespace SignalRWebHub.DataService
{
    public interface IStakeholders
    {
        public string Room { get; set; }
        public IList<Participants> Participants { get; set; }

    }
}
