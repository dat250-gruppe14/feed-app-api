namespace FeedAppApi.Models.Web;

public class DeviceWeb
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public PollWeb? ConnectedPoll { get; set; }
		public string? connectionToken { get; set; }
}
