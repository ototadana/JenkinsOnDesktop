using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Media;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class MessageStyleTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            // when
            MessageStyle messageStyle = new MessageStyle();

            // then
            Assert.AreEqual(MessagePosition.Right, messageStyle.Position);
            Assert.AreEqual(new Thickness(12), messageStyle.Padding);
            Assert.AreEqual(new FontFamily("Meiryo"), messageStyle.FontFamily);
            Assert.AreEqual(14, messageStyle.FontSize);
            Assert.AreEqual(0.0, messageStyle.Width);
            Assert.AreEqual(0.0, messageStyle.Height);
            Assert.AreEqual(new Thickness(0), messageStyle.BorderThickness);
            Assert.AreEqual(new CornerRadius(0), messageStyle.CornerRadius);
            Assert.AreEqual(null, messageStyle.BorderBrush);
            Assert.AreEqual(null, messageStyle.Background);
            Assert.AreEqual(null, messageStyle.BackgroundFile);
        }

        [TestMethod]
        public void TestSetDefaultStyle()
        {
            // setup
            MessageStyle messageStyle = new MessageStyle();

            // when
            messageStyle.SetDefaultStyle();

            // then
            Assert.AreEqual(200.0, messageStyle.Width);
            Assert.AreEqual(200.0, messageStyle.Height);
            Assert.AreEqual(new SolidColorBrush(Colors.Black).ToString(), messageStyle.BorderBrush.ToString());
            Assert.AreEqual(new CornerRadius(8), messageStyle.CornerRadius);
            Assert.AreEqual(new SolidColorBrush(Colors.White).ToString(), messageStyle.Background.ToString());
        }

        [TestMethod]
        public void TestXamlWithDefaultStyle()
        {
            // setup
            string expected = TestUtil.ReadTestResource(@"Core\MessageStyleTest_01.txt");

            // when
            string actual = TestUtil.ToXamlString(new MessageStyle());

            // then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestXaml()
        {
            // setup
            string expected = TestUtil.ReadTestResource(@"Core\MessageStyleTest_01.txt");

            // when
            string actual = TestUtil.ToXamlString(new MessageStyle());

            // then
            Assert.AreEqual(expected, actual);
        }
    }
}
