using Newtonsoft.Json;
using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.DesktopApp.Services
{
    public class ServiceItems : IServiceItems
    {
        private IRequestApi api = null;

        public ServiceItems(IRequestApi api)
        {
            this.api = api;
        }

        public List<RssItemDTO> GetItems(long channelId, int pageNo, int pageSize)
        {
            List<RssItemDTO> items = null;

            string url = string.Format("http://localhost:64910/api/rssitems?channelid={0}&pageno={1}&pagesize={2}", channelId, pageNo, pageSize);
            string json = this.api.Request(Method.GET, url);
            items = JsonConvert.DeserializeObject<List<RssItemDTO>>(json);

            return items;
        }
    }
}
