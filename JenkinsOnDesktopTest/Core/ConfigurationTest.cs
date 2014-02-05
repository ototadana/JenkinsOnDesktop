using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using System.IO;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            Configuration configuration = new Configuration();

            // then
            Assert.AreEqual(ButlerFactory.EmotionalJenkins, configuration.Butler);
            Assert.AreEqual(BusinessesFolder.CheckJobStatus, configuration.Business);
            Assert.AreEqual(50, configuration.DesktopMargin);
        }

        [TestMethod]
        public void TestSaveAndGetInstance()
        {
            {
                // when
                Configuration configuration = Configuration.GetInstance();

                // then
                Assert.AreEqual(ButlerFactory.EmotionalJenkins, configuration.Butler);
                Assert.AreEqual(BusinessesFolder.CheckJobStatus, configuration.Business);
                Assert.AreEqual(50, configuration.DesktopMargin);
                Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Calm-Jenkins")));
                Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Emotional-Jenkins")));
                Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "Time-keeping")));
                Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "Check-job-status")));
            }

            {
                // when
                new Configuration() { DesktopMargin = 40 }.Save();

                // then
                Configuration configuration = Configuration.GetInstance();
                Assert.AreEqual(ButlerFactory.EmotionalJenkins, configuration.Butler);
                Assert.AreEqual(BusinessesFolder.CheckJobStatus, configuration.Business);
                Assert.AreEqual(40, configuration.DesktopMargin);
            }
        }
    }
}
