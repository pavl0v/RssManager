using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Interfaces.DTO
{
    public class RssChannelDTO : IRssChannelDTO
    {
        public string RssCategory { get; set; }
        public string RssCloud { get; set; }
        public string RssCopyright { get; set; }
        public string RssDescription { get; set; }
        public string RssDocs { get; set; }
        public string RssGenerator { get; set; }
        public string RssImage { get; set; }
        public string RssLanguage { get; set; }
        public string RssLastBuildDate { get; set; }
        public string RssLink { get; set; }
        public string RssManagingEditor { get; set; }
        public string RssPubDate { get; set; }
        public string RssSkipDays { get; set; }
        public string RssSkipHours { get; set; }
        public string RssTextInput { get; set; }
        public string RssTitle { get; set; }
        public string RssTtl { get; set; }
        public string RssWebMaster { get; set; }

        public long UserId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public long Id { get; set; }
    }
}
