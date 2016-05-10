using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RssManager.DesktopApp.Services;
using RssManager.Interfaces.DTO;
using System.Collections.Generic;

namespace RssManager.UnitTests.DesktopApp
{
    [TestClass]
    public class ServiceItemsTest
    {
        [TestMethod]
        public void GetItemsTest()
        {
            ServiceItems s = new ServiceItems(new RequestApi());
            List<RssItemDTO> items = s.GetItems(85, 1, 20);
        }
    }
}
