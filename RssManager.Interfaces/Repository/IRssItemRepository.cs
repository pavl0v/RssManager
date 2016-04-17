using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Enum;
using System.Collections.Generic;

namespace RssManager.Interfaces.Repository
{
    public interface IRssItemRepository : IRepository<IRssItemDTO>
    {
        IEnumerable<IRssItemDTO> GetByChannelId(long channelId);
        IEnumerable<IRssItemDTO> GetByChannelId(long channelId, int pageNo, int pageSize, long userId);
        int SetReadState(long itemId, long userId, ReadState state);
        IRssItemDTO GetByGuid(string guid);
    }
}
