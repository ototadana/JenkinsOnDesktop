using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using System.IO;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class ButlerTest
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
        public void TestGetAppearance()
        {
            // setup
            Butler butler = ButlerFactory.CreateCalmJenkins();

            // expect
            Assert.AreSame(butler.Appearances[ButlerFactory.Normal], butler.GetAppearance(null));
            Assert.AreSame(butler.Appearances[ButlerFactory.Normal], butler.GetAppearance(""));
            Assert.AreSame(butler.Appearances[ButlerFactory.Normal], butler.GetAppearance("  "));
            Assert.AreSame(butler.Appearances[ButlerFactory.Normal], butler.GetAppearance("xx"));
            Assert.AreSame(butler.Appearances[ButlerFactory.Normal], butler.GetAppearance(ButlerFactory.Normal));
        }

        [TestMethod]
        public void TestSetDefaultAppearance()
        {
            // setup
            Butler butler = ButlerFactory.CreateCalmJenkins();

            // when
            butler.SetDefaultAppearance();

            // then
            Assert.AreEqual(false, butler.HasNews);
            Assert.AreEqual(false, butler.HasMessage);
            Assert.AreEqual(null, butler.SourceUrl);
            Assert.AreEqual(null, butler.MessageText);
            Assert.AreEqual(null, butler.BalloonTipText);
            Assert.AreEqual(null, butler.BalloonTipTitle);
            Assert.AreEqual(null, butler.Icon);
            Assert.AreEqual(null, butler.Image);
            Assert.AreEqual(ToolTipIcon.None, butler.ToolTipIcon);
            Assert.AreEqual(10, butler.BalloonTipTimeout);
            Assert.AreEqual(false, butler.Topmost);
            Assert.IsNotNull(butler.MessageStyle);
            Assert.IsNotNull(butler.EnterAnimation);
            Assert.IsNotNull(butler.ExitAnimation);
        }

        [TestMethod]
        public void TestUpdateAppearance()
        {
            // setup
            Report report = new Report();
            report.Hashtable.Add("IsUpdated", true);
            report.Hashtable.Add("SourceUrl", "http://localhost/");
            report.Hashtable.Add("Subject", "aaa");
            report.Hashtable.Add("CurrentStatus", "bbb");
            report.Hashtable.Add("Title", "ccc");

            {
                // when
                Butler butler = ButlerFactory.CreateEmotionalJenkins();
                butler.UpdateAppearance(ButlerFactory.Sad, report);

                // then
                Assert.AreEqual(true, butler.HasNews);
                Assert.AreEqual(true, butler.HasMessage);
                Assert.AreEqual("http://localhost/", butler.SourceUrl);
                Assert.AreEqual("aaa bbb", butler.MessageText);
                Assert.AreEqual(null, butler.BalloonTipText);
                Assert.AreEqual(null, butler.BalloonTipTitle);
                Assert.AreEqual(null, butler.Icon);
                Assert.AreEqual(null, butler.Image);
                Assert.AreEqual(ToolTipIcon.None, butler.ToolTipIcon);
                Assert.AreEqual(10, butler.BalloonTipTimeout);
                Assert.AreEqual(false, butler.Topmost);
                Assert.IsNotNull(butler.MessageStyle);
                Assert.IsNotNull(butler.EnterAnimation);
                Assert.IsNotNull(butler.ExitAnimation);

                // when
                butler.UpdateAppearance(ButlerFactory.Rageful, report);

                // then
                Assert.AreEqual(true, butler.HasNews);
                Assert.AreEqual(true, butler.HasMessage);
                Assert.AreEqual("http://localhost/", butler.SourceUrl);
                Assert.AreEqual("aaa bbb", butler.MessageText);
                Assert.AreEqual(null, butler.BalloonTipText);
                Assert.AreEqual(null, butler.BalloonTipTitle);
                Assert.AreEqual(null, butler.Icon);
                Assert.AreEqual(null, butler.Image);
                Assert.AreEqual(ToolTipIcon.None, butler.ToolTipIcon);
                Assert.AreEqual(10, butler.BalloonTipTimeout);
                Assert.AreEqual(true, butler.Topmost);
                Assert.IsNotNull(butler.MessageStyle);
                Assert.IsNotNull(butler.EnterAnimation);
                Assert.IsNotNull(butler.ExitAnimation);
            }
            {
                // when
                Butler butler = ButlerFactory.CreateCalmJenkins();
                butler.UpdateAppearance(ButlerFactory.Normal, report);

                // then
                Assert.AreEqual(true, butler.HasNews);
                Assert.AreEqual(false, butler.HasMessage);
                Assert.AreEqual("http://localhost/", butler.SourceUrl);
                Assert.AreEqual(null, butler.MessageText);
                Assert.AreEqual("aaa bbb", butler.BalloonTipText);
                Assert.AreEqual("ccc", butler.BalloonTipTitle);
                Assert.AreEqual(null, butler.Icon);
                Assert.AreEqual(null, butler.Image);
                Assert.AreEqual(ToolTipIcon.None, butler.ToolTipIcon);
                Assert.AreEqual(10, butler.BalloonTipTimeout);
                Assert.AreEqual(false, butler.Topmost);
                Assert.IsNotNull(butler.MessageStyle);
                Assert.IsNotNull(butler.EnterAnimation);
                Assert.IsNotNull(butler.ExitAnimation);
            }
        }

        [TestMethod]
        public void TestSetErrorMessage()
        {
            {
                // when
                Butler butler = ButlerFactory.CreateEmotionalJenkins();
                butler.SetErrorMessage("message1", "logfile1");

                // then
                Assert.AreEqual(true, butler.HasNews);
                Assert.AreEqual(true, butler.HasMessage);
                Assert.AreEqual("logfile1", butler.SourceUrl);
                Assert.AreEqual("message1", butler.MessageText);
                Assert.AreEqual(null, butler.BalloonTipText);
                Assert.AreEqual(null, butler.BalloonTipTitle);
                Assert.AreEqual(null, butler.Icon);
                Assert.IsNotNull(butler.Image);
                Assert.AreEqual(ToolTipIcon.None, butler.ToolTipIcon);
                Assert.AreEqual(10, butler.BalloonTipTimeout);
                Assert.AreEqual(false, butler.Topmost);
                Assert.IsNotNull(butler.MessageStyle);
                Assert.IsNotNull(butler.EnterAnimation);
                Assert.IsNotNull(butler.ExitAnimation);
            }
            {
                // when
                Butler butler = ButlerFactory.CreateCalmJenkins();
                butler.SetErrorMessage("message1", "logfile1");

                // then
                Assert.AreEqual(true, butler.HasNews);
                Assert.AreEqual(true, butler.HasMessage);
                Assert.AreEqual("logfile1", butler.SourceUrl);
                Assert.AreEqual("message1", butler.MessageText);
                Assert.AreEqual(null, butler.BalloonTipText);
                Assert.AreEqual(null, butler.BalloonTipTitle);
                Assert.AreEqual(null, butler.Icon);
                Assert.IsNotNull(butler.Image);
                Assert.AreEqual(ToolTipIcon.None, butler.ToolTipIcon);
                Assert.AreEqual(10, butler.BalloonTipTimeout);
                Assert.AreEqual(false, butler.Topmost);
                Assert.IsNotNull(butler.MessageStyle);
                Assert.IsNotNull(butler.EnterAnimation);
                Assert.IsNotNull(butler.ExitAnimation);
            }
        }

        [TestMethod]
        public void TestGetInstance()
        {
            // setup
            ButlersFolder.Initialize("xxx");
            Assert.IsTrue(Directory.Exists(ButlersFolder.GetFolder("xxx")));

            {
                // when
                Butler butler = Butler.GetInstance("test1");

                // then
                Assert.AreEqual("test1", butler.Name);
                Assert.AreEqual("test1", butler.DisplayName);
                Assert.AreEqual("test1", butler.Nickname);
                Assert.IsFalse(Directory.Exists(ButlersFolder.GetFolder("test1")));
            }

            {
                // when
                Butler butler = Butler.GetInstance(ButlerFactory.EmotionalJenkins);

                // then
                Assert.AreEqual(ButlerFactory.EmotionalJenkins, butler.Name);
            }
        }

        [TestMethod]
        public void TestReadReport()
        {
            // setup
            ButlersFolder.Initialize("xxx");
            Report report = new Report();
            report.Hashtable["CurrentGrade"] = -1;

            {
                // when
                Butler butler = Butler.GetInstance(ButlerFactory.CalmJenkins);
                butler.ReadReport(report);

                // then
                Assert.AreSame(butler.GetAppearance(ButlerFactory.Happy).Image, butler.Image);
            }

            {
                // when
                Butler butler = Butler.GetInstance(ButlerFactory.EmotionalJenkins);
                butler.ReadReport(report);

                // then
                Assert.AreSame(butler.GetAppearance(ButlerFactory.Sad).Image, butler.Image);
            }

        }
    }
}
