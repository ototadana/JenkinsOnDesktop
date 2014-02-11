using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class MoveAnimationBaseTest
    {
        [TestMethod]
        public void TestDirection()
        {
            // setup
            SlideIn slideIn = new SlideIn();

            // when
            slideIn.Direction = Direction.Bottom;

            // then
            Assert.AreEqual(Direction.Bottom, slideIn.Direction);
        }

        [TestMethod]
        public void TestUpdateProperties()
        {
            // setup
            SlideIn slideIn = new SlideIn();

            {
                // when
                slideIn.Direction = Direction.Left;

                // then
                Assert.AreEqual("Left", Storyboard.GetTargetProperty(slideIn).Path);
            }
            {
                // when
                slideIn.Direction = Direction.Right;

                // then
                Assert.AreEqual("Left", Storyboard.GetTargetProperty(slideIn).Path);
            }
            {
                // when
                slideIn.Direction = Direction.Top;

                // then
                Assert.AreEqual("Top", Storyboard.GetTargetProperty(slideIn).Path);
            }
            {
                // when
                slideIn.Direction = Direction.Bottom;

                // then
                Assert.AreEqual("Top", Storyboard.GetTargetProperty(slideIn).Path);
            }
        }
    }
}
