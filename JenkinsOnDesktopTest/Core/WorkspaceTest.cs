using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class WorkspaceTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
            Workspace.Current.Business = null;
            Workspace.Current.Butler = null;
            Workspace.Current.Configuration = null;
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            Workspace workspace = Workspace.Current;

            // then
            Assert.IsNotNull(workspace);
            Assert.IsNotNull(workspace.Report);
            Assert.IsNull(workspace.Business);
            Assert.IsNull(workspace.Butler);
            Assert.IsNull(workspace.Configuration);
            Assert.IsFalse(workspace.HasConfigurationFile);
        }

        [TestMethod]
        public void TestInitialize()
        {
            // setup
            Workspace workspace = Workspace.Current;
            Report report = workspace.Report;
            Assert.IsNull(workspace.Business);
            Assert.IsNull(workspace.Butler);
            Assert.IsNull(workspace.Configuration);
            Assert.IsFalse(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Calm-Jenkins")));

            // when
            workspace.Initialize();
            
            // then
            Assert.AreNotSame(report, workspace.Report);
            Assert.IsNotNull(workspace.Business);
            Assert.IsNotNull(workspace.Butler);
            Assert.IsNotNull(workspace.Configuration);
            Assert.IsFalse(workspace.HasConfigurationFile);
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Calm-Jenkins")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Butlers", "Emotional-Jenkins")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "Time-keeping")));
            Assert.IsTrue(Directory.Exists(Path.Combine(WorkspaceFolder.FullName, "Businesses", "Check-job-status")));
        }

        [TestMethod]
        public void TestSaveConfigurations()
        {
            // setup
            Workspace workspace = Workspace.Current;
            workspace.Initialize();
            Assert.IsFalse(workspace.HasConfigurationFile);
            string businessConfigFile = Path.Combine(WorkspaceFolder.FullName, "Businesses", "Check-job-status", "Config.xml");
            Assert.IsFalse(File.Exists(businessConfigFile));

            // when
            workspace.SaveConfigurations();

            // then
            Assert.IsTrue(workspace.HasConfigurationFile);
            Assert.IsTrue(File.Exists(businessConfigFile));
        }

        [TestMethod]
        public void TestDoBusiness()
        {
            // setup
            Workspace workspace = Workspace.Current;
            workspace.Initialize();
            workspace.Configuration.Business = "Time-keeping";
            workspace.SaveConfigurations();
            workspace.Initialize();
            Assert.IsFalse(workspace.Report.IsUpdated);
            Assert.IsFalse(workspace.Butler.HasNews);

            // when
            workspace.DoBusiness();

            // then
            Assert.IsTrue(workspace.Report.IsUpdated);
            Assert.IsTrue(workspace.Butler.HasNews);
        }
    }
}
