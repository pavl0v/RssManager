using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using RssManager.Objects.BO;
using RssManager.Interfaces.RssContentReader;
using RssManager.Objects.RssContentReader;
using RssManager.Repository.ADO;
using RssManager.Interfaces.Repository;
using RssManager.Interfaces.BO;
using RssManager.Interfaces.Enum;

namespace RssManager.UnitTests
{
    [TestClass]
    public class RssItemTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssItemTest));
        
        private IRssItemRepository rssItemRepository = null;
        private IRssChannelRepository rssChannelRepository = null;
        private IRssChannel rssChannel = null;

        [TestInitialize()]
        public void Startup()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net-config.xml"));

            #region CREATE AND INSERT CHANNEL

            rssItemRepository = new RssItemRepository();
            rssChannelRepository = new RssChannelRepository(rssItemRepository);

            string rssChannelUri = "Data\\rian.ru.xml";
            IRssSourceContentReader reader = new RssLocalContentReader(rssChannelUri);
            
            rssChannel = new RssChannel(reader, rssItemRepository);
            rssChannel.Refresh();
            rssChannelRepository.Save(rssChannel);

            #endregion
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void UpdateRssItemTest()
        {
            if(this.rssChannel == null)
                Assert.Fail("Rss channel is NULL");

            #region GET RSSITEM 1

            if (this.rssChannel.Items == null || this.rssChannel.Items.Count == 0)
                Assert.Fail("Rss channel has no items");

            long rssItemId = this.rssChannel.Items[0].Id;

            //RssItem rssItem1 = RssItem.Create(rssItemId);
            RssItem rssItem1 = rssItemRepository.Get(rssItemId) as RssItem;
            
            if(rssItem1 == null)
                Assert.Fail("Rss item is NULL");

            #endregion

            #region UPDATE RSSITEM 1

            string testString = "TEST STRING";

            rssItem1.ReadState = ReadState.IsInvisible;
            rssItem1.RssAuthor = testString;
            rssItem1.RssCategory = testString;
            rssItem1.RssComments = testString;
            rssItem1.RssDescription = testString;
            rssItem1.RssEnclosure = testString;
            //rssItem1.RssGuid = testString;
            rssItem1.RssLink = testString;
            rssItem1.RssPubDate = DateTime.Now.ToString();
            rssItem1.RssSource = testString;
            rssItem1.RssTitle = testString;
            //rssItem1.Save();
            this.rssItemRepository.Save(rssItem1);

            #endregion

            #region GET RSSITEM 2

            RssItem rssItem2 = this.rssItemRepository.Get(rssItemId) as RssItem;
            if (rssItem2 == null)
                Assert.Fail("Rss item is NULL");

            #endregion

            Assert.IsTrue(rssItem1.Equals(rssItem2));
        }
    }
}
