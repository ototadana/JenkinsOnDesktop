using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class BusinessInformationTest
    {
        private CultureInfo defaultUICulture;

        [TestInitialize]
        public void Setup()
        {
            defaultUICulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentUICulture = defaultUICulture;
        }

        [TestMethod]
        public void TestGetInstance()
        {
            {
                // when
                BusinessInformation information = BusinessInformation.GetInstance("xxx");

                // then
                Assert.AreEqual("xxx", information.Name);
                Assert.AreEqual(null, information.Synopsis);
                Assert.AreEqual(null, information.Description);
                Assert.AreEqual(null, information.Examples);
            }

            {
                // setup
                BusinessesFolder.Initialize("zzz");

                // when
                BusinessInformation information = BusinessInformation.GetInstance("zzz");

                // then
                Assert.AreEqual("zzz", information.Name);
                Assert.AreEqual("Reports current time.", information.Synopsis);
                Assert.IsTrue(information.Description.StartsWith("Copyright "));
                Assert.AreEqual("[<CommonParameters>]", information.Examples.Trim());
            }
        }
    }
}
