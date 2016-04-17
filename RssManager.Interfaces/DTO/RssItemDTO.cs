using RssManager.Interfaces;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Enum;
using System;

namespace RssManager.Interfaces.DTO
{
    public class RssItemDTO : IRssItemDTO
    {
        private DateTime pubDateTime = DateTime.MinValue;

        public string RssAuthor { get; set; }
        public string RssCategory { get; set; }
        public string RssComments { get; set; }
        public string RssDescription { get; set; }
        public string RssEnclosure { get; set; }
        public string RssGuid { get; set; }
        public string RssLink { get; set; }
        public string RssPubDate { get; set; }
        public string RssSource { get; set; }
        public string RssTitle { get; set; }

        public long Id { get; set; }
        public long ChannelId { get; set; }
        public ReadState ReadState { get; set; }
        public string PubDateTimeFormatted { get; set; }
        public DateTime PubDateTime
        {
            get
            {
                return this.pubDateTime;
            }
            set
            {
                this.pubDateTime = value;
                this.PubDateTimeFormatted = this.pubDateTime.ToString("dd.MM.yyyy HH:mm");
            }
        }
    }
}
