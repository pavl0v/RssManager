using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RssManager.DesktopApp.Services;
using Moq;
using System.Collections.Generic;
using RssManager.Interfaces.DTO;
using System.Collections;

namespace RssManager.UnitTests.DesktopApp
{
    [TestClass]
    public class ServiceChannelsTest
    {
        [TestMethod]
        public void GetChannelsTest()
        {
            List<RssChannelDTO> expected = new List<RssChannelDTO>();
            expected.Add(
                new RssChannelDTO
                {
                    Autorefresh = true,
                    Id = 85,
                    Name = "BBC",
                    RssCopyright = "Copyright: (C) British Broadcasting Corporation, s",
                    RssDescription = "The latest stories from the World section of the B",
                    RssLanguage = "en-gb",
                    RssTitle = "BBC News - World",
                    Url = "http://feeds.bbci.co.uk/news/world/rss.xml"
                }
            );
            Mock<IRequestApi> mockApi = new Mock<IRequestApi>();
            string rv = 
                "[{\"RssCategory\":null,\"RssCloud\":null,\"RssCopyright\":\"Copyright: (C) British Broadcasting Corporation, s\"," +
                "\"RssDescription\":\"The latest stories from the World section of the B\",\"RssDocs\":null,\"RssGenerator\":null," +
                "\"RssImage\":null,\"RssLanguage\":\"en-gb\",\"RssLastBuildDate\":null,\"RssLink\":null,\"RssManagingEditor\":null," +
                "\"RssPubDate\":null,\"RssSkipDays\":null,\"RssSkipHours\":null,\"RssTextInput\":null," +
                "\"RssTitle\":\"BBC News - World\",\"RssTtl\":null,\"RssWebMaster\":null,\"UserId\":0,\"Name\":\"BBC\"," +
                "\"Url\":\"http://feeds.bbci.co.uk/news/world/rss.xml\",\"Id\":85,\"Autorefresh\":true}]";
            mockApi.Setup<string>(t => t.Request(It.IsAny<Method>(), It.IsAny<string>())).Returns(rv);
            ServiceChannels s = new ServiceChannels(mockApi.Object);
            List<RssChannelDTO> channels = s.GetChannels();
            CollectionAssert.AreEqual(expected, channels, new RssChannelDTOComparerById());
        }
    }

    public class RssChannelDTOComparerById : IComparer
    {
        public int Compare(object x, object y)
        {
            var mx = x as RssChannelDTO;
            var my = y as RssChannelDTO;
            if (mx == null)
                return -1;
            if (my == null)
                return 1;

            return mx.Id.CompareTo(my.Id);
        }
    }
}
