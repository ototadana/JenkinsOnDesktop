using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class FadeTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            // when
            FadeOut fade = new FadeOut();

            // then
            Assert.AreEqual("Opacity", Storyboard.GetTargetProperty(fade).Path);
        }        
    }
}
