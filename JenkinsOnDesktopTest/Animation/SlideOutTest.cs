using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.JenkinsOnDesktop.Core;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class SlideOutTest
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
            SlideOut slideOut = new SlideOut();

            // then
            Assert.AreEqual(0.8, slideOut.AccelerationRatio);
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
                SlideOut slideOut = new SlideOut();
                slideOut.Direction = Direction.Left;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(10.0, slideOut.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(-window.ScreenWidth, slideOut.GetCurrentValue(null, null, 1.0));
            }

            {
                // when
                SlideOut slideOut = new SlideOut();
                slideOut.Direction = Direction.Right;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(10.0, slideOut.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(window.ScreenWidth * 2, slideOut.GetCurrentValue(null, null, 1.0));
            }

            {
                // when
                SlideOut slideOut = new SlideOut();
                slideOut.Direction = Direction.Top;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(11.0, slideOut.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(-window.ScreenHeight, slideOut.GetCurrentValue(null, null, 1.0));
            }

            {
                // when
                SlideOut slideOut = new SlideOut();
                slideOut.Direction = Direction.Bottom;
                window.Top = 11.0;
                window.Left = 10.0;

                // then
                Assert.AreEqual(11.0, slideOut.GetCurrentValue(null, null, 0.0));
                Assert.AreEqual(window.ScreenHeight * 2, slideOut.GetCurrentValue(null, null, 1.0));
            }
        }
    }
}
