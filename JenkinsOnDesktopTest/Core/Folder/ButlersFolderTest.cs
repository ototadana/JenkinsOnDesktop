using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    [TestClass]
    public class ButlersFolderTest
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
            string path = ButlersFolder.GetFolder("aaa");

            // then
            Console.WriteLine(path);
            Assert.IsTrue(path.EndsWith("\\Butlers\\aaa"));
        }

        [TestMethod]
        public void TestInitialize()
        {
            {
                // when
                ButlersFolder.Initialize("xxx");

                // then
                string path = ButlersFolder.GetFolder("xxx");
                Console.WriteLine(path);
                Assert.IsTrue(path.EndsWith("\\Butlers\\xxx"));
                AssertEmotionalJenkins(path);

                string parent = Path.GetDirectoryName(path);
                AssertEmotionalJenkins(Path.Combine(parent, "Emotional-Jenkins"));
                AssertCalmJenkins(Path.Combine(parent, "Calm-Jenkins"));
            }

            {
                // when
                string path = ButlersFolder.GetFolder("xxx");
                TestUtil.ClearDirectory(path);
                ButlersFolder.Initialize("xxx");

                // then
                Assert.AreEqual(0, Directory.GetFiles(path).Length);
            }

            { 
                // when
                string path = ButlersFolder.GetFolder("Emotional-Jenkins");
                CustomizeFolder(path);
                ButlersFolder.Initialize("xxx");

                // then
                AssertCustomizedFolder(path, 8);
            }

            {
                // when
                string path = ButlersFolder.GetFolder("Calm-Jenkins");
                CustomizeFolder(path);
                ButlersFolder.Initialize("xxx");

                // then
                AssertCustomizedFolder(path, 4);
            }
        }

        private void AssertCustomizedFolder(string path, int fileCount)
        {
            string[] files = Directory.GetFiles(path);
            Assert.AreEqual(fileCount, files.Length);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) == "jenkins.png")
                {
                    Assert.IsTrue(new FileInfo(file).Length > 0);
                }
                else
                {
                    Assert.AreEqual(0, new FileInfo(file).Length);
                }
            }
        }

        private static void CustomizeFolder(string path)
        {
            string jenkinsPng = Path.Combine(path, "jenkins.png");
            File.Delete(jenkinsPng);
            Assert.IsFalse(File.Exists(jenkinsPng));
            TestUtil.TrimFilesIn(path);
        }

        private static void AssertCalmJenkins(string path)
        {
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(File.Exists(Path.Combine(path, "Butler.xml")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "feel.ps1")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "jenkins.png")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "jenkins_icon.ico")));
        }

        private static void AssertEmotionalJenkins(string path)
        {
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(File.Exists(Path.Combine(path, "Butler.xml")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "feel.ps1")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "jenkins.png")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "jenkins_icon.ico")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "ninja.png")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "oni.png")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "onibi.png")));
            Assert.IsTrue(File.Exists(Path.Combine(path, "sad.png")));
        }

        [TestMethod]
        public void TestSetupFolderIfNotExists()
        {
            {
                // setup
                ButlersFolder.Initialize("abc");
                string folder = ButlersFolder.GetFolder("def");
                Assert.IsFalse(Directory.Exists(folder));

                // when
                ButlersFolder.SetupFolderIfNotExists("def");

                // then
                Assert.IsTrue(Directory.Exists(folder));
            }
            {
                // setup
                ButlersFolder.Initialize("abc");
                string butlerXml = Path.Combine(ButlersFolder.GetFolder("abc"), "Butler.xml");
                Assert.IsTrue(File.Exists(butlerXml));
                File.Delete(butlerXml);
                Assert.IsFalse(File.Exists(butlerXml));

                // when
                ButlersFolder.SetupFolderIfNotExists("abc");

                // then
                Assert.IsFalse(File.Exists(butlerXml));
            }
        }

        [TestMethod]
        public void TestGetFolderNames()
        {
            {
                // when
                ButlersFolder.Initialize("xxx");
                List<string> names = ButlersFolder.GetFolderNames().ToList();

                // then
                names.Sort();
                Assert.AreEqual(3, names.Count);
                Assert.AreEqual("Calm-Jenkins", names[0]);
                Assert.AreEqual("Emotional-Jenkins", names[1]);
                Assert.AreEqual("xxx", names[2]);
            }

            {
                // when
                ButlersFolder.SetupFolderIfNotExists("zzz");
                List<string> names = ButlersFolder.GetFolderNames().ToList();

                // then
                names.Sort();
                Assert.AreEqual(4, names.Count);
                Assert.AreEqual("Calm-Jenkins", names[0]);
                Assert.AreEqual("Emotional-Jenkins", names[1]);
                Assert.AreEqual("xxx", names[2]);
                Assert.AreEqual("zzz", names[3]);
            }
        }

        [TestMethod]
        public void TestLoad()
        {
            {
                // when
                Butler butler = ButlersFolder.Load("xxx");

                // then
                Assert.AreEqual("xxx", butler.Name);
                Assert.AreEqual("xxx", butler.DisplayName);
                Assert.AreEqual("xxx", butler.Nickname);
                Assert.AreSame(butler.Appearances[ButlerFactory.Normal].Image, butler.Image);
                Assert.AreEqual(Resources.jenkins.Width, butler.Image.PixelWidth);
                Assert.AreEqual(Resources.jenkins.Height, butler.Image.PixelHeight);
                Assert.AreEqual(Resources.jenkins_icon.Width, butler.Icon.Width);
                Assert.AreEqual(Resources.jenkins_icon.Height, butler.Icon.Height);
                Assert.IsFalse(Directory.Exists(ButlersFolder.GetFolder("xxx")));
            }
            {
                // setup
                ButlersFolder.Initialize("zzz");
                Assert.IsTrue(Directory.Exists(ButlersFolder.GetFolder("zzz")));

                // when
                string folder = ButlersFolder.GetFolder("zzz");
                TestUtil.Replace(Path.Combine(folder, "Butler.xml"),
                    @"Nickname=""zzz""", @"Nickname=""yyy""");
                TestUtil.Replace(Path.Combine(folder, "Butler.xml"),
                    @"MessageStyle Position=""Right"" Padding=""12,12,12,12"" FontSize=""14"" FontFamily=""Meiryo"" Width=""200"" Height=""200""", 
                    @"MessageStyle Position=""Right"" Padding=""12,12,12,12"" FontSize=""14"" FontFamily=""Meiryo"" BackgroundFile=""sad.png""");

                File.Copy(
                    Path.Combine(folder, "ninja.png"), Path.Combine(folder, "jenkins.png"), true);
                File.Copy(
                    TestUtil.GetTestResourcePath(@"Core\Folder\xpf.ico"), 
                    Path.Combine(folder, "jenkins_icon.ico"), true);
                Butler butler = ButlersFolder.Load("zzz");

                // then
                Assert.AreEqual("zzz", butler.Name);
                Assert.AreEqual("zzz", butler.DisplayName);
                Assert.AreEqual("yyy", butler.Nickname);
                Assert.AreSame(butler.Appearances[ButlerFactory.Normal].Image, butler.Image);
                Assert.AreEqual(Resources.ninja.Width, butler.Image.PixelWidth);
                Assert.AreEqual(Resources.ninja.Height, butler.Image.PixelHeight);
                Assert.AreEqual(32, butler.Icon.Width);
                Assert.AreEqual(32, butler.Icon.Height);
                Assert.AreEqual("sad.png", butler.MessageStyle.BackgroundFile);
                Assert.AreEqual(Resources.sad.Width, butler.MessageStyle.Width);
                Assert.AreEqual(Resources.sad.Height, butler.MessageStyle.Height);
            }
        }

        [TestMethod]
        public void TestOpen()
        {
            // setup
            WorkspaceFolder.processWrapper = new ProcessWrapperStub();
            ButlersFolder.Initialize("abc");
            string folder = ButlersFolder.GetFolder("def");
            Assert.IsFalse(Directory.Exists(folder));

            // when
            ButlersFolder.Open("def");

            // then
            Assert.IsTrue(Directory.Exists(folder));
            Assert.AreEqual(folder, ((ProcessWrapperStub)WorkspaceFolder.processWrapper).fileName);
        }

    }

    internal class ProcessWrapperStub : ProcessWrapper
    {
        internal string fileName;
        internal override void Start(string fileName)
        {
            this.fileName = fileName;
        }
    }
}
