using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using RssManager.Interfaces.RssContentReader;
using RssManager.Objects.RssContentReader;
using RssManager.Objects.BO;
using RssManager.Interfaces.Repository;
using RssManager.Repository.ADO;
using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;

namespace RssManager.UnitTests
{
    [TestClass]
    public class RssChannelTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssChannelTest));

        private IRssItemRepository rssItemRepository = null;
        private IRssChannelRepository rssChannelRepository = null;
        private IUserRepository userRepository = null;
        private IUserDTO user = null; 

        [TestInitialize()]
        public void Startup()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net-config.xml"));
            log.Info("Entering application.");

            rssItemRepository = new RssItemRepository();
            rssChannelRepository = new RssChannelRepository(rssItemRepository);
            userRepository = new UserRepository();
            user = userRepository.GetByUsername("demo");
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public async System.Threading.Tasks.Task RerfeshTest()
        {
            IRssSourceContentReader reader = new RssHttpContentReader("http://ria.ru/export/rss2/politics/index.xml");
            RssChannel rss = new RssChannel(reader, rssItemRepository);
            rss.ItemsLoaded += rss_ItemsLoaded;
            await rss.RefreshAsync();
        }

        void rss_ItemsLoaded(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("==================");
            System.Diagnostics.Trace.WriteLine("OK");
            System.Diagnostics.Trace.WriteLine("==================");
        }

        [TestMethod]
        public void ParseXmlTest()
        {
            log.Info("ParseXmlTest().");
            
            //IRssSourceContentReader reader = new RssHttpContentReader("http://ria.ru/export/rss2/politics/index.xml");
            //IRssSourceContentReader reader = new RssHttpContentReader("http://feeds.feedburner.com/ProbloggerHelpingBloggersEarnMoney");
            IRssSourceContentReader reader = new RssLocalContentReader("Data\\rian.ru.xml");

            RssChannel rss = new RssChannel(reader, rssItemRepository);
            rss.Refresh();
        }

        [TestMethod]
        public void SelectRssChannelTest()
        {
            //string uri = "http://ria.ru/export/rss2/politics/index.xml";
            //string uri = "http://feeds.feedburner.com/ProbloggerHelpingBloggersEarnMoney";
            string uri = "Data\\rian.ru.xml";

            #region CREATE AND INSERT CHANNEL

            IRssSourceContentReader reader = new RssLocalContentReader(uri);
            RssChannel rss = new RssChannel(reader, rssItemRepository);
            //rss.UserId = user.Id;
            rss.Refresh();
            rssChannelRepository.Add(rss);
            Assert.AreNotEqual(0, rss.Id);

            #endregion

            #region SELECT CHANNEL

            IRssChannel selectedChannel = rssChannelRepository.Get(rss.Id);
            CollectionAssert.AreEqual(selectedChannel.Items, rss.Items);

            #endregion

            #region DELETE CHANNEL

            int del = 0;
            del = rssChannelRepository.Delete(rss.Id);
            Assert.AreEqual(1, del);

            #endregion
        }
    }
}
