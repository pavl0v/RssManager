using log4net;
using Microsoft.AspNet.SignalR;
using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Repository;
using RssManager.WebAPI.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RssManager.WebAPI.SignalR
{
    public class RssInformer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssInformer));

        private readonly IRssChannelRepository rssChannelRepository = null;
        private readonly IRssItemRepository rssItemRepository = null;
        private readonly IUserRepository userRepository = null;
        private readonly ISettingsRepository settingsRepository = null;
        private readonly IHubContext backendHub;

        public RssInformer(
            IRssChannelRepository rssChannelRepository,
            IRssItemRepository rssItemRepository, 
            IUserRepository userRepository,
            ISettingsRepository settingsRepository)
        {
            this.rssChannelRepository = rssChannelRepository;
            this.rssItemRepository = rssItemRepository;
            this.userRepository = userRepository;
            this.settingsRepository = settingsRepository;
            this.backendHub = GlobalHost.ConnectionManager.GetHubContext<BackendHub>();
        }

        public void Broadcast() 
        {
            ISettings s = this.settingsRepository.Get();
            bool autoRefresh = false;
            bool.TryParse(s["AUTOREFRESH"], out autoRefresh);

            if (!autoRefresh)
                return;

            //System.Diagnostics.Debug.WriteLine("AUTOREFRESH IS ON");
            log.Error("AUTOREFRESH IS ON");

            List<ConnectionChannel> destinations = new List<ConnectionChannel>();
            IEnumerable<IRssChannel> channels = this.rssChannelRepository.GetAll();
            foreach (IRssChannel channel in channels)
            {
                List<ConnectionChannel> dest = null;
                try
                {
                    dest = ProcessRssChannel(channel);
                    destinations.AddRange(dest);
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }
                
            }
            IEnumerable<string> connections = destinations.Select(x => x.ConnectionId).Distinct();
            if (connections == null || connections.Count() == 0)
            {
                log.Error("There are no any subscribers");
                return;
            }
            
            foreach (string connectionId in connections)
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<string> strChannelIds = destinations.Where(x => x.ConnectionId == connectionId).Select(x => x.ChannelId.ToString());
                if (strChannelIds != null)
                {
                    foreach (string strId in strChannelIds)
                    {
                        sb.Append(strId);
                        sb.Append(";");
                    }
                }
                this.backendHub.Clients.Client(connectionId).broadcastMessage(sb.ToString().Trim());
            }
        }

        private List<ConnectionChannel> ProcessRssChannel(IRssChannel channel)
        {
            List<ConnectionChannel> res = new List<ConnectionChannel>();

            channel.Refresh();
            bool newItems = false;
            foreach (IRssItemDTO item in channel.Items)
            {
                IRssItemDTO existingItem = rssItemRepository.GetByGuid(item.RssGuid);
                if (existingItem == null)
                {
                    newItems = true;
                    rssItemRepository.Add(item);
                    //break;
                }
            }

            if (!newItems)
                return res;

            List<ISubscriberDTO> channelSubscribers = channel.GetSubscribers(this.userRepository);

            IEnumerable<string> names = BackendHub.connections.GetKeys();
            foreach (string name in names)
            {
                ISubscriberDTO t = channelSubscribers.Where(x => x.UserName == name).FirstOrDefault();
                if (t == null)
                    continue;

                foreach (var connectionId in BackendHub.connections.GetConnections(name))
                {
                    res.Add(new ConnectionChannel { ConnectionId = connectionId, ChannelId = channel.Id });
                }
            }

            return res;
        }

        class ConnectionChannel
        {
            public string ConnectionId { get; set; }
            public long ChannelId { get; set; }
        }
    }
}