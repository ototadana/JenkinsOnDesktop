using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core.ScriptEngine
{
    [TestClass]
    public class BusinessInformationScriptEngineTest
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
        public void TestLoad()
        {
            // setup
            BusinessesFolder.Initialize("Check-Job-status");

            // when
            BusinessInformation information = BusinessInformationScriptEngine.Load("Check-Job-status");

            // then
            Assert.AreEqual("Check-Job-status", information.Name);
            Assert.AreEqual("Reports job statuses of a build server.", information.Synopsis);
            Assert.IsTrue(information.Description.StartsWith("Copyright "));
            Assert.IsTrue(information.Examples.Trim().StartsWith("[-url] <String>"));
        }
    }
}
