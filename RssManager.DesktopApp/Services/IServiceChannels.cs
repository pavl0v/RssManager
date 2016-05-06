using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssManager.DesktopApp.Services
{
    public interface IServiceChannels
    {
        List<RssChannelDTO> GetChannels();
        Task<List<RssChannelDTO>> GetChannelsAsync();
    }
}
