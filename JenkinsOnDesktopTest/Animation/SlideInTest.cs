using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.JenkinsOnDesktop.Core;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class SlideInTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            if (MainWindow.Current != null)
            {
                MainWindow.Current.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            SlideIn slideIn = new SlideIn();

            // then
            Assert.AreEqual(0.8, slideIn.DecelerationRatio);
        }

        [TestMethod]
        public void TestPosition()
        {
            // setup
            SlideIn slideIn = new SlideIn();

            // when
            slideIn.Position = Position.LeftMiddle;

            // then
            Assert.AreEqual(Position.LeftMiddle, slideIn.Position);
        }

        [TestMethod]
        public void TestGetCurrentValue()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            {
                // when
                SlideIn slideIn = new SlideIn();
                slideIn.Position = Position.LeftTop;
                slideIn.Direction = Direction.Left;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(window.ScreenWidth * 2, slideIn.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(margin, slideIn.GetCurrentValue(null, null, 1.0));
                Assert.AreEqual(margin, window.Top);
                Assert.AreEqual(10.0, window.Left);
            }

            {
                // when
                SlideIn slideIn = new SlideIn();
                slideIn.Position = Position.RightTop;
                slideIn.Direction = Direction.Right;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(-window.ScreenWidth, slideIn.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(window.ScreenWidth - margin, slideIn.GetCurrentValue(null, null, 1.0));
                Assert.AreEqual(margin, window.Top);
                Assert.AreEqual(10.0, window.Left);
            }

            {
                // when
                SlideIn slideIn = new SlideIn();
                slideIn.Position = Position.RightBottom;
                slideIn.Direction = Direction.Bottom;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(-window.ScreenHeight, slideIn.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(window.ScreenHeight - margin, slideIn.GetCurrentValue(null, null, 1.0));
                Assert.AreEqual(window.ScreenWidth - margin, window.Left);
                Assert.AreEqual(11.0, window.Top);
            }

            {
                // when
                SlideIn slideIn = new SlideIn();
                slideIn.Position = Position.LeftBottom;
                slideIn.Direction = Direction.Top;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(window.ScreenHeight * 2, slideIn.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(window.ScreenHeight - margin, slideIn.GetCurrentValue(null, null, 1.0));
                Assert.AreEqual(margin, window.Left);
                Assert.AreEqual(11.0, window.Top);
            }
        }
    }
}
