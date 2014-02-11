using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    [TestClass]
    public class BusinessesFolderTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestMethod]
        public void TestGetFolder()
        {
            // when
            string path = BusinessesFolder.GetFolder("aaa");

            // then
            Console.WriteLine(path);
            Assert.IsTrue(path.EndsWith("\\Businesses\\aaa"));
        }

        [TestMethod]
        public void TestInitialize()
        {
            {
                // when
                BusinessesFolder.Initialize("xxx");

                // then
                string path = BusinessesFolder.GetFolder("xxx");
                Console.WriteLine(path);
                Assert.IsTrue(path.EndsWith("\\Businesses\\xxx"));
                AssertBusinessFiles(path);

                string parent = Path.GetDirectoryName(path);
                AssertBusinessFiles(Path.Combine(parent, "Time-keeping"));
                AssertBusinessFiles(Path.Combine(parent, "Check-job-status"));
            }

            {
                // when
                string path = BusinessesFolder.GetFolder("xxx");
                TestUtil.ClearDirectory(path);
                BusinessesFolder.Initialize("xxx");

                // then
                Assert.AreEqual(0, Directory.GetFiles(path).Length);
            }

            {
                // when
                string path = BusinessesFolder.GetFolder("Time-keeping");
                CustomizeFolder(path);
                BusinessesFolder.Initialize("xxx");

                // then
                AssertCustomizedFolder(path);
            }

            {
                // when
                string path = BusinessesFolder.GetFolder("Check-job-status");
                CustomizeFolder(path);
                BusinessesFolder.Initialize("xxx");

                // then
                AssertCustomizedFolder(path);
            }
        }

        private void AssertCustomizedFolder(string path)
        {
            string[] files = Directory.GetFiles(path);
            Assert.AreEqual(1, files.Length);
            Assert.IsTrue(File.Exists(Path.Combine(path, "main.ps1")));

            string jaJP = Path.Combine(path, "ja-JP");
            Assert.IsTrue(Directory.Exists(jaJP));
            Assert.IsTrue(File.Exists(Path.Combine(jaJP, "main.psd1")));
        }

        private void CustomizeFolder(string path)
        {
            string mainPs1 = Path.Combine(path, "main.ps1");
            File.Delete(mainPs1);
            Assert.IsFalse(File.Exists(mainPs1));
            TestUtil.TrimFilesIn(Path.Combine(path, "ja-JP"));
        }

        private void AssertBusinessFiles(string path)
        {
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(File.Exists(Path.Combine(path, "main.ps1")));

            string jaJP = Path.Combine(path, "ja-JP");
            Assert.IsTrue(Directory.Exists(jaJP));
            Assert.IsTrue(File.Exists(Path.Combine(jaJP, "main.psd1")));
        }

        [TestMethod]
        public void TestSetupFolderIfNotExists()
        {
            {
                // setup
                BusinessesFolder.Initialize("abc");
                string folder = BusinessesFolder.GetFolder("def");
                Assert.IsFalse(Directory.Exists(folder));

                // when
                BusinessesFolder.SetupFolderIfNotExists("def");

                // then
                Assert.IsTrue(Directory.Exists(folder));
            }
            {
                // setup
                BusinessesFolder.Initialize("abc");
                string mainPs1 = Path.Combine(BusinessesFolder.GetFolder("abc"), "main.ps1");
                Assert.IsTrue(File.Exists(mainPs1));
                File.Delete(mainPs1);
                Assert.IsFalse(File.Exists(mainPs1));

                // when
                BusinessesFolder.SetupFolderIfNotExists("abc");

                // then
                Assert.IsFalse(File.Exists(mainPs1));
            }
        }

        [TestMethod]
        public void TestGetFolderNames()
        {
            {
                // when
                BusinessesFolder.Initialize("xxx");
                List<string> names = BusinessesFolder.GetFolderNames().ToList();

                // then
                names.Sort();
                Assert.AreEqual(3, names.Count);
                Assert.AreEqual("Check-job-status", names[0]);
                Assert.AreEqual("Time-keeping", names[1]);
                Assert.AreEqual("xxx", names[2]);
            }

            {
                // when
                BusinessesFolder.SetupFolderIfNotExists("zzz");
                List<string> names = BusinessesFolder.GetFolderNames().ToList();

                // then
                names.Sort();
                Assert.AreEqual(4, names.Count);
                Assert.AreEqual("Check-job-status", names[0]);
                Assert.AreEqual("Time-keeping", names[1]);
                Assert.AreEqual("xxx", names[2]);
                Assert.AreEqual("zzz", names[3]);
            }
        }

        [TestMethod]
        public void TestSaveAndLoad()
        {
            {
                // when
                Business business = BusinessesFolder.Load("xxx");

                // then
                Assert.IsNull(business.Name);
                Assert.IsNull(business.Parameters);
                Assert.AreEqual(30, business.TimerInterval);
                Assert.IsFalse(Directory.Exists(BusinessesFolder.GetFolder("xxx")));
            }

            {
                // when
                BusinessesFolder.Save("zzz", new Business() {Parameters = "abc", TimerInterval=12});

                // then
                Assert.IsTrue(Directory.Exists(BusinessesFolder.GetFolder("zzz")));

                // when
                string folder = BusinessesFolder.GetFolder("zzz");
                Console.Write(folder);
                TestUtil.Replace(Path.Combine(folder, "Config.xml"),
                    @"Parameters=""abc""", @"Parameters=""def""");
                Business business = BusinessesFolder.Load("zzz");

                // then
                Assert.AreEqual("def", business.Parameters);
                Assert.AreEqual(12, business.TimerInterval);
            }
        }

        [TestMethod]
        public void TestOpen()
        {
            // setup
            WorkspaceFolder.processWrapper = new ProcessWrapperStub();
            BusinessesFolder.Initialize("abc");
            string folder = BusinessesFolder.GetFolder("def");
            Assert.IsFalse(Directory.Exists(folder));

            // when
            BusinessesFolder.Open("def");

            // then
            Assert.IsTrue(Directory.Exists(folder));
            Assert.AreEqual(folder, ((ProcessWrapperStub)WorkspaceFolder.processWrapper).fileName);
        }
    }
}
