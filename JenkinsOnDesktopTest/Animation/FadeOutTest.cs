using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class FadeOutTest
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
        public void TestGetCurrentValue()
        {
            // setup
            FadeOut fade = new FadeOut();
            MainWindow.Current = new MainWindow();
            MainWindow.Current.Opacity = 1.0;

            // when
            Assert.AreEqual(1.0, fade.GetCurrentValue(null, null, 0.0));
            Assert.AreEqual(0.5, fade.GetCurrentValue(null, null, 0.5));
            Assert.AreEqual(0.0, fade.GetCurrentValue(null, null, 1.0));
        }
    }
}
