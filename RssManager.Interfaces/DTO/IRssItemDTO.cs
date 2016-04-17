using RssManager.Interfaces.BO;
using RssManager.Interfaces.Enum;
using System;

namespace RssManager.Interfaces.DTO
{
    public interface IRssItemDTO : IEntity
    {
        long ChannelId { get; set; }
        //long Id { get; set; }
        DateTime PubDateTime { get; set; }
        ReadState ReadState { get; set; }
        string RssAuthor { get; set; }
        string RssCategory { get; set; }
        string RssComments { get; set; }
        string RssDescription { get; set; }
        string RssEnclosure { get; set; }
        string RssGuid { get; set; }
        string RssLink { get; set; }
        string RssPubDate { get; set; }
        string RssSource { get; set; }
        string RssTitle { get; set; }
    }
}
