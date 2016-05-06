using Newtonsoft.Json;
using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.DesktopApp.Services
{
    public class ServiceChannels : IServiceChannels
    {
        private IRequestApi api = null;

        public ServiceChannels(IRequestApi api)
        {
            this.api = api;
        }

        public List<RssChannelDTO> GetChannels()
        {
            List<RssChannelDTO> channels = null;

            string json = this.api.Request(Method.GET, "http://localhost:64910/api/rsschannels");
            channels = JsonConvert.DeserializeObject<List<RssChannelDTO>>(json);

            return channels;
        }
    }
}
