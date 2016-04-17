using log4net;
using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Enum;
using System;

namespace RssManager.Objects.BO
{
    public class RssItem : IRssItemDTO, IEntity
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssItem));

        #region FIELDS

        private string author = string.Empty;
        private string category = string.Empty;
        private string comments = string.Empty;
        private string description = string.Empty;
        private string enclosure = string.Empty;
        private string guid = string.Empty;
        private string link = string.Empty;
        private string pubDate = string.Empty;
        private string source = string.Empty;
        private string title = string.Empty;
        private DateTime pubDateTime = DateTime.MinValue;
        private long id = 0;
        private long channelId = 0;
        private ReadState state = ReadState.IsNew;
        private bool isChanged = false;

        #endregion

        #region PROPERTIES

        public string RssAuthor
        {
            get { return this.author; }
            set
            {
                if (this.author != value)
                {
                    this.author = value;
                    isChanged = true;
                }
            }
        }

        public string RssCategory
        {
            get { return this.category; }
            set
            {
                if (this.category != value)
                {
                    this.category = value;
                    isChanged = true;
                }
            }
        }

        public string RssComments
        {
            get { return this.comments; }
            set
            {
                if (this.comments != value)
                {
                    this.comments = value;
                    isChanged = true;
                }
            }
        }

        public string RssDescription
        {
            get { return this.description; }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    isChanged = true;
                }
            }
        }

        public string RssEnclosure
        {
            get { return this.enclosure; }
            set
            {
                if (this.enclosure != value)
                {
                    this.enclosure = value;
                    isChanged = true;
                }
            }
        }

        public string RssGuid
        {
            get { return this.guid; }
            set
            {
                if (this.guid != value)
                {
                    this.guid = value;
                    isChanged = true;
                }
            }
        }

        public string RssLink
        {
            get { return this.link; }
            set
            {
                if (this.link != value)
                {
                    this.link = value;
                    isChanged = true;
                }
            }
        }

        public string RssPubDate
        {
            get { return this.pubDate; }
            set
            {
                if (this.pubDate != value)
                {
                    this.pubDate = value;
                    this.ConvertPubDate();
                    isChanged = true;
                }
            }
        }

        public string RssSource
        {
            get { return this.source; }
            set
            {
                if (this.source != value)
                {
                    this.source = value;
                    isChanged = true;
                }
            }
        }

        public string RssTitle
        {
            get { return this.title; }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    isChanged = true;
                }
            }
        }

        public DateTime PubDateTime
        {
            get { return this.pubDateTime; }
            set
            {
                if (this.pubDateTime != value)
                {
                    this.pubDateTime = value;
                    isChanged = true;
                }
            }
        }

        public string PubDateTimeFormatted
        {
            get { return this.pubDateTime.ToString("dd.MM.yyyy HH:mm"); }
        }

        public long Id
        {
            get { return this.id; }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    isChanged = true;
                }
            }
        }

        public long ChannelId
        {
            get { return this.channelId; }
            set
            {
                if (this.channelId != value)
                {
                    this.channelId = value;
                    isChanged = true;
                }
            }
        }

        public ReadState ReadState
        {
            get { return this.state; }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    isChanged = true;
                }
            }
        }

        public bool IsChanged 
        {
            get { return this.isChanged; }
        }

        #endregion

        #region CONSTRUCTORS

        public RssItem()
        {
            
        }

        public RssItem(IRssItemDTO dto)
        {
            this.author = dto.RssAuthor;
            this.category = dto.RssCategory;
            this.channelId = dto.ChannelId;
            this.comments = dto.RssComments;
            this.description = dto.RssDescription;
            this.enclosure = dto.RssEnclosure;
            this.guid = dto.RssGuid;
            this.id = dto.Id;
            this.link = dto.RssLink;
            this.pubDate = dto.RssPubDate;
            this.pubDateTime = dto.PubDateTime;
            this.source = dto.RssSource;
            this.title = dto.RssTitle;
            this.ReadState = (ReadState)dto.ReadState;
        }

        #endregion

        public override bool Equals(object obj)
        {
            bool res = true;
            RssItem compare = obj as RssItem;

            if(compare == null)
                return false;

            res = res & this.RssAuthor == compare.RssAuthor;
            res = res & this.RssCategory == compare.RssCategory;
            res = res & this.RssComments == compare.RssComments;
            res = res & this.RssDescription == compare.RssDescription;
            res = res & this.RssEnclosure == compare.RssEnclosure;
            res = res & this.RssGuid == compare.RssGuid;
            res = res & this.RssLink == compare.RssLink;
            res = res & this.RssPubDate == compare.RssPubDate;
            res = res & this.RssSource == compare.RssSource;
            res = res & this.RssTitle == compare.RssTitle;

            return res;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region PRIVATE METHODS

        private void ConvertPubDate()
        {
            bool isSuccessful = DateTime.TryParse(this.RssPubDate, out this.pubDateTime);
            if (!isSuccessful)
            {
                this.pubDateTime = DateTime.MinValue;
                log.Error(string.Format("DateTime conversion failed for: {0}", this.pubDate));
            }
        }

        #endregion
    }
}
