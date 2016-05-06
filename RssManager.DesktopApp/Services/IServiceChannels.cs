using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;

namespace RssManager.DesktopApp.Services
{
    public interface IServiceChannels
    {
        List<RssChannelDTO> GetChannels();
    }
}
