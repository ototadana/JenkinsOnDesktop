using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class OperationTest
    {
        [TestMethod]
        public void TestCommand()
        {
            // setup
            Operation operation = new Operation();

            // when
            operation.Command = Command.Hide;

            // then
            Assert.AreEqual(Command.Hide, operation.Command);
        }

        [TestMethod]
        public void TestTargetPropertyType()
        {
            // setup
            Operation operation = new Operation();

            // expect
            Assert.AreEqual(typeof(Command), operation.TargetPropertyType);
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            Operation operation = new Operation();

            // then
            Assert.AreEqual(new Duration(TimeSpan.FromMilliseconds(10)), operation.Duration);
            Assert.AreSame(WindowOperator.GetInstance(), Storyboard.GetTarget(operation));
            Assert.AreEqual("Command", Storyboard.GetTargetProperty(operation).Path);
        }

        [TestMethod]
        public void TestGetCurrentValue()
        {
            // setup
            Operation operation = new Operation();

            {
                // when
                operation.Command = Command.HideMessage;

                // then
                Assert.AreEqual(Command.HideMessage, operation.GetCurrentValue(null, null, null));
            }

            {
                // when
                operation.Command = Command.Maximize;

                // then
                Assert.AreEqual(Command.Maximize, operation.GetCurrentValue(null, null, null));
            }
        }
    }
}
