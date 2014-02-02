using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using XPFriend.JenkinsOnDesktop.Animation;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class AppearanceTest
    {
        [TestMethod]
        public void TestConstructor1()
        {
            // when
            Appearance appearance = new Appearance();

            // then
            Assert.AreEqual(10, appearance.BalloonTipTimeout);
            Assert.IsNotNull(appearance.MessageStyle);
            Assert.AreEqual("jenkins_icon.ico", appearance.IconFile);
            Assert.AreEqual("jenkins.png", appearance.ImageFile);
            Assert.AreEqual(0, appearance.EnterAnimation.Count);
            Assert.AreEqual(0, appearance.ExitAnimation.Count);
            Assert.AreEqual(false, appearance.Topmost);
            Assert.AreEqual(ToolTipIcon.None, appearance.ToolTipIcon);
            Assert.IsNull(appearance.MessageText);
            Assert.IsNull(appearance.BalloonTipText);
            Assert.IsNull(appearance.BalloonTipTitle);
            Assert.IsNull(appearance.Icon);
            Assert.IsNull(appearance.Image);
            Assert.IsNull(appearance.Bitmap);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            // when
            Appearance appearance = new Appearance(
                "title1", "balloonTipText1", "messageText1", "imageFile1", Resources.jenkins);

            // then
            Assert.AreEqual(10, appearance.BalloonTipTimeout);
            Assert.IsNotNull(appearance.MessageStyle);
            Assert.AreEqual("jenkins_icon.ico", appearance.IconFile);
            Assert.AreEqual("imageFile1", appearance.ImageFile);
            Assert.AreEqual(3, appearance.EnterAnimation.Count);
            Assert.AreEqual(2, appearance.ExitAnimation.Count);
            Assert.AreEqual(false, appearance.Topmost);
            Assert.AreEqual(ToolTipIcon.None, appearance.ToolTipIcon);
            Assert.AreEqual("messageText1", appearance.MessageText);
            Assert.AreEqual("balloonTipText1", appearance.BalloonTipText);
            Assert.AreEqual("title1", appearance.BalloonTipTitle);
            Assert.IsNull(appearance.Icon);
            Assert.IsNull(appearance.Image);
            Assert.IsNotNull(appearance.Bitmap);
        }

        [TestMethod]
        public void TestConstructor3()
        {
            // when
            Appearance appearance = new Appearance(
                "title1", "balloonTipText1", "messageText1", null, Resources.jenkins, false);

            // then
            Assert.AreEqual(10, appearance.BalloonTipTimeout);
            Assert.IsNotNull(appearance.MessageStyle);
            Assert.AreEqual("jenkins_icon.ico", appearance.IconFile);
            Assert.AreEqual("jenkins.png", appearance.ImageFile);
            Assert.AreEqual(0, appearance.EnterAnimation.Count);
            Assert.AreEqual(0, appearance.ExitAnimation.Count);
            Assert.AreEqual(false, appearance.Topmost);
            Assert.AreEqual(ToolTipIcon.None, appearance.ToolTipIcon);
            Assert.AreEqual("messageText1", appearance.MessageText);
            Assert.AreEqual("balloonTipText1", appearance.BalloonTipText);
            Assert.AreEqual("title1", appearance.BalloonTipTitle);
            Assert.IsNull(appearance.Icon);
            Assert.IsNull(appearance.Image);
            Assert.IsNotNull(appearance.Bitmap);
        }

        [TestMethod]
        public void TestXamlWithConstructor1()
        {
            // setup
            string expected = TestUtil.ReadTestResource(@"Core\AppearanceTest_01.txt");

            // when
            string actual = TestUtil.ToXamlString(new Appearance());

            // then
            Assert.AreEqual(expected, actual);
       }

        [TestMethod]
        public void TestXamlWithConstructor2()
        {
            // setup
            string expected = TestUtil.ReadTestResource(@"Core\AppearanceTest_02.txt");

            // when
            Appearance appearance = new Appearance(
                "title1", "balloonTipText1", "messageText1", "imageFile1", Resources.jenkins);
            string actual = TestUtil.ToXamlString(appearance);

            // then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetEnterAnimationAsStoryboard()
        {
            // setup
            Appearance appearance = new Appearance();

            // when
            appearance.EnterAnimation.Add(new Operation());
            Storyboard storyboard = appearance.GetEnterAnimationAsStoryboard();

            // then
            Assert.AreEqual(FillBehavior.HoldEnd, storyboard.FillBehavior);
            Assert.AreEqual(1, storyboard.Children.Count);

            Assert.AreEqual(TimeSpan.FromSeconds(0), storyboard.Children[0].BeginTime);
            Assert.AreEqual(Command.None, ((Operation)storyboard.Children[0]).Command);
        }

        [TestMethod]
        public void TestGetEnterAnimationAsStoryboardDefault()
        {
            // setup
            Appearance appearance = new Appearance();

            // when
            Storyboard storyboard = appearance.GetEnterAnimationAsStoryboard();

            // then
            Assert.AreEqual(FillBehavior.HoldEnd, storyboard.FillBehavior);
            Assert.AreEqual(3, storyboard.Children.Count);

            Assert.AreEqual(TimeSpan.FromSeconds(0), storyboard.Children[0].BeginTime);
            Assert.AreEqual(Command.UpdateWindow, ((Operation)storyboard.Children[0]).Command);

            Assert.AreEqual(TimeSpan.FromSeconds(0.1), storyboard.Children[1].BeginTime);
            Assert.AreEqual(new Duration(TimeSpan.FromSeconds(1)), storyboard.Children[1].Duration);
            Assert.AreEqual(Direction.Right, ((SlideIn)storyboard.Children[1]).Direction);
            Assert.AreEqual(Position.LeftBottom, ((SlideIn)storyboard.Children[1]).Position);

            Assert.AreEqual(TimeSpan.FromSeconds(1.2), storyboard.Children[2].BeginTime);
            Assert.AreEqual(Command.ShowMessage, ((Operation)storyboard.Children[2]).Command);
        }

        [TestMethod]
        public void TestGetExitAnimationAsStoryboardDefault()
        {
            // setup
            Appearance appearance = new Appearance();

            // when
            Storyboard storyboard = appearance.GetExitAnimationAsStoryboard();

            // then
            Assert.AreEqual(FillBehavior.HoldEnd, storyboard.FillBehavior);
            Assert.AreEqual(2, storyboard.Children.Count);

            Assert.AreEqual(TimeSpan.FromSeconds(0), storyboard.Children[0].BeginTime);
            Assert.AreEqual(Command.HideMessage, ((Operation)storyboard.Children[0]).Command);

            Assert.AreEqual(TimeSpan.FromSeconds(0.3), storyboard.Children[1].BeginTime);
            Assert.AreEqual(new Duration(TimeSpan.FromSeconds(1)), storyboard.Children[1].Duration);
            Assert.AreEqual(Direction.Left, ((SlideOut)storyboard.Children[1]).Direction);
        }

        [TestMethod]
        public void TestGetExitAnimationAsStoryboard()
        {
            // setup
            Appearance appearance = new Appearance();

            // when
            appearance.ExitAnimation.Add(new Operation());
            Storyboard storyboard = appearance.GetExitAnimationAsStoryboard();

            // then
            Assert.AreEqual(FillBehavior.HoldEnd, storyboard.FillBehavior);
            Assert.AreEqual(1, storyboard.Children.Count);
            Assert.AreEqual(TimeSpan.FromSeconds(0), storyboard.Children[0].BeginTime);
            Assert.AreEqual(Command.None, ((Operation)storyboard.Children[0]).Command);
        }
    }
}
