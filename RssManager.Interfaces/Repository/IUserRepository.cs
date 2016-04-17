using RssManager.Interfaces.DTO;
using System.Collections.Generic;

namespace RssManager.Interfaces.Repository
{
    public interface IUserRepository : IRepository<IUserDTO>
    {
        IUserDTO GetByUsername(string username);
        List<ISubscriberDTO> GetByRssChannelId(long channelId);
    }
}
