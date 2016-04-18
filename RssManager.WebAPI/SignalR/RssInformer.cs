using log4net;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
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

            List<ConnectionChannel> messages = new List<ConnectionChannel>();
            IEnumerable<IRssChannel> channels = this.rssChannelRepository.GetAll();
            foreach (IRssChannel channel in channels)
            {
                List<ConnectionChannel> message = null;
                try
                {
                    message = ProcessRssChannel(channel);
                    messages.AddRange(message);
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }
                
            }
            IEnumerable<string> subscribers = messages.Select(x => x.ConnectionId).Distinct();
            if (subscribers == null || subscribers.Count() == 0)
            {
                log.Error("There are no any subscribers");
                return;
            }
            
            foreach (string connectionId in subscribers)
            {
                string str = string.Empty;
                IEnumerable<ConnectionChannel> messages2subscriber = messages.Where(x => x.ConnectionId == connectionId);
                if (messages2subscriber != null)
                    str = JsonConvert.SerializeObject(messages2subscriber);
                this.backendHub.Clients.Client(connectionId).broadcastMessage(str);
            }
        }

        private List<ConnectionChannel> ProcessRssChannel(IRssChannel channel)
        {
            List<ConnectionChannel> res = new List<ConnectionChannel>();

            channel.Refresh();
            int newItems = 0;
            foreach (IRssItemDTO item in channel.Items)
            {
                IRssItemDTO existingItem = rssItemRepository.GetByGuid(item.RssGuid);
                if (existingItem == null)
                {
                    newItems++;
                    rssItemRepository.Add(item);
                    //break;
                }
            }

            if (newItems == 0)
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
                    res.Add(
                        new ConnectionChannel() 
                        { 
                            ConnectionId = connectionId, 
                            ChannelId = channel.Id, 
                            NewItems = newItems 
                        });
                }
            }

            return res;
        }

        class ConnectionChannel
        {
            [JsonIgnore]
            public string ConnectionId { get; set; }
            public long ChannelId { get; set; }
            public int NewItems { get; set; }
        }
    }
}