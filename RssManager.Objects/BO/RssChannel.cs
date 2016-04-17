using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using RssManager.Objects.Json;
using Newtonsoft.Json;
using RssManager.Objects.Exceptions;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.BO;
using RssManager.Objects.RssContentReader;
using RssManager.Interfaces.RssContentReader;
using RssManager.Interfaces.Repository;

namespace RssManager.Objects.BO
{
    public class RssChannel : IRssChannel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssChannel));
        private readonly IRssItemRepository rssItemRepository = null;

        #region EVENTS

        public event EventHandler ItemsLoaded;

        #endregion

        #region FIELDS

        private string category = string.Empty;
        private string cloud = string.Empty;
        private string copyright = string.Empty;
        private string description = string.Empty;
        private string docs = string.Empty;
        private string generator = string.Empty;
        private string image = string.Empty;
        private string language = string.Empty;
        private string lastBuildDate = string.Empty;
        private string link = string.Empty;
        private string managingEditor = string.Empty;
        private string pubDate = string.Empty;
        private string skipHours = string.Empty;
        private string skipDays = string.Empty;
        private string textInput = string.Empty;
        private string title = string.Empty;
        private string ttl = string.Empty;
        private string webMaster = string.Empty;

        private string url = string.Empty;
        private string name = "RSS-CHANNEL-NAME";
        private long id = 0;
        private long userId = 0;
        private string xml = string.Empty;
        private List<IRssItemDTO> items = new List<IRssItemDTO>();
        private IRssSourceContentReader reader = null;

        #endregion

        #region PROPERTIES

        public string RssCategory
        {
            get { return this.category; }
            set { this.category = value; }
        }

        public string RssCloud
        {
            get { return this.cloud; }
            set { this.cloud = value; }
        }

        public string RssCopyright
        {
            get { return this.copyright; }
            set { this.copyright = value; }
        }

        public string RssDescription
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string RssDocs
        {
            get { return this.docs; }
            set { this.docs = value; }
        }

        public string RssGenerator
        {
            get { return this.generator; }
            set { this.generator = value; }
        }

        public string RssImage
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public string RssLanguage
        {
            get { return this.language; }
            set { this.language = value; }
        }

        public string RssLastBuildDate
        {
            get { return this.lastBuildDate; }
            set { this.lastBuildDate = value; }
        }

        public string RssLink
        {
            get { return this.link; }
            set { this.link = value; }
        }

        public string RssManagingEditor
        {
            get { return this.managingEditor; }
            set { this.managingEditor = value; }
        }

        public string RssPubDate
        {
            get { return this.pubDate; }
            set { this.pubDate = value; }
        }

        public string RssSkipHours
        {
            get { return this.skipHours; }
            set { this.skipHours = value; }
        }

        public string RssSkipDays
        {
            get { return this.skipDays; }
            set { this.skipDays = value; }
        }

        public string RssTextInput
        {
            get { return this.textInput; }
            set { this.textInput = value; }
        }

        public string RssTitle
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string RssTtl
        {
            get { return this.ttl; }
            set { this.ttl = value; }
        }

        public string RssWebMaster
        {
            get { return this.webMaster; }
            set { this.webMaster = value; }
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        //public long UserId
        //{
        //    get { return this.userId; }
        //    set { this.userId = value; }
        //}

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [JsonConverter(typeof(JsonArrayToRssItemsCollectionConverter))]
        public List<IRssItemDTO> Items
        {
            get 
            {
                //if (this.items == null)
                //{
                //    if (this.id == 0)
                //    {
                //        this.items = new List<IRssItemDTO>();
                //    }
                //    else
                //    {
                //        this.items = rssItemRepository.GetByChannelId(this.id) as List<IRssItemDTO>;
                //    }
                //}
                return this.items; 
            }
            set { this.items = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public RssChannel(IRssSourceContentReader reader, IRssItemRepository rssItemRepository)
        {
            this.reader = reader;
            this.url = reader.Uri;
            this.rssItemRepository = rssItemRepository;
        }

        public RssChannel(IRssChannelDTO dto, IRssItemRepository rssItemRepository)
        {
            this.copyright = dto.RssCopyright;
            this.description = dto.RssDescription;
            this.id = dto.Id;
            this.language = dto.RssLanguage;
            this.title = dto.RssTitle;
            this.url = dto.Url;
            this.name = dto.Name;
            //this.userId = dto.UserId;

            if (this.url.StartsWith("http://") || this.url.StartsWith("https://"))
                this.reader = new RssHttpContentReader(this.url);
            else
                this.reader = new RssLocalContentReader(this.url);

            this.rssItemRepository = rssItemRepository;
        }

        #endregion

        #region PUBLIC METHODS

        public void Refresh()
        {
            #region READ

            try
            {
                this.xml = this.reader.Read();
            }
            catch(Exception ex)
            {
                throw new RssSourceReadException(this.reader.Uri, "Unable to read RSS source: " + this.reader.Uri, ex);
            }

            #endregion

            #region PARSE

            try
            {
                this.ParseXml(this.xml);
            }
            catch (Exception ex)
            {
                throw new RssSourceParseException(this.reader.Uri, "Unable to parse RSS source: " + this.reader.Uri, ex);
            }

            #endregion
        }

        public async System.Threading.Tasks.Task RefreshAsync()
        {
            Task t = new Task(() => Refresh());
            t.Start();
            await t.ContinueWith((x) => {
                if (this.ItemsLoaded != null)
                    this.ItemsLoaded(this, EventArgs.Empty);
            });
        }

        public List<ISubscriberDTO> GetSubscribers(IUserRepository userRepository)
        {
            return userRepository.GetByRssChannelId(this.id);
        }

        #endregion

        #region PRIVATE METHODS

        private void ParseXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement root = doc.DocumentElement;

            XmlNodeList channels = root.SelectNodes("//channel");
            XmlNode channel = channels[0];

            PropertyInfo[] props = this.GetType().GetProperties();
            IEnumerable<PropertyInfo> rssProps = props.Where(x => x.Name.StartsWith("Rss"));

            this.items.Clear();

            foreach (XmlNode channelNode in channel.ChildNodes)
            {
                string nodeName = channelNode.Name;
                if (nodeName == "item")
                {
                    #region RSS ITEM

                    IRssItemDTO item = new RssItem();
                    item.ChannelId = this.id;

                    PropertyInfo[] itemProperties = item.GetType().GetProperties();
                    foreach (XmlNode itemNode in (channelNode as XmlElement).ChildNodes)
                    {
                        string propertyName = ("rss" + itemNode.Name).ToLower();
                        PropertyInfo pi = itemProperties.Where(x => x.Name.ToLower() == propertyName).FirstOrDefault();
                        if (pi != null)
                        {
                            pi.SetValue(item, itemNode.InnerText);
                        }
                        else
                        {
                            //log.Error(string.Format("RssItem class does not contain property for: '{0}'", propertyName));
                        }
                    }

                    /**
                     * If refresh existing channel with collection of items
                     * then replace existing item with new one (Id and ChannelId are preserved)
                     */
                    //IRssItemDTO existingItem = this.Items.Where(x => x.RssGuid == item.RssGuid).FirstOrDefault();
                    //if (existingItem != null)
                    //{
                    //    item.Id = existingItem.Id;
                    //    if (existingItem.PubDateTime >= item.PubDateTime)
                    //        item.ReadState = existingItem.ReadState;
                    //    this.items.Remove(existingItem);
                    //}

                    this.items.Add(item);
                    

                    //IRssItemDTO existingItem2 = this.rssItemRepository.GetByGuid(item.RssGuid);
                    //if (existingItem2 != null)
                    //{
                    //    if (existingItem2.PubDateTime < item.PubDateTime)
                    //    {
                    //        item.Id = existingItem2.Id;
                    //        item.ReadState = Interfaces.Enum.ReadState.IsNew;
                    //        this.rssItemRepository.Update(item);
                    //    }
                    //}
                    //else
                    //{
                    //    this.rssItemRepository.Add(item);
                    //}

                    #endregion
                }
                else
                {
                    #region RSS CHANNEL

                    string propertyName = ("rss" + channelNode.Name).ToLower();
                    PropertyInfo pi = rssProps.Where(x => x.Name.ToLower() == propertyName).FirstOrDefault();
                    if (pi != null)
                    {
                        pi.SetValue(this, channelNode.InnerText);
                    }
                    else
                    {
                        //log.Error(string.Format("RssChannel class does not contain property for: '{0}'", propertyName));
                    }

                    #endregion
                }
            }

            if (this.ItemsLoaded != null)
                this.ItemsLoaded(this, EventArgs.Empty);
        }

        #endregion
    }
}
