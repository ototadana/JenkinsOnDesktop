using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Xml;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core
{
    internal class TestUtil
    {
        private const string topDirectory = @"..\..";
        private const string workingDirectory = @"..\test";

        static TestUtil()
        {
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }
        }

        internal static void UpdateWorkspaceFolder(string name)
        {
            string path = Path.GetFullPath(Path.Combine(workingDirectory, name));
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            WorkspaceFolder.SetApplicationPath(path);
        }

        internal static string WorkingDirectory { get { return Path.GetFullPath(workingDirectory); } }

        internal static void Save(object targetObject, string file)
        {
            WorkspaceFolder.SaveObject(targetObject, Path.Combine(workingDirectory, file), true);
        }

        internal static string ToXamlString(object targetObject)
        {
            using (MemoryStream fs = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                XamlWriter.Save(targetObject, writer);
                return Encoding.UTF8.GetString(fs.GetBuffer(), 3, (int)fs.Length-3);
            }
        }

        internal static string GetTestResourcePath(string file)
        {
            return Path.Combine(topDirectory, file);
        }

        internal static string ReadTestResource(string file)
        {
            return File.ReadAllText(GetTestResourcePath(file), Encoding.UTF8);
        }

        internal static void Replace(string file, string oldValue, string newValue)
        {
            string text = File.ReadAllText(file, Encoding.UTF8);
            text = text.Replace(oldValue, newValue);
            File.WriteAllText(file, text, Encoding.UTF8);
        }

        internal static void ClearDirectory(string path)
        {
            Directory.Delete(path, true);
            Assert.IsFalse(Directory.Exists(path));
            Directory.CreateDirectory(path);
            Assert.IsTrue(Directory.Exists(path));
        }

        internal static void TrimFilesIn(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                TrimFile(file);
            }
        }

        internal static void TrimFile(string path)
        {
            File.Delete(path);
            Assert.IsFalse(File.Exists(path));
            File.Create(path).Dispose();
            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual(0, new FileInfo(path).Length);
        }
    }
}
