using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class ReportTest
    {
        [TestMethod]
        public void TestThis()
        {
            // setup
            Report report = new Report();

            // when
            report["a"] = "A";
            // then
            Assert.AreEqual("A", report["a"]);

            // when
            report["a"] = "B";
            // then
            Assert.AreEqual("B", report["a"]);

            // when
            report["b"] = "C";
            // then
            Assert.AreEqual("C", report["b"]);
        }

        [TestMethod]
        public void TestHashtableProperty()
        {
            // setup
            Report report = new Report();
            report["a"] = "A";

            // expect
            Assert.AreEqual("A", report.Hashtable["a"]);
        }

        [TestMethod]
        public void TestHashtablePropertyWhenUsingCustomHashtable()
        {
            // setup
            Report report = new Report();
            Hashtable table = new Hashtable();

            // when
            table["a"] = "A";
            report.Hashtable = table;
            // then
            Assert.AreEqual("A", report.Hashtable["a"]);
        }

        [TestMethod]
        public void TestIsUpdated()
        {
            // setup
            Report report = new Report();

            // when
            report.IsUpdated = true;
            // then
            Assert.IsTrue(report.IsUpdated);

            // when
            report.IsUpdated = false;
            // then
            Assert.IsFalse(report.IsUpdated);

            // when
            report["IsUpdated"] = true;
            // then
            Assert.IsTrue((bool)report["IsUpdated"]);
            Assert.IsTrue(report.IsUpdated);
        }

        [TestMethod]
        public void TestSourceUrl()
        {
            // setup
            Report report = new Report();

            // when
            report.SourceUrl = "a";
            // then
            Assert.AreEqual("a", report.SourceUrl);

            // when
            report.SourceUrl = "b";
            // then
            Assert.AreEqual("b", report.SourceUrl);

            // when
            report["SourceUrl"] = "c";
            // then
            Assert.AreEqual("c", report.SourceUrl);
            Assert.AreEqual("c", report["SourceUrl"]);
        }

        [TestMethod]
        public void TestFormat()
        {
            // expect : if format parameter is null or space, then it returns null.
            Assert.AreEqual(null, new Report() { Hashtable = new Hashtable() { { "A", "a" } } }.Format(null));
            Assert.AreEqual(null, new Report() { Hashtable = new Hashtable() { { "A", "a" } } }.Format(""));
            Assert.AreEqual(null, new Report() { Hashtable = new Hashtable() { { "A", "a" } } }.Format(" "));

            // expect : if format parameter is invalid, then it returns null.
            Assert.AreEqual(null, new Report().Format("{x}"));
            Assert.AreEqual(null, new Report() { Hashtable = new Hashtable() }.Format("{x}"));
            Assert.AreEqual(null, new Report() { Hashtable = new Hashtable() { { "A", "a" } } }.Format("{x}"));

            // expect : it returns formated string.
            Assert.AreEqual("x", new Report().Format("x"));
            Assert.AreEqual("xx", new Report() { Hashtable = new Hashtable() }.Format("xx"));
            Assert.AreEqual("abc", new Report() { Hashtable = new Hashtable() { { "A", "b" } } }.Format("a{A}c"));
            Assert.AreEqual("abc", new Report() { Hashtable = new Hashtable() { { "A", "b" } } }.Format("a{a}c")); // ignore case
            Assert.AreEqual("xxbyy", 
                new Report() { Hashtable = new Hashtable() { { "aa", "xx" }, {"cc","yy"} } }.Format("{aa}b{cc}"));
        }
    }
}
