using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using XPFriend.JenkinsOnDesktop.Core;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop
{
    [TestClass]
    public class AppTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (MainWindow.Current != null)
            {
                MainWindow.Current.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestHandleException()
        {
            // setup
            App app = new App();
            MainWindow window = new MainWindow();
            app.MainWindow = window;

            // when
            app.HandleException(new Exception("exception1", new Exception("exception2")));

            // then
            string errorLogFolder = Path.Combine(WorkspaceFolder.FullName, "Logs");
            Assert.IsTrue(Directory.Exists(errorLogFolder));
            string[] logFiles = Directory.GetFiles(errorLogFolder);
            Assert.AreEqual(1, logFiles.Length);
            string logFile = logFiles[0];
            
            // assert file name
            Assert.IsTrue(Path.GetFileName(logFile).StartsWith("error_" + DateTime.Now.ToString("yyyy-MM-dd_HH")));
            Assert.IsTrue(logFile.EndsWith(".txt"));

            // assert file contents
            string logText = File.ReadAllText(logFile);
            Assert.IsTrue(logText.Contains("exception1"));
            Assert.IsTrue(logText.Contains("exception2"));
            Assert.IsTrue(logText.Contains(" ---> "));
            Assert.IsTrue(logText.Contains(logFile));
        }
    }
}
