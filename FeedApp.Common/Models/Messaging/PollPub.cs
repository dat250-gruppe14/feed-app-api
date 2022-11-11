namespace FeedApp.Common.Models.Messaging
{
    public class PollPub
    {
        public Guid Id { get; set; }
        public string Pincode { get; set; }
        public string Question { get; set; }
        public string OptionOne { get; set; }
        public string OptionTwo { get; set; }
        public PollCountsPub Counts { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public UserPub Owner { get; set; }
    }
}
