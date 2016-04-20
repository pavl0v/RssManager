using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RssManager.UnitTests
{
    [TestClass]
    public class SignInModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            DesktopApp.Models.SignInModel m = new DesktopApp.Models.SignInModel();
            m.Auth("demo", "demo");
        }
    }
}
