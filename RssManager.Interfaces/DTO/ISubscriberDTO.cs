
namespace RssManager.Interfaces.DTO
{
    public interface ISubscriberDTO : IUserDTO
    {
        string ChannelName { get; set; }
    }
}
