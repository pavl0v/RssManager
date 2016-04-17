using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssManager.Interfaces.BO
{
    public interface IRssChannel : IRssChannelDTO, IEntity
    {
        event EventHandler ItemsLoaded;

        List<IRssItemDTO> Items { get; }

        void Refresh();
        Task RefreshAsync();
        List<ISubscriberDTO> GetSubscribers(IUserRepository userRepository);
    }
}
