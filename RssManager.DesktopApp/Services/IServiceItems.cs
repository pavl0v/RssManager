using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.DesktopApp.Services
{
    public interface IServiceItems
    {
        List<RssItemDTO> GetItems(long channelId, int pageNo, int pageSize);
    }
}
