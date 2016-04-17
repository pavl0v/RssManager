using RssManager.Interfaces.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Interfaces.DTO
{
    public interface IRssChannelDTO : IEntity
    {
        string RssCategory { get; set; }
        string RssCloud { get; set; }
        string RssCopyright { get; set; }
        string RssDescription { get; set; }
        string RssDocs { get; set; }
        string RssGenerator { get; set; }
        string RssImage { get; set; }
        string RssLanguage { get; set; }
        string RssLastBuildDate { get; set; }
        string RssLink { get; set; }
        string RssManagingEditor { get; set; }
        string RssPubDate { get; set; }
        string RssSkipDays { get; set; }
        string RssSkipHours { get; set; }
        string RssTextInput { get; set; }
        string RssTitle { get; set; }
        string RssTtl { get; set; }
        string RssWebMaster { get; set; }
        //long Id { get; set; }
        //long UserId { get; set; }
        string Name { get; set; }
        string Url { get; set; }
    }
}
