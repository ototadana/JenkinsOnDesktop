using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class LocateTest
    {
        [TestMethod]
        public void TestTargetPropertyType()
        {
            // setup
            Locate locate = new Locate();

            // expect
            Assert.AreEqual(typeof(Position), locate.TargetPropertyType);
        }

        [TestMethod]
        public void TestPosition()
        {
            // setup
            Locate locate = new Locate();

            // when
            locate.Position = Position.RightBottom;

            // then
            Assert.AreEqual(Position.RightBottom, locate.Position);
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            Locate locate = new Locate();

            // then
            Assert.AreEqual(new Duration(TimeSpan.FromMilliseconds(10)), locate.Duration);
            Assert.AreSame(WindowOperator.GetInstance(), Storyboard.GetTarget(locate));
            Assert.AreEqual("Position", Storyboard.GetTargetProperty(locate).Path);
        }

        [TestMethod]
        public void TestGetCurrentValue()
        {
            // setup
            Locate locate = new Locate();

            {
                // when
                locate.Position = Position.RightMiddle;

                // then
                Assert.AreEqual(Position.RightMiddle, locate.GetCurrentValue(null, null, null));
            }

            {
                // when
                locate.Position = Position.LeftTop;

                // then
                Assert.AreEqual(Position.LeftTop, locate.GetCurrentValue(null, null, null));
            }
        }
    }
}
