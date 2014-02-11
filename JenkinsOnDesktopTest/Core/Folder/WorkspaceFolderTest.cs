using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    [TestClass]
    public class WorkspaceFolderTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestMethod]
        public void TestHasConfigurationFile()
        {
            {
                // expect
                Assert.IsFalse(WorkspaceFolder.HasConfigurationFile);
            }
            {
                // setup
                WorkspaceFolder.Save(new Configuration());

                // expect
                Assert.IsTrue(WorkspaceFolder.HasConfigurationFile);
            }
        }

        [TestMethod]
        public void TestInitialize()
        {
            // when
            WorkspaceFolder.Initialize(new Configuration() { Butler = "abc", Business = "def" });

            // then
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "abc")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Calm-Jenkins")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Emotional-Jenkins")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "def")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "Time-keeping")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "Check-job-status")));
        }

        [TestMethod]
        public void TestSaveAndLoadConfiguration()
        {
            {
                // when
                Configuration configuration = WorkspaceFolder.LoadConfiguration();

                // then
                Assert.AreEqual(ButlerFactory.EmotionalJenkins, configuration.Butler);
                Assert.AreEqual(BusinessesFolder.CheckJobStatus, configuration.Business);
                Assert.AreEqual(50, configuration.DesktopMargin);
                Assert.IsFalse(WorkspaceFolder.HasConfigurationFile);
            }

            {
                // when
                WorkspaceFolder.Save(new Configuration() { DesktopMargin = 40 });

                // then
                Assert.IsTrue(WorkspaceFolder.HasConfigurationFile);
                Configuration configuration = WorkspaceFolder.LoadConfiguration();
                Assert.AreEqual(ButlerFactory.EmotionalJenkins, configuration.Butler);
                Assert.AreEqual(BusinessesFolder.CheckJobStatus, configuration.Business);
                Assert.AreEqual(40, configuration.DesktopMargin);
            }
        }

        [TestMethod]
        public void TestValidateAsFileName()
        {
            try
            {
                // when
                WorkspaceFolder.ValidateAsFileName("a:b");
                Assert.Fail("ここにはこない");
            }
            catch (ApplicationException e)
            {
                // then
                Console.WriteLine(e.Message);
            }
            {
                // expect
                WorkspaceFolder.ValidateAsFileName("abc.txt");
            }
        }
    }
}
