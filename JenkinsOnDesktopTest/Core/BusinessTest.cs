using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using System.Collections;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class BusinessTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            Business business = new Business();

            // then
            Assert.AreEqual(null, business.Name);
            Assert.AreEqual(null, business.Parameters);
            Assert.AreEqual(30, business.TimerInterval);
        }

        [TestMethod]
        public void TestGetInstance()
        {
            // when
            Business business = Business.GetInstance("xxx");

            // then
            Assert.AreEqual("xxx", business.Name);
            Assert.IsNull(business.Parameters);
            Assert.AreEqual(30, business.TimerInterval);
            Assert.IsFalse(Directory.Exists(BusinessesFolder.GetFolder("xxx")));
        }

        [TestMethod]
        public void TestSave()
        {
            // when
            new Business() { Parameters = "abc", TimerInterval = 12 }.Save("zzz");
            Business business = Business.GetInstance("zzz");

            // then
            Assert.AreEqual("zzz", business.Name);
            Assert.AreEqual("abc", business.Parameters);
            Assert.AreEqual(12, business.TimerInterval);
        }

        [TestMethod]
        public void TestCreateReport()
        {
            // setup
            BusinessesFolder.Initialize("zzz");
            Business business = Business.GetInstance(BusinessesFolder.TimeKeeping);

            // when
            Hashtable report = business.CreateReport(new Hashtable());

            // then
            Assert.AreEqual(true, report["IsUpdated"]);
        }
    }
}
