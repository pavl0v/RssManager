using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using System.Collections.Generic;

namespace RssManager.Interfaces.Repository
{
    public interface IRssChannelRepository : IRepository<IRssChannel>
    {
        IEnumerable<IRssChannelDTO> GetByUserId(long userId);
        IRssChannel Get(long channelId, long userId);
        IRssChannelDTO GetDTO(long channelId, long userId);
        void AddByUserId(IRssChannel entity, long userId);
        void UpdateByUserId(IRssChannelDTO entity, long userId);
        int DeleteByUserId(long channelId, long userId);
    }
}
