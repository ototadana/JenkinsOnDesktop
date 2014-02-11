using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class FadeInTest
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
        public void TestPosition()
        {
            // setup
            FadeIn fade = new FadeIn();

            // when
            fade.Position = Position.CenterMiddle;

            // then
            Assert.AreEqual(Position.CenterMiddle, fade.Position);
        }

        [TestMethod]
        public void TestGetCurrentValue()
        {
            // setup
            FadeIn fade = new FadeIn();
            fade.Position = Position.CenterTop;
            MainWindow.Current = new MainWindow();
            MainWindow.Current.Opacity = 0.0;

            // when
            Assert.AreEqual(0.0, fade.GetCurrentValue(null, null, 0.0));
            Assert.AreEqual(0.5, fade.GetCurrentValue(null, null, 0.5));
            Assert.AreEqual(1.0, fade.GetCurrentValue(null, null, 1.0));
            Assert.AreEqual(Position.CenterTop, 
                WindowOperator.GetInstance().GetValue(WindowOperator.PositionProperty));
        }
    }
}
