using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class TimelinesTest
    {
        [TestMethod]
        public void TestCapacity()
        {
            // setup
            Timelines timelines = new Timelines();

            // when
            timelines.Capacity = 10;

            // then
            Assert.AreEqual(10, timelines.Capacity);
        }
    }
}
