using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XPFriend.JenkinsOnDesktop
{
    [TestClass]
    public class MainWindowTest
    {
        //[TestMethod]
        public void TestCurrentProperty()
        {
            // setup
            MainWindow mainWindow = new MainWindow();

            // when
            MainWindow.Current = mainWindow;

            // then
            Assert.AreSame(MainWindow.Current, mainWindow);
        }
    }
}
