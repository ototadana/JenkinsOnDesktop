using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class WindowAnimationBaseTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            // when
            SlideIn slideIn = new SlideIn();

            // then
            Assert.AreSame(WindowOperator.GetInstance(), Storyboard.GetTarget(slideIn));
            Assert.AreEqual(TimeSpan.FromSeconds(1), slideIn.Duration);
        }
        
        [TestMethod]
        public void TestTargetPropertyType()
        {
            // setup
            SlideIn slideIn = new SlideIn();

            // expect
            Assert.AreEqual(typeof(double), slideIn.TargetPropertyType);
        }
    }
}
